using System;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(ObjectContainerBuilder<,,,>), true)]
public class ObjectContainerBuilderEditor : Editor
{
    private readonly static string viewDataKey = "ObjectBuilderFoldout_" + Guid.NewGuid().ToString();

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

        var label = new Label("Objects");
        label.AddToClassList("header");
        label.style.marginTop = 20;
        container.Add(label);

        var objectsField = ListDrawer.Draw(serializedObject, "m_Objects");
        objectsField.AddToClassList("inner-list");
        container.Add(objectsField);

        return container;
    }
}