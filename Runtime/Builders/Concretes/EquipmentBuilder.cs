using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentBuilder", menuName = "ShireSoft/Containers/Equipment")]
public class EquipmentBuilder : ThingsContainerBuilder<EquipmentBuilder, Equipment, SlotType, ItemBuilder, Item>
{
    public override Equipment GetCopy()
    {
        return new Equipment(this);
    }

    public override Equipment GetInstance()
    {
        m_Built ??= new Equipment(this);
        return m_Built;
    }
}