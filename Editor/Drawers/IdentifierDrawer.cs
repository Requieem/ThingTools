using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// IngredientDrawerUIE
[CustomPropertyDrawer(typeof(ThingID), true)]
public class IdentifierDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();
        container.AddToClassList("base");

        var IdentifierHeader = new Label("Persistent Identifier");
        IdentifierHeader.AddToClassList("header");

        container.Add(IdentifierHeader);

        var identifierProp = property.FindPropertyRelative("id");
        var identifierField = new TextElement();
        identifierField.BindProperty(identifierProp);

        container.Add(identifierField);

        return container;
    }
}