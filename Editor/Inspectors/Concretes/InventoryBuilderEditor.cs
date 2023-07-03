using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryBuilder), true)]
public class InventoryBuilderEditor : ObjectContainerBuilderEditor
{
    public override VisualElement CreateInspectorGUI()
    {
        var container = base.CreateInspectorGUI();
        var walletField = ObjectPicker.Field(serializedObject, "m_Wallet");
        var label = new Label("Wallet");
        label.AddToClassList("header");

        container.Add(label);
        ObjectPicker.AddTo(container, walletField);

        return container;
    }
}