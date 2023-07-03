using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class ObjectPicker
{
    public static PropertyField Field(SerializedProperty property, string label = "")
    {
        var field = new PropertyField(property, label);
        field.AddToClassList("object-picker");
        field.BindProperty(property);

        return field;
    }

    public static PropertyField Field(SerializedObject obj, string propertyPath, string label = "")
    {
        var prop = obj.FindProperty(propertyPath);

        if (prop != null)
        {
            return Field(prop, label);
        }
        else
        {
            return null;
        }
    }

    public static void AddTo(VisualElement container, PropertyField field)
    {
        if (field != null)
        {
            container.Add(field);
        }
    }
}
