using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Entity<,>), true)]
public class EntityDrawer : PropertyDrawer
{
    private static readonly string viewDataKey = "EntityFoldout_" + Guid.NewGuid().ToString();
    public StyleSheet m_StyleSheet;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        m_StyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.shiresoft.thingtools/Editor/Style/ShireSoft.uss");

        var container = new VisualElement();
        container.styleSheets.Add(m_StyleSheet);
        container.viewDataKey = viewDataKey + property.serializedObject.targetObject.name;
        var entityField = new PropertyField(property);
        entityField.BindProperty(property);

        container.Add(entityField);

        return container;
    }
}