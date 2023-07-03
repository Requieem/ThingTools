using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterBuilder", menuName = "ShireSoft/Character")]
public class CharacterBuilder : EntityBuilder<CharacterBuilder, Character>
{
    [SerializeField] protected float m_Exp;
    [SerializeField] protected InventoryBuilder m_Inventory;
    [SerializeField] protected EquipmentBuilder m_Equipment;
    [SerializeField] protected ActivatorBuilder m_Activator;
    [SerializeField] protected InteractorBuilder m_Interactor;
    [SerializeField] protected ActioneerBuilder m_Actioneer;
    [SerializeField] UnityEvent m_OnLevelUp;

    // public get-set properties for all of the above, not sure if these are needed so they might be removed later
    // for now, they are here to allow for easy access to the data while ensuring no direct access to field modifications where set is private

    public float StartingExp
    { get { return m_Exp; } set { m_Exp = value; } }
    public Inventory Inventory
    { get { return GetSafeCopy<Inventory, InventoryBuilder>(m_Inventory); } }
    public Equipment Equipment
    { get { return GetSafeCopy<Equipment, EquipmentBuilder>(m_Equipment); } }
    public Activator Activator
    { get { return GetSafeCopy<Activator, ActivatorBuilder>(m_Activator); } }
    public Interactor Interactor
    { get { return GetSafeCopy<Interactor, InteractorBuilder>(m_Interactor); } }
    public Actioneer Actioneer
    { get { return GetSafeCopy<Actioneer, ActioneerBuilder>(m_Actioneer); } }
    public UnityEvent OnLevelUp
    { get { return m_OnLevelUp; } }

    public override Character GetCopy()
    {
        return new Character(this);
    }
}
