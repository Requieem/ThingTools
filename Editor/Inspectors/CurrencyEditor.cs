using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(Currency), true)]
[CanEditMultipleObjects]
public class CurrencyEditor : ShireEditor
{
    private readonly static string viewDataKey = "CurrencyFoldout_" + Guid.NewGuid().ToString();
    public override VisualElement CreateInspectorGUI()
    {
        m_StyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("./Style/ShireSoft.uss");

        var container = new VisualElement();
        container.styleSheets.Add(m_StyleSheet);

        container.Add(CurrencyContainer());
        container.Add(base.CreateInspectorGUI());

        return container;
    }

    public VisualElement CurrencyContainer()
    {
        var container = new VisualElement();
        container.AddToClassList("base");

        // toggle that determines if the level is the first level
        var label = new Label("Currency Info");
        label.AddToClassList("header");

        var rateField = new Slider(0f, 10f);
        rateField.showInputField = true;
        rateField.BindProperty(serializedObject.FindProperty("fixedRate"));

        var listContainer = new Foldout();
        container.viewDataKey = viewDataKey + serializedObject.targetObject.name;
        listContainer.AddToClassList("base");
        listContainer.AddToClassList("header");
        var toggle = listContainer.Q<Toggle>();
        toggle.AddToClassList("left-padded");
        listContainer.text = "Money Pieces";

        var moneyPiecesProp = serializedObject.FindProperty("m_MoneyPieces");
        var moneyPiecesField = new ListView();
        moneyPiecesField.AddToClassList("customListView");
        moneyPiecesField.makeItem = () => new PropertyField();
        moneyPiecesField.bindItem = (element, index) =>
        {
            var item = (PropertyField)element;
            item.BindProperty(moneyPiecesProp.GetArrayElementAtIndex(index));
        };

        moneyPiecesField.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        moneyPiecesField.style.flexGrow = 1;

        // get a list from the serializedProperty enumerator
        var list = new List<object>();
        var enumerator = moneyPiecesProp.GetEnumerator();
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
        }

        moneyPiecesField.itemsSource = list;
        moneyPiecesField.showAddRemoveFooter = true;

        moneyPiecesField.itemsAdded += (items) =>
        {
            foreach (var item in items)
            {
                moneyPiecesProp.arraySize++;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                moneyPiecesField.Rebuild();
            }
        };

        moneyPiecesField.itemsRemoved += (items) =>
        {
            foreach (var item in items)
            {
                moneyPiecesProp.DeleteArrayElementAtIndex(item);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        };

        var content = moneyPiecesField.Query(className: "unity-scroll-view").First();
        content.AddToClassList("list-content");

        listContainer.Add(moneyPiecesField);
        container.Add(label);
        container.Add(rateField);
        container.Add(listContainer);

        return container;
    }
}
