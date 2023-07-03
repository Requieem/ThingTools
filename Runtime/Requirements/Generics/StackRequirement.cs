using UnityEngine;

[CreateAssetMenu(fileName = "StackRequirement", menuName = "ShireSoft/Requirements/StackRequirement", order = 0)]
public class StackRequirement<D, S> : ReferenceRequirement<D, S> where D : IObjectSatisfier<S>
{
    [SerializeField] float requiredQuantity;
    [SerializeField] float currentQuantity;

    /// <summary>
    /// Invokes the onSatisfied event if the currentQuantity is >= requiredQuantity, while setting the isSatisfied boolean to true.
    /// </summary>
    /// <remarks>
    /// This method is called when the doSatisfy event is invoked on the above condition. It also sets the isSatisfied boolean to true.
    /// </remarks>
    public override void Satisfy()
    {
        if (++currentQuantity >= requiredQuantity)
        {
            base.Satisfy();
        }
    }

    /// <summary>
    /// Invokes the onUnsatisfied event if the currentQuantity is < requiredQuantity, while setting the isSatisfied boolean to false.
    /// </summary>
    /// <remarks>
    /// This method is called when the unSatisfy event is invoked on the above condition. It also sets the isSatisfied boolean to false.
    /// </remarks>
    public override void Unsatisfy()
    {
        if (--currentQuantity < requiredQuantity)
        {
            base.Unsatisfy();
        }
    }

    /// <summary>
    /// Registers the doSatisfy and unSatisfy events to the collection.
    /// </summary>
    /// <remarks>
    /// This method is called when the object is enabled. It also sets the currentQuantity to the quantity of the item in the collection.
    /// If there is enough of the item in the collection, it will invoke the onSatisfied event.
    /// </remarks>
    public override void Enable()
    {
        base.Enable();
        if (m_Item != null)
        {
            currentQuantity = m_Satisfier?.Watch(m_Item, m_DoSatisfy, m_UnSatisfy) ?? 0;
            if (currentQuantity >= requiredQuantity)
            {
                base.Satisfy();
            }
            else
            {
                base.Unsatisfy();
            }
        }
    }
}