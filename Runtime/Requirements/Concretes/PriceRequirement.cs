using UnityEngine;

[CreateAssetMenu(fileName = "PriceRequirement", menuName = "ShireSoft/Requirements/PriceRequirement", order = 0)]
public class PriceRequirement : Requirement {

    #region Instance Fields:
    [SerializeField] protected Numismatic wallet;
    [SerializeField] protected Numismatic targetPrice;
    #endregion

    #region Instance Properties:
    public Numismatic Wallet { get { return wallet; } }
    public Numismatic TargetPrice { get { return targetPrice; } }
    #endregion

    #region Initializers:
    public virtual void Initialize(bool isSatisfied, DataTrigger<Requirement> onSatisfied, DataTrigger<Requirement> onUnsatisfied, Numismatic wallet, Numismatic targetPrice)
    {
        base.Initialize(isSatisfied, onSatisfied, onUnsatisfied);
        this.wallet = wallet;
        this.targetPrice = targetPrice;
    }
    #endregion

    #region Instance Methods:
    
    public override void Enable()
    {
        base.Enable();
        if (targetPrice != null)
            if (wallet?.Watch(targetPrice, m_DoSatisfy, m_UnSatisfy) == 1) {
                Satisfy();
            } else {
                Unsatisfy();
            }
    }

    public override void Disable()
    {
        base.Disable();
        if (targetPrice != null)
            wallet?.Unwatch(targetPrice);
    }

    #endregion
}