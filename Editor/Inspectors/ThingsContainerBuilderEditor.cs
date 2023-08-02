using System;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(ThingsContainerBuilder<,,,,>), true)]
public class ThingsContainerBuilderEditor : Editor
{
    private readonly static string viewDataKey = "ContextBuilderFoldout_" + Guid.NewGuid().ToString();

    StyleSheet styleSheet;
    public override VisualElement CreateInspectorGUI()
    {
        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("./Style/ShireSoft.uss");

        // Create property container element.
        var container = new Foldout();
        container.styleSheets.Add(styleSheet);
        container.viewDataKey = viewDataKey + serializedObject.targetObject.name;
        var toggle = container.Q<Toggle>();
        toggle.AddToClassList("left-padded");
        container.text = serializedObject.targetObject.name;

        container.AddToClassList("base");
        container.AddToClassList("header");

        var objectsField = ListDrawer.Draw(serializedObject, "m_Objects");
        container.Add(objectsField);

        return container;
    }
}