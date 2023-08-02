using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(Money), true)]
[CanEditMultipleObjects]
public class MoneyEditor : ShireEditor
{
    public override VisualElement CreateInspectorGUI()
    {
        m_StyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(PathConstants.StylePath);

        var container = new VisualElement();
        container.styleSheets.Add(m_StyleSheet);

        container.Add(MoneyContainer());
        container.Add(base.CreateInspectorGUI());

        return container;
    }

    public VisualElement MoneyContainer()
    {
        var container = new VisualElement();
        container.AddToClassList("base");

        // toggle that determines if the level is the first level
        var label = new Label("Rate");
        label.AddToClassList("header");
        var rateField = new Slider(0f, 100f);
        rateField.showInputField = true;
        rateField.BindProperty(serializedObject.FindProperty("rate"));

        var floatField = rateField.Q<TextField>();
        floatField.RegisterCallback<FocusOutEvent>((evt) =>
        {
            if (float.TryParse(floatField.text, out float res))
            {
                serializedObject.FindProperty("rate").floatValue = res;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        });

        container.Add(label);
        container.Add(rateField);

        return container;
    }
}