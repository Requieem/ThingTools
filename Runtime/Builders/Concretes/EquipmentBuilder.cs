using UnityEngine;

/// <summary>
/// Builder for creating instances of type <see cref="Equipment"/>, which are containers for Items.
/// </summary>
[CreateAssetMenu(fileName = "EquipmentBuilder", menuName = "ShireSoft/Containers/Equipment")]
public class EquipmentBuilder : ThingsContainerBuilder<EquipmentBuilder, Equipment, SlotType, ItemBuilder, Item>
{
    /// <summary>
    /// Gets a copy of the Equipment.
    /// </summary>
    /// <returns>A copy of the Equipment.</returns>
    public override Equipment GetCopy()
    {
        return new Equipment(this);
    }

    /// <summary>
    /// Gets an instance of the Equipment. If no instance is currently built, a new one is built and stored.
    /// </summary>
    /// <returns>An instance of the Equipment.</returns>
    public override Equipment GetInstance()
    {
        m_Built ??= new Equipment(this);
        return m_Built;
    }
}
