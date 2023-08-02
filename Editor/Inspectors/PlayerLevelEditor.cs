using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(Step<>), true)]
[CanEditMultipleObjects]
public class StepEditor : ShireEditor
{
    public override VisualElement CreateInspectorGUI()
    {
        m_StyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("./Style/ShireSoft.uss");

        var container = new VisualElement();
        container.styleSheets.Add(m_StyleSheet);

        container.Add(LevelContainer());
        container.Add(base.CreateInspectorGUI());

        return container;
    }

    public VisualElement LevelContainer()
    {
        var container = new VisualElement();
        container.AddToClassList("base");

        // toggle that determines if the level is the first level
        var isMinLevelProp = serializedObject.FindProperty("m_IsMinLevel");
        var isMinLevelField = new Toggle();
        isMinLevelField.BindProperty(isMinLevelProp);
        isMinLevelField.label = "Min Level: ";


        // toggle that determines if the level is the last level
        var isMaxLevelProp = serializedObject.FindProperty("m_IsMaxLevel");
        var isMaxLevelField = new Toggle();
        isMaxLevelField.BindProperty(isMaxLevelProp);
        isMaxLevelField.label = "Max Level: ";

        var expToThisLevelProp = serializedObject.FindProperty("m_ExpToThisLevel");
        var expToThisLevelField = new PropertyField(expToThisLevelProp);
        expToThisLevelField.BindProperty(expToThisLevelProp);
        expToThisLevelField.label = "Needed Exp: ";
        expToThisLevelField.SetEnabled(false);

        var expToNextLevelProp = serializedObject.FindProperty("m_ExpToNextLevel");
        var expToNextLevelField = new PropertyField(expToNextLevelProp);
        expToNextLevelField.BindProperty(expToNextLevelProp);
        expToNextLevelField.label = "Exp to Next: ";

        var levelPointsProp = serializedObject.FindProperty("m_LevelPoints");
        var levelPointsField = new PropertyField(levelPointsProp);
        levelPointsField.BindProperty(levelPointsProp);
        levelPointsField.label = "Gained Points: ";

        var previousLevelProp = serializedObject.FindProperty("m_PreviousLevel");
        var previousLevelField = new PropertyField(previousLevelProp);
        previousLevelField.BindProperty(previousLevelProp);
        previousLevelField.SetEnabled(false);

        var nextLevelProp = serializedObject.FindProperty("m_NextLevel");
        var nextLevelField = new PropertyField(nextLevelProp);
        nextLevelField.BindProperty(nextLevelProp);
        nextLevelField.SetEnabled(false);

        isMinLevelField.RegisterValueChangedCallback((evt) =>
        {
            if (evt.newValue)
            {
                previousLevelField.AddToClassList("hidden");
            }
            else
            {
                previousLevelField.RemoveFromClassList("hidden");
            }
        });

        isMaxLevelField.RegisterValueChangedCallback((evt) =>
        {
            if (evt.newValue)
            {
                nextLevelField.AddToClassList("hidden");
            }
            else
            {
                nextLevelField.RemoveFromClassList("hidden");
            }
        });

        container.Add(previousLevelField);
        container.Add(nextLevelField);
        container.Add(isMinLevelField);
        container.Add(isMaxLevelField);
        container.Add(expToThisLevelField);
        container.Add(expToNextLevelField);
        container.Add(levelPointsField);

        return container;
    }
}