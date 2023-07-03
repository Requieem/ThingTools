using System;
using UnityEditor;
using UnityEngine.UIElements;

// IngredientDrawerUIE
[CustomPropertyDrawer(typeof(ThingsContainer<,>), true)]
public class ThingsContainerDrawer : PropertyDrawer
{
    private readonly static string viewDataKey = "ThingsContainerFoldout_" + Guid.NewGuid().ToString();

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        return Draw(property, true);
    }

    public static VisualElement Draw(SerializedProperty property, bool foldout = true)
    {
        // Create property container element.
        var container = foldout ? new Foldout() : new VisualElement();
        if (foldout)
        {
            container.viewDataKey = viewDataKey + property.serializedObject.targetObject.name;
            var toggle = container.Q<Toggle>();
            toggle.AddToClassList("left-padded");
            (container as Foldout).text = property.displayName;
        }
        else
        {
            container.AddToClassList("left-margin");
        }

        container.AddToClassList("base");
        container.AddToClassList("header");

        var dictionaryField = ListDrawer.Draw(property.serializedObject, property, "m_Entries");

        container.Add(dictionaryField);


        return container;
    }
}