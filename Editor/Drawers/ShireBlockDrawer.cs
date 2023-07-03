using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

// IngredientDrawerUIE
[CustomPropertyDrawer(typeof(ShireBlock), true)]
public class ShireBlockDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();
        container.AddToClassList("inner-base");
        container.style.backgroundColor = new StyleColor(new Color(1, 1, 1, 0.05f));

        var valueProp = property.FindPropertyRelative("m_Value");
        var modProp = property.FindPropertyRelative("m_Mod");

        var valueField = new PropertyField(valueProp);
        valueField.BindProperty(valueProp);
        var modField = new PropertyField(modProp);
        modField.SetEnabled(false);
        modField.BindProperty(modProp);

        container.Add(valueField);
        container.Add(modField);

        return container;
    }
}