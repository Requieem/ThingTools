using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Displayable), true)]
public class DisplayableDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();
        container.AddToClassList("base");

        var displayableHeader = new Label("Displayable");
        displayableHeader.AddToClassList("header");

        container.Add(displayableHeader);

        var spriteAndColorContainer = new VisualElement();
        spriteAndColorContainer.AddToClassList("spriteBySideBox");
        var spriteContainer = new VisualElement();
        spriteContainer.AddToClassList("sideSpriteBox");
        spriteContainer.style.width = spriteContainer.style.height;
        var spriteImage = new VisualElement();
        spriteImage.AddToClassList("sideSprite");
        var doublePropContainer = new VisualElement();

        var spriteProp = property.FindPropertyRelative("sprite");
        var colorProp = property.FindPropertyRelative("color");

        if (spriteProp.objectReferenceValue != null)
        {
            spriteImage.style.backgroundImage = new StyleBackground(spriteProp.objectReferenceValue as Sprite);
            spriteContainer.RemoveFromClassList("hidden");
        }
        else
        {
            spriteContainer.AddToClassList("hidden");
        }

        spriteImage.style.backgroundImage = new StyleBackground(spriteProp.objectReferenceValue as Sprite);

        var spriteField = new ObjectField();
        spriteField.objectType = typeof(Sprite);
        spriteField.label = "Sprite";
        spriteField.BindProperty(spriteProp);
        spriteField.AddToClassList("spriteProp");

        spriteField.RegisterValueChangedCallback(value =>
        {
            if(value.newValue != null)
            {
                spriteImage.style.backgroundImage = new StyleBackground(value.newValue as Sprite);
                spriteContainer.RemoveFromClassList("hidden");
            }
            else
            {
                spriteContainer.AddToClassList("hidden");
            }
        });

        var colorField = new ColorField();
        colorField.BindProperty(colorProp);
        colorField.label = "Color";
        colorField.AddToClassList("colorProp");

        var descriptionProp = property.FindPropertyRelative("description");
        var descriptionField = new PropertyField(descriptionProp);
        descriptionField.style.marginTop = new StyleLength(10f);

        doublePropContainer.Add(spriteField);
        doublePropContainer.Add(colorField);
        doublePropContainer.Add(descriptionField);

        spriteContainer.Add(spriteImage);
        spriteAndColorContainer.Add(spriteContainer);
        spriteAndColorContainer.Add(doublePropContainer);

        container.Add(spriteAndColorContainer);

        return container;
    }
}