
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CanEditMultipleObjects]
[CustomEditor(typeof(ItemBuilder), true)]
public class ItemBuilderEditor : EntityBuilderEditor
{
    private readonly static string itemDataKey = "ItemBuilderFoldout_" + Guid.NewGuid().ToString();
    public override VisualElement CreateInspectorGUI()
    {
        var sObj = serializedObject;

        // find all the properties
        var valueProp = serializedObject.FindProperty("m_Value");
        var itemLevelProp = serializedObject.FindProperty("m_ItemLevel");
        var usagesProp = serializedObject.FindProperty("m_Usages");
        var activatableProp = serializedObject.FindProperty("m_Activatable");
        var onAcquireProp = serializedObject.FindProperty("m_OnAcquire");
        var onLoseProp = serializedObject.FindProperty("m_OnLose");
        var onUseProp = serializedObject.FindProperty("m_OnUse");

        // create the fields
        var statisticsField = ObjectPicker.Field(sObj, "m_Statistics");
        var valueField = ObjectPicker.Field(valueProp, "Value");
        var itemLevelField = ObjectPicker.Field(itemLevelProp);
        var usagesField = ObjectPicker.Field(usagesProp);
        var activatableField = ObjectPicker.Field(activatableProp);
        var onAcquireField = ObjectPicker.Field(onAcquireProp, "OnAcquire");
        var onLoseField = ObjectPicker.Field(onLoseProp, "OnLose");
        var onUseField = ObjectPicker.Field(onUseProp, "OnUse");

        // bind the fields
        valueField.BindProperty(valueProp);
        itemLevelField.BindProperty(itemLevelProp);
        usagesField.BindProperty(usagesProp);
        activatableField.BindProperty(activatableProp);
        onAcquireField.BindProperty(onAcquireProp);
        onLoseField.BindProperty(onLoseProp);
        onUseField.BindProperty(onUseProp);

        // create the container
        var baseContainer = base.CreateInspectorGUI();

        var firstContainer = new Foldout();
        firstContainer.viewDataKey = itemDataKey;
        firstContainer.AddToClassList("base");
        firstContainer.AddToClassList("header");
        var secondToggle = firstContainer.Q<Toggle>();
        secondToggle.AddToClassList("left-padded");
        firstContainer.text = "Item Info";

        var innerContainer = new VisualElement();
        innerContainer.AddToClassList("horizontal-container");

        // add fields to the container
        firstContainer.Add(valueField);
        firstContainer.Add(usagesField);
        innerContainer.Add(itemLevelField);
        innerContainer.Add(activatableField);
        ObjectPicker.AddTo(innerContainer, statisticsField);
        firstContainer.Add(innerContainer);
        firstContainer.Add(onAcquireField);
        firstContainer.Add(onLoseField);
        firstContainer.Add(onUseField);

        baseContainer.Insert(0, firstContainer);

        return baseContainer;
    }
}