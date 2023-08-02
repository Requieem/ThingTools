
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CanEditMultipleObjects]
public abstract class EntityBuilderEditor : Editor
{
    StyleSheet styleSheet;
    private readonly static string infoDataKey = "EntityInfoFoldout_" + Guid.NewGuid().ToString();
    private readonly static string containersDataKey = "EntityContainersFoldout_" + Guid.NewGuid().ToString();

    public override VisualElement CreateInspectorGUI()
    {
        var displayableProp = serializedObject.FindProperty("m_Displayable");
        var displayableField = new PropertyField(displayableProp);
        displayableField.BindProperty(displayableProp);

        m_StyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(PathConstants.StylePath);

        var baseContainer = new VisualElement();
        baseContainer.styleSheets.Add(styleSheet);
        baseContainer.Add(displayableField);

        return baseContainer;
    }
}
