using UnityEngine;

public class LoadButton : ContinueButton
{
    [SerializeField] SaveFileTrigger onSelectedSave;

    public override void Enable()
    {
        base.Enable();
        onSelectedSave?.AddListener(SelectFile);
    }

    public override void Disable()
    {
        base.Disable();
        onSelectedSave?.RemoveListener(SelectFile);
    }
}
