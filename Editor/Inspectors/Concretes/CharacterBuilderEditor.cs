
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(CharacterBuilder), true)]
public class CharacterBuilderEditor : EntityBuilderEditor
{
    private readonly static string characterDataKey = "CharacterBuilderFoldout_" + Guid.NewGuid().ToString();
    public override VisualElement CreateInspectorGUI()
    {
        // find all the properties
        var sObj = serializedObject;

        var expProp = serializedObject.FindProperty("m_Exp");
        var classField = ObjectPicker.Field(sObj, "m_Class");
        var levelField = ObjectPicker.Field(sObj, "m_Level");
        var factionField = ObjectPicker.Field(sObj, "m_Faction");
        var vitalityField = ObjectPicker.Field(sObj, "m_Vitality");
        var inventoryField = ObjectPicker.Field(sObj, "m_Inventory");
        var equipmentField = ObjectPicker.Field(sObj, "m_Equipment");
        var statisticsField = ObjectPicker.Field(sObj, "m_Statistics");
        var activatorProp = serializedObject.FindProperty("m_Activator");
        var interactorProp = serializedObject.FindProperty("m_Interactor");
        var actioneerProp = serializedObject.FindProperty("m_Actioneer");
        var onLevelUpProp = serializedObject.FindProperty("m_OnLevelUp");

        // create the fields
        var expField = ObjectPicker.Field(expProp, "Starting Experience:");
        var activatorField = ObjectPicker.Field(activatorProp);
        var interactorField = ObjectPicker.Field(interactorProp);
        var actioneerField = ObjectPicker.Field(actioneerProp);
        var onLevelUpField = ObjectPicker.Field(onLevelUpProp, "OnLevelUp");

        // bind the fields
        expField.BindProperty(expProp);
        activatorField.BindProperty(activatorProp);
        interactorField.BindProperty(interactorProp);
        actioneerField.BindProperty(actioneerProp);
        onLevelUpField.BindProperty(onLevelUpProp);

        // create the container
        var baseContainer = base.CreateInspectorGUI();

        var firstContainer = new Foldout();
        firstContainer.viewDataKey = characterDataKey;
        firstContainer.AddToClassList("base");
        firstContainer.AddToClassList("header");
        var secondToggle = firstContainer.Q<Toggle>();
        secondToggle.AddToClassList("left-padded");
        firstContainer.text = "Character Info";

        var innerContainer = new VisualElement();
        innerContainer.AddToClassList("horizontal-container");

        // add fields to the container
        firstContainer.Add(expField);
        innerContainer.Add(activatorField);
        innerContainer.Add(interactorField);
        ObjectPicker.AddTo(innerContainer, classField);
        ObjectPicker.AddTo(innerContainer, levelField);
        ObjectPicker.AddTo(innerContainer, factionField);
        ObjectPicker.AddTo(innerContainer, vitalityField);
        ObjectPicker.AddTo(innerContainer, inventoryField);
        ObjectPicker.AddTo(innerContainer, equipmentField);
        ObjectPicker.AddTo(innerContainer, statisticsField);
        innerContainer.Add(actioneerField);
        firstContainer.Add(innerContainer);
        firstContainer.Add(onLevelUpField);

        baseContainer.Insert(0, firstContainer);

        return baseContainer;
    }
}