using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptableObjectCreationWindow : EditorWindow
{
    #region Constants

    string DEFAULT_PATH
    {
        get
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            string pathToCurrentFolder = obj.ToString();
            return pathToCurrentFolder;
        }
    }
    const string DEFAULT_TEXT = "No Type Selected!";
    const string DEFAULT_NAME = "NewAsset";
    const string WINDOW_NAME = "Thing Tool";
    const string WINDOW_MENU = "Shiresoft/";
    const string WINDOW_PATH = "Window/" + WINDOW_MENU + WINDOW_NAME;
    const string HELP_MESSAGE = "Select a class to create an asset of.";
    const string PATH_ERROR_MESSAGE = "Invalid path. Can't create asset.";
    const string TYPE_ERROR_MESSAGE = "Invalid or unselected type. Can't create asset.";
    readonly List<Type> EXCLUDED_TYPES = new List<Type>
    {
        typeof(EditorWindow),
        typeof(Editor),
    };

    #endregion

    #region Fields & Properties

    [SerializeField] VisualTreeAsset m_TreeAsset;
    [SerializeField] VisualTreeAsset m_HeaderButton;
    [SerializeField] VisualTreeAsset m_ListButton;
    [SerializeField] VisualTreeAsset m_ListFoldout;

    TemplateContainer root;

    Button createButton;
    VisualElement header;
    ScrollView scrollView;
    DropdownField dropdown;
    TextField assetNameField;
    TextField pathField;
    Toggle overrideToggle;
    Toolbar helpBox;
    Label selectedField;
    Label errorMessage;
    SliderInt copiesCountSlider;

    VisualElement selectedButtonContainer;
    VisualElement selectedHeaderButtonContainer;

    Type m_SelectedType;
    Type m_SelectedHeader;
    Type m_SelectedSubtype;

    string m_AssetName = DEFAULT_NAME;
    bool overrideDefaultPath = false;
    bool validPath = true;
    Action<ScriptableObject> m_OnAssetCreated;

    // K = Type in hierarchy
    // V = Depth in hierarchy tree
    Dictionary<Type, int> m_Depth = new();

    // K = Type in hierarchy
    // V = Parent Type in hierarchy (direct baseType)
    Dictionary<Type, Type> m_Parents = new();

    // K = Type in hierarchy
    // V = List of child Types
    Dictionary<Type, List<Type>> m_Hierarchy = new();

    // K = Type in hierarchy
    // V = VisualElement Parent in VisualTree
    Dictionary<Type, VisualElement> m_VisualParents = new();

    // K = Type in hierarchy
    // V = ButtonConteinr for type
    Dictionary<Type, VisualElement> m_Buttons = new();

    // K = HeaderType
    // V = Last selected SubType index
    Dictionary<Type, int> m_SubtypeIndex;

    // Current template asset 
    ScriptableObject m_TemplateAsset;

    public Assembly[] Assemblies { get => System.AppDomain.CurrentDomain.GetAssemblies(); }
    public List<Type> Types 
    { 
        get
        {
            List<Type> types = new();
            foreach(var assembly in Assemblies)
            {
                types.AddRange(assembly.GetTypes().Where(t => !t.IsAbstract && !t.IsGenericType && t.IsSubclassOf(typeof(ScriptableObject))).ToList());
            }
            return types;
        }
    } 
    public List<Type> HeaderTypes { get => m_Depth.Where(x => x.Value == 0).Select(x => x.Key).ToList(); }
    public List<Type> SubTypes { get => m_Hierarchy.ContainsKey(m_SelectedHeader) ? m_Hierarchy[m_SelectedHeader] : new List<Type>(); }
    public List<Type> ContentTypes
    {
        get
        {
            return m_SelectedSubtype == null ?
                // If no subtype, traverse from the header because the first subtype is buildable
                TraverseHierarchy(m_SelectedHeader, addRoot: false) :
                // elseif subtype, traverse from the subtype because the subtype is not buildable
                TraverseHierarchy(m_SelectedSubtype, addRoot: false);

        }
    }

    #endregion

    #region Window Implementation

    [MenuItem(WINDOW_PATH)]
    public static void ShowWindow()
    {
        ScriptableObjectCreationWindow window = GetWindow<ScriptableObjectCreationWindow>();
        window.titleContent = new GUIContent(WINDOW_NAME);
    }

    void EnsurePath()
    {
        overrideDefaultPath = overrideToggle.value;
        pathField.SetEnabled(overrideDefaultPath);
        var desiredPath = overrideDefaultPath ? pathField.value : DEFAULT_PATH;
        CheckPath(desiredPath);
    }

    KeyValuePair<KeyValuePair<string, string>, int> GetEnumeratorSuffix(string name)
    {
        var spacePair = new KeyValuePair<string, string>(" ", "");
        var underscorePair = new KeyValuePair<string, string>("_", "");
        var parenthesesPair = new KeyValuePair<string, string>("(", ")");
        var spaceParenthesesPair = new KeyValuePair<string, string>(" (", ")");
        var bracketsPair = new KeyValuePair<string, string>("[", "]");
        var spaceBracketsPair = new KeyValuePair<string, string>(" [", "]");
        var bracesPair = new KeyValuePair<string, string>("{", "}");
        var spaceBracesPair = new KeyValuePair<string, string>(" {", "}");

        var underscore = GetEnumeratorValue(name, underscorePair.Key);
        var space = GetEnumeratorValue(name, spacePair.Key);
        var parentheses = GetEnumeratorValue(name, parenthesesPair.Key, parenthesesPair.Value);
        var brackets = GetEnumeratorValue(name, bracketsPair.Key, bracketsPair.Value);
        var braces = GetEnumeratorValue(name, bracesPair.Key, bracesPair.Value);
        var spaceParentheses = GetEnumeratorValue(name, spaceParenthesesPair.Key, spaceParenthesesPair.Value);
        var spaceBrackets = GetEnumeratorValue(name, spaceBracketsPair.Key, spaceBracketsPair.Value);
        var spaceBraces = GetEnumeratorValue(name, spaceBracesPair.Key, spaceBracesPair.Value);


        var spaceRes = new KeyValuePair<KeyValuePair<string, string>, int>(spacePair, space);
        var underscoreRes = new KeyValuePair<KeyValuePair<string, string>, int>(underscorePair, underscore);
        var parenthesesRes = new KeyValuePair<KeyValuePair<string, string>, int>(parenthesesPair, parentheses);
        var spaceParenthesesRes = new KeyValuePair<KeyValuePair<string, string>, int>(spaceParenthesesPair, spaceParentheses);
        var bracketsRes = new KeyValuePair<KeyValuePair<string, string>, int>(bracketsPair, brackets);
        var spaceBracketsRes = new KeyValuePair<KeyValuePair<string, string>, int>(spaceBracketsPair, spaceBrackets);
        var bracesRes = new KeyValuePair<KeyValuePair<string, string>, int>(bracesPair, braces);
        var spaceBracesRes = new KeyValuePair<KeyValuePair<string, string>, int>(spaceBracesPair, spaceBraces);

        var results = new List<KeyValuePair<KeyValuePair<string, string>, int>>()
        {
            spaceRes,
            underscoreRes,
            parenthesesRes,
            spaceParenthesesRes,
            bracketsRes,
            spaceBracketsRes,
            bracesRes,
            spaceBracesRes
        };

        var first = results.FirstOrDefault(x => x.Value != -1);
        if (first.Value == default) return new KeyValuePair<KeyValuePair<string, string>, int>(new KeyValuePair<string, string>("", ""), -1);
        else return first;
    }

    int GetEnumeratorValue(string name, string separator, string endcap = "")
    {
        var isSeparated = name.LastIndexOf(separator);
        if (isSeparated == -1) return -1;
        else
        {
            var shouldHaveEndcap = endcap != "";
            var hasCorrectEndCap = name.LastIndexOf(endcap) == name.Length - 1;

            if (shouldHaveEndcap && !hasCorrectEndCap) return -1;

            var endValue = name.Length - isSeparated - (shouldHaveEndcap ? 2 : 1);
            var value = name.Substring(isSeparated + 1, endValue);
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            else return -1;
        }
    }

    private Type MoveAtDepthFrom(int depth, Type type)
    {
        if (depth < 0) return null;
        if (depth == m_Depth[type]) return type;
        else if (depth < m_Depth[type])
        {
            int currentDepth = m_Depth[type];
            for (Type _type = type; m_Hierarchy.ContainsKey(_type) && currentDepth >= 0; _type = m_Parents[_type])
            {
                currentDepth = m_Depth[_type];
                if (currentDepth == depth) return _type;
            }
        }

        return null;
    }

    private void OnSelectionChanged()
    {
        var obj = Selection.activeObject;
        if (obj != null)
        {
            CheckSelection(obj);
        }
    }

    private bool CheckSelection(UnityEngine.Object obj)
    {
        var selectedType = obj.GetType();
        var validType = selectedType != null && Types.Contains(selectedType);
        var isTemplate = validType && obj.Equals(m_TemplateAsset);

        var res = validType && !isTemplate;

        if (validType && !isTemplate)
        {
            m_SelectedType = selectedType;
            m_SelectedHeader = MoveAtDepthFrom(0, m_SelectedType);
            m_SelectedSubtype = MoveAtDepthFrom(1, m_SelectedType);
            m_SubtypeIndex[m_SelectedHeader] = SubTypes.IndexOf(m_SelectedSubtype);
            RefreshHeader();
            createButton.text = "Clone";
            createButton.SetEnabled(true);
            selectedField.text = TypeToName(m_SelectedType);
        }
        else
        {
            createButton.text = "Create";
        }

        return res;
    }

    private void OnEnable()
    {
        root = m_TreeAsset.CloneTree();
        header = root.Q<VisualElement>("Header");
        dropdown = root.Q<DropdownField>("Dropdown");
        scrollView = root.Q<ScrollView>("ScrollView");
        selectedField = root.Q<Label>("Type");
        assetNameField = root.Q<TextField>("NameFieldInput");
        pathField = root.Q<TextField>("PathFieldInput");
        createButton = root.Q<Button>("CreateButton");
        overrideToggle = root.Q<Toggle>("OverridePathToggle");
        copiesCountSlider = root.Q<SliderInt>("SliderInt");
        helpBox = root.Q<Toolbar>("ErrorToolbar");
        helpBox.style.display = DisplayStyle.None;
        errorMessage = helpBox.Q<Label>("Message");
        errorMessage.text = PATH_ERROR_MESSAGE;

        Selection.selectionChanged += OnSelectionChanged;

        dropdown.RegisterValueChangedCallback(x =>
        {
            m_SelectedSubtype = NameToType(x.newValue);
            RefreshContent();

            if (m_SelectedHeader != null)
            {
                m_SubtypeIndex[m_SelectedHeader] = dropdown.index;
            }
        });

        EnsurePath();

        overrideToggle.RegisterValueChangedCallback(x =>
        {
            EnsurePath();
        });

        m_Depth.Clear();
        m_Parents.Clear();
        m_VisualParents.Clear();
        m_Hierarchy.Clear();
        m_Hierarchy = CreateHierarchy(Types);

        CheckDepth();

        m_SubtypeIndex = HeaderTypes.ToDictionary(key => key, value => 0);
        m_SelectedHeader = HeaderTypes.Count > 0 ? HeaderTypes[0] : null;
        m_SelectedSubtype = SubTypes.Count > 0 ? SubTypes[0] : null;
        m_SelectedType = null;
        selectedField.text = DEFAULT_TEXT;
        createButton.SetEnabled(false);


        var selectedObject = Selection.activeObject;
        if (selectedObject == null || !CheckSelection(selectedObject))
        {
            RefreshHeader();
        }

        /*        CreateVisualElements(classesScrollView);*/

        // Handle asset name input
        assetNameField.RegisterValueChangedCallback(evt =>
        {
            m_AssetName = evt.newValue.ToString();
        });

        pathField.RegisterValueChangedCallback(evt =>
        {
            var flag = CheckPath(evt.newValue.ToString());
            createButton.SetEnabled(flag && m_SelectedType != null);
        });

        // Handle creation of asset button
        createButton.clickable.clicked += () =>
        {
            var hasSelectedType = m_SelectedType != null;
            var hasTemplateObject = m_TemplateAsset != null && m_TemplateAsset is not null;

            if (!(hasSelectedType || hasTemplateObject))
            {
                ShowHelpBox(true, TYPE_ERROR_MESSAGE);
                createButton.SetEnabled(false);
                return;
            }

            m_AssetName = assetNameField.value;

            var desiredPath = overrideDefaultPath ? pathField.value : DEFAULT_PATH;
            var fileExists = File.Exists($"{desiredPath}/{m_AssetName}.asset");
            var index = 0;
            var count = copiesCountSlider.value;

            var detectFormat = GetEnumeratorSuffix(m_AssetName);
            var enumerationFormat = new KeyValuePair<string, string>(" (", ")");
            if (detectFormat.Value != -1)
            {
                enumerationFormat = detectFormat.Key;
                index = detectFormat.Value;
                m_AssetName = m_AssetName.Split(enumerationFormat.Key)[0];
            }

            var hasAssetTemplate = m_TemplateAsset != null && m_TemplateAsset is not null;
            var hasAssetToClone = Selection.activeObject?.GetType()?.Equals(m_SelectedType) ?? false;

            while (count > 0)
            {
                if (!fileExists)
                {

                    var enumerationSuffix = index > 0 ? $"{enumerationFormat.Key}{index}{enumerationFormat.Value}" : "";
                    ScriptableObject asset = hasAssetToClone ? (ScriptableObject)Instantiate(Selection.activeObject) : hasAssetTemplate ? Instantiate(m_TemplateAsset) : CreateInstance(m_SelectedType);
                    AssetDatabase.CreateAsset(asset, $"{desiredPath}/{m_AssetName}{enumerationSuffix}.asset");
                    m_OnAssetCreated?.Invoke(asset);

                    count--;
                }
                index++;
                fileExists = File.Exists($"{desiredPath}/{m_AssetName}{enumerationFormat.Key}{index}{enumerationFormat.Value}.asset");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ForceReserializeAssets();
        };

        // style the root to fill up the whole page
        root.style.flexGrow = 1;
        root.style.flexShrink = 1;

        // Add the root to the window
        rootVisualElement.Add(root);
    }

    bool CheckPath(string path)
    {
        var flag = Directory.Exists(path);
        ShowHelpBox(!flag);
        return flag;
    }

    void ShowHelpBox(bool show, string text = PATH_ERROR_MESSAGE)
    {
        errorMessage.text = text;
        helpBox.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }

    #endregion

    #region Reflection & Trees

    List<Type> TraverseHierarchy(Type start, bool addRoot = true)
    {
        var path = new List<Type>();
        if (addRoot || (!start.IsAbstract && !start.IsGenericType))
        {
            path.Add(start);
        }

        foreach (var type in m_Hierarchy[start])
        {
            path.AddRange(TraverseHierarchy(type));
        }

        return path
                .OrderBy(x => -m_Hierarchy[x].Count)
                .ThenBy(x => TypeToName(x))
                .ThenBy(x => m_Depth.ContainsKey(x) ? m_Depth[x] : 0)
                .ToList();
    }

    void AddParent(Type type, Dictionary<Type, List<Type>> hierarchy)
    {
        var current = type;
        var baseType = type.BaseType;

        if (baseType.IsConstructedGenericType)
        {
            baseType = baseType.GetGenericTypeDefinition();
        }

        while (!(baseType.Equals(typeof(ScriptableObject)) || !baseType.IsSubclassOf(typeof(ScriptableObject))))
        {
            if (!hierarchy.ContainsKey(baseType))
            {
                hierarchy.Add(baseType, new List<Type>());
            }

            if (!hierarchy[baseType].Contains(current))
            {
                hierarchy[baseType].Add(current);
            }

            if (!m_Parents.ContainsKey(current))
            {
                m_Parents.Add(current, baseType);
            }

            current = baseType;
            baseType = baseType.BaseType;

            if (baseType.IsConstructedGenericType)
            {
                baseType = baseType.GetGenericTypeDefinition();
            }
        }

        if (!hierarchy.ContainsKey(current))
        {
            hierarchy.Add(current, new List<Type>());
        }

        if (!baseType.Equals(typeof(ScriptableObject)))
        {
            if (!hierarchy.ContainsKey(baseType))
            {
                hierarchy.Add(baseType, new List<Type>());
            }
            else
            {
                hierarchy[baseType].Add(current);
            }
        }
    }

    Dictionary<Type, List<Type>> CreateHierarchy(List<Type> types)
    {
        Dictionary<Type, List<Type>> hierarchy = new();
        var actualtypes = types.Except(EXCLUDED_TYPES);

        foreach (var type in actualtypes)
        {
            AddParent(type, hierarchy);
            if (!hierarchy.ContainsKey(type))
            {
                hierarchy.Add(type, new List<Type>());
            }
        }

        return hierarchy;
    }

    int DetectDepth(Type type)
    {
        if (!m_Parents.ContainsKey(type))
        {
            if (m_Depth.ContainsKey(type))
            {
                m_Depth[type] = 0;
            }
            else
            {
                m_Depth.Add(type, 0);
            }

            return m_Depth[type];
        }
        else
        {
            if (!m_Depth.ContainsKey(type))
            {
                m_Depth.Add(type, DetectDepth(m_Parents[type]) + 1);
            }

            return m_Depth[type];
        }
    }

    void CheckDepth()
    {
        foreach (Type type in m_Parents.Values)
        {
            var depth = DetectDepth(type);
            Debug.Log(type.Name + ": " + depth);
        }

        foreach (Type type in m_Parents.Keys)
        {
            var depth = DetectDepth(type);
        }

        foreach (var excludedType in EXCLUDED_TYPES)
        {
            m_Depth.Remove(excludedType);
        }
    }

    #endregion

    #region Type Utilities
    string TypeToName(Type type)
    {
        return type.IsGenericType ? type.Name.Split("`")[0] : type.Name;
    }

    bool IsTypeName(string name, Type type)
    {
        return TypeToName(type) == name;
    }

    Type NameToType(string name)
    {
        return m_Hierarchy.Select(x => x.Key).Where(x => IsTypeName(name, x)).FirstOrDefault();
    }

    #endregion

    #region Visual Elements Prcedures

    void RefreshHeader()
    {
        header.Clear();
        if (HeaderTypes.Count <= 0) return;

        foreach (var headerType in HeaderTypes)
        {
            var buttonContainer = m_HeaderButton.CloneTree().ElementAt(0);
            var button = buttonContainer.Q<Button>();

            button.text = TypeToName(headerType);
            button.clicked += () =>
                {
                    m_SelectedHeader = headerType;
                    buttonContainer.AddToClassList("selected-header-button-container");
                    RefreshDropdown();

                    if (selectedHeaderButtonContainer != null)
                    {
                        selectedHeaderButtonContainer.RemoveFromClassList("selected-header-button-container");
                    }

                    selectedHeaderButtonContainer = buttonContainer;
                };

            if (m_SelectedHeader == headerType)
            {
                buttonContainer.AddToClassList("selected-header-button-container");
                RefreshDropdown();

                if (selectedHeaderButtonContainer != null)
                {
                    selectedHeaderButtonContainer.RemoveFromClassList("selected-header-button-container");
                }

                selectedHeaderButtonContainer = buttonContainer;
            }
            header.Add(buttonContainer);
        }

        RefreshDropdown();
    }

    void RefreshDropdown()
    {
        if (m_SelectedHeader == null)
        {
            dropdown.choices = new List<string>();
        }
        else
        {
            dropdown.choices = SubTypes.Select(x => TypeToName(x)).ToList();
            var prevIndex = m_SubtypeIndex[m_SelectedHeader];
            dropdown.index = prevIndex;
            m_SelectedSubtype ??= SubTypes[prevIndex];
        }

        RefreshContent();
    }

    void RefreshContent()
    {
        m_VisualParents.Clear();
        m_Buttons.Clear();

        if (m_SelectedHeader == null)
        {
            var helpBox = new HelpBox(HELP_MESSAGE, HelpBoxMessageType.Info);
            scrollView.Add(helpBox);
            return;
        }

        scrollView.Clear();

        foreach (Type type in ContentTypes)
        {
            if (m_VisualParents.ContainsKey(type)) continue;

            if (m_Hierarchy[type].Count > 0)
            {
                var foldout = AddFoldout(type);
                m_VisualParents.Add(type, foldout);
            }
            else
            {
                var leaf = AddLeaf(type);
                m_VisualParents.Add(type, leaf);
            }
        }

        foreach (KeyValuePair<Type, VisualElement> entry in m_VisualParents)
        {
            var key = entry.Key;
            var value = entry.Value;

            scrollView.Add(value);
        }

        if (m_SelectedType != null && ContentTypes.Contains(m_SelectedType))
        {
            var buttonContainer = m_Buttons[m_SelectedType];
            buttonContainer.AddToClassList("selected-list-button");

            if (selectedButtonContainer != null)
            {
                selectedButtonContainer.RemoveFromClassList("selected-list-button");
            }

            selectedButtonContainer = buttonContainer;
            var parent = selectedButtonContainer.parent;
            while (parent != null && parent is Foldout)
            {
                (parent as Foldout).value = true;
                parent = parent.parent;
            }

            assetNameField.value = TypeToName(m_SelectedType);
        }
    }

    VisualElement AddFoldout(Type type, VisualElement parent = null)
    {
        var foldout = m_ListFoldout.CloneTree().ElementAt(0) as Foldout;
        var typeName = type.IsGenericType ? type.Name.Split("`")[0] : type.Name;
        foldout.text = typeName;

        foreach (Type subType in m_Hierarchy[type])
        {
            if (m_Hierarchy[subType].Count > 0)
            {
                AddFoldout(subType, foldout);
            }
            else
            {
                AddLeaf(subType, foldout);
            }
        }


        if (parent != null)
        {
            if (!m_VisualParents.ContainsKey(type))
            {
                m_VisualParents.Add(type, foldout);
            }

            parent.Add(foldout);
        }

        foldout.value = false;
        return foldout;
    }

    VisualElement AddLeaf(Type type, VisualElement parent = null)
    {
        if (m_VisualParents.ContainsKey(type)) return m_VisualParents[type];

        var buttonContainer = m_ListButton.CloneTree().ElementAt(0);
        var button = buttonContainer.Q<Button>();

        button.clicked += (() =>
        {
            m_SelectedType = type;
            selectedField.text = TypeToName(type);
            buttonContainer.AddToClassList("selected-list-button");

            if (selectedButtonContainer != null)
            {
                selectedButtonContainer.RemoveFromClassList("selected-list-button");
            }

            selectedButtonContainer = buttonContainer;

            if (m_TemplateAsset != null)
            {
                DestroyImmediate(m_TemplateAsset);
            }

            m_TemplateAsset = CreateInstance(type);
            createButton.SetEnabled(true);
            Selection.activeObject = m_TemplateAsset;
            assetNameField.value = TypeToName(m_SelectedType);
        });

        button.text = TypeToName(type);

        if (!m_Buttons.ContainsKey(type))
        {
            m_Buttons.Add(type, buttonContainer);
        }

        if (parent != null)
        {
            m_VisualParents.Add(type, parent);
            parent.Insert(0, buttonContainer);
        }

        return buttonContainer;
    }

    #endregion
}
