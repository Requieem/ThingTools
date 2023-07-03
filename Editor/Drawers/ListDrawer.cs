using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class ListDrawer
{
    private readonly static string viewDataKey = "ContextContainerList_" + Guid.NewGuid().ToString();

    public static ListView Draw(SerializedObject obj, string propertyPath)
    {
        var prop = obj.FindProperty(propertyPath);
        return DoDraw(obj, prop);
    }

    public static ListView Draw(SerializedObject obj, SerializedProperty property, string propertyPath)
    {
        var prop = property.FindPropertyRelative(propertyPath);
        return DoDraw(obj, prop);
    }

    public static ListView DoDraw(SerializedObject obj, SerializedProperty prop)
    {
        var field = new ListView();
        field.viewDataKey = viewDataKey + obj.targetObject.name;
        field.AddToClassList("customListView");
        /*
         * Reordering does not work, need more research.
         * dictionaryField.reorderable = true;
         * dictionaryField.reorderMode = ListViewReorderMode.Animated;
        */
        field.makeItem = () => new PropertyField();
        field.bindItem = (element, index) =>
        {
            var item = (PropertyField)element;
            item.BindProperty(prop.GetArrayElementAtIndex(index));
        };

        field.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        field.style.flexGrow = 1;

        // get a list from the serializedProperty enumerator
        var list = new List<object>();
        var enumerator = prop.GetEnumerator();
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
        }

        field.itemIndexChanged += (index, newIndex) =>
        {
            // swap
            field.itemsSource = list;
            obj.ApplyModifiedProperties();
            obj.Update();
            field.Rebuild();
        };

        field.itemsSource = list;
        field.showAddRemoveFooter = true;

        field.itemsAdded += (items) =>
        {
            foreach (var item in items)
            {
                prop.arraySize++;
                obj.ApplyModifiedProperties();
                obj.Update();
                field.Rebuild();
            }
        };

        field.itemsRemoved += (items) =>
        {
            foreach (var item in items)
            {
                prop.DeleteArrayElementAtIndex(item);
                obj.ApplyModifiedProperties();
                obj.Update();
            }
        };

        var content = field.Query(className: "unity-scroll-view").First();
        content.AddToClassList("list-content");

        return field;
    }
}
