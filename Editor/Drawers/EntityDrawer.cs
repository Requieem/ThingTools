using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Entity<,>), true)]
public class EntityDrawer : PropertyDrawer
{
    private static readonly string viewDataKey = "EntityFoldout_" + Guid.NewGuid().ToString();
    StyleSheet styleSheet;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/ShireSoft/Editor/Style/ShireSoft.uss");

        var container = new VisualElement();
        container.styleSheets.Add(styleSheet);
        container.viewDataKey = viewDataKey + property.serializedObject.targetObject.name;
        var entityField = new PropertyField(property);
        entityField.BindProperty(property);

        container.Add(entityField);

        return container;
    }
}