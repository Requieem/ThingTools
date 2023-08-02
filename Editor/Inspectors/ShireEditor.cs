using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CanEditMultipleObjects]
[CustomEditor(typeof(Shire<>), true)]
public class ShireEditor : Editor
{
    protected StyleSheet m_StyleSheet;
    public override VisualElement CreateInspectorGUI()
    {
        m_StyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(PathConstants.StylePath);

        var container = new VisualElement();
        container.styleSheets.Add(m_StyleSheet);

        var displayableProp = serializedObject.FindProperty("m_Displayable");
        var displayableField = new PropertyField(displayableProp);
        displayableField.BindProperty(displayableProp);

        var identifierProp = serializedObject.FindProperty("m_Identifier");
        var identifierField = new PropertyField(identifierProp);
        identifierField.BindProperty(identifierProp);

        container.Add(UsageContainer());
        container.Add(displayableField);
        container.Add(identifierField);

        return container;
    }

    public VisualElement UsageContainer()
    {
        var usedProp = serializedObject.FindProperty("m_Used");
        var orderProp = serializedObject.FindProperty("m_Order");
        var itemsProp = serializedObject.FindProperty("m_Lister").FindPropertyRelative("m_Siblings");

        var usageContainer = new VisualElement();

        var usageHeader = new Label("Usage");
        usageHeader.AddToClassList("header");

        var usageToggleContainer = new VisualElement();

        var usedToggle = new Toggle("Used");
        usedToggle.BindProperty(usedProp);

        var orderSlider = new SliderInt("Order", 0, itemsProp.arraySize - 1);
        if (orderProp.intValue >= itemsProp.arraySize)
        {
            orderProp.intValue = itemsProp.arraySize - 1;
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
        orderSlider.BindProperty(orderProp);
        orderSlider.showInputField = true;

        orderSlider.RegisterValueChangedCallback(value =>
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        });

        if (!usedProp.boolValue)
        {
            orderSlider.AddToClassList("hidden");
        }

        usedToggle.RegisterValueChangedCallback(value =>
        {
            if (value.newValue)
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                orderSlider.RemoveFromClassList("hidden");
                orderSlider.highValue = itemsProp.arraySize - 1;
            }
            else
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                orderSlider.AddToClassList("hidden");
            }
        });

        usageContainer.Add(usageHeader);
        usageToggleContainer.Add(usedToggle);
        usageToggleContainer.Add(orderSlider);
        usageContainer.Add(usageToggleContainer);
        usageContainer.AddToClassList("base");

        return usageContainer;
    }
}