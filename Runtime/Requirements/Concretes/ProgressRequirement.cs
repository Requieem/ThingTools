using UnityEngine;

[CreateAssetMenu(fileName = "ProgressRequirement", menuName = "ShireSoft/Requirements/ProgressRequirement", order = 0)]
public class ProgressRequirement : ReferenceRequirement<Progress, float>
{
    #region Instance Methods:

    public override void Enable()
    {
        base.Enable();
        if (m_Satisfier?.Watch(m_Item, m_DoSatisfy, m_UnSatisfy) >= m_Item)
        {
            Satisfy();
        }
        else
        {
            Unsatisfy();
        }
    }

    public override void Disable()
    {
        base.Disable();
        m_Satisfier?.Unwatch(m_Item);
    }

    #endregion
}