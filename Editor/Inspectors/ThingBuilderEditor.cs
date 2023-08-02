using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(ThingBuilder<,>), true)]
public class ThingBuilderEditor : Editor
{
    StyleSheet styleSheet;
    public override VisualElement CreateInspectorGUI()
    {
        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("./Style/ShireSoft.uss");

        var container = new VisualElement();
        container.styleSheets.Add(styleSheet);

        var thingProp = serializedObject.FindProperty("m_Built");
        var thingField = new PropertyField(thingProp);
        thingField.BindProperty(thingProp);
        container.Add(thingField);

        return container;
    }
}