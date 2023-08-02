using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(Class), true)]
[CanEditMultipleObjects]
public class ClassEditor : ShireEditor
{
    public override VisualElement CreateInspectorGUI()
    {
        m_StyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(PathConstants.StylePath);

        var container = new VisualElement();
        container.styleSheets.Add(m_StyleSheet);

        var statisticsProp = serializedObject.FindProperty("statistics");
        var statisticsField = new PropertyField(statisticsProp);
        statisticsField.BindProperty(statisticsProp);

        container.Add(statisticsField);
        container.Add(base.CreateInspectorGUI());

        return container;
    }
}