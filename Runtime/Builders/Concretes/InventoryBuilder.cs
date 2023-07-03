using UnityEngine;

[CreateAssetMenu(fileName = "InventoryBuilder", menuName = "ShireSoft/Containers/Inventory")]
public class InventoryBuilder : ObjectContainerBuilder<InventoryBuilder, Inventory, ItemBuilder, Item>
{
    [SerializeField] protected NumismaticBuilder m_Wallet;
    public Numismatic Wallet { get { return m_Wallet.GetInstance(); } }

    public override Inventory GetCopy()
    {
        return new Inventory(this);
    }

    public override Inventory GetInstance()
    {
        m_Built ??= new Inventory(this);
        return m_Built;
    }
}