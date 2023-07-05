using UnityEngine;

/// <summary>
/// Builder for creating instances of type <see cref="Inventory"/>, which are containers for Items.
/// </summary>
[CreateAssetMenu(fileName = "InventoryBuilder", menuName = "ShireSoft/Containers/Inventory")]
public class InventoryBuilder : ObjectContainerBuilder<InventoryBuilder, Inventory, ItemBuilder, Item>
{
    /// <summary>
    /// The builder for the inventory's wallet.
    /// </summary>
    [SerializeField] protected NumismaticBuilder m_Wallet;

    /// <summary>
    /// Gets an instance of the inventory's wallet.
    /// </summary>
    public Numismatic Wallet
    {
        get { return m_Wallet.GetInstance(); }
    }

    /// <summary>
    /// Gets a copy of the Inventory.
    /// </summary>
    /// <returns>A copy of the Inventory.</returns>
    public override Inventory GetCopy()
    {
        return new Inventory(this);
    }

    /// <summary>
    /// Gets an instance of the Inventory. If no instance is currently built, a new one is built and stored.
    /// </summary>
    /// <returns>An instance of the Inventory.</returns>
    public override Inventory GetInstance()
    {
        m_Built ??= new Inventory(this);
        return m_Built;
    }
}
