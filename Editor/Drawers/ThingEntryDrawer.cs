using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ThingEntry<,>), true)]
public class ThingEntryDrawer : PropertyDrawer
{
    private readonly static string viewDataKey = "ContextObjectFoldout_" + Guid.NewGuid().ToString();
    private readonly static int random = new System.Random().Next();
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new Foldout();
        container.viewDataKey = viewDataKey + property.serializedObject.targetObject.name + random.ToString();
        container.AddToClassList("list-base");
        var content = container.Query(className: "unity-foldout__content").First();
        content.AddToClassList("list-content");
        var toggle = container.Q<Toggle>();
        toggle.AddToClassList("left-padded");
        container.style.backgroundColor = new StyleColor(new Color(1, 1, 1, 0.05f));

        var keyProp = property.FindPropertyRelative("m_Key");
        var valueProp = property.FindPropertyRelative("m_Value");

        var keyField = new PropertyField(keyProp);
        keyField.RegisterValueChangeCallback((evt) =>
        {
            var enumNames = keyProp.enumNames;
            var enumValue = enumNames is not null && enumNames.Length > 0 ? enumNames[keyProp.enumValueIndex] : null;
            container.text = enumValue is not null ? enumValue : keyProp?.objectReferenceValue?.name ?? "NULL";
            keyField.MarkDirtyRepaint();
        });

        keyField.BindProperty(keyProp);
        var valueField = new PropertyField(valueProp);
        valueField.BindProperty(valueProp);

        valueField.style.marginBottom = 10;
        var enumNames = keyProp.enumNames;
        var enumValue = enumNames is not null && enumNames.Length > 0 ? enumNames[keyProp.enumValueIndex] : null;
        container.text = enumValue is not null ? enumValue : keyProp?.objectReferenceValue?.name ?? "NULL";

        container.Add(keyField);
        container.Add(valueField);

        return container;
    }
}