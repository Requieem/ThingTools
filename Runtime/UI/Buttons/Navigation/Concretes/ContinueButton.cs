using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : NavButton
{
    [SerializeField] SaveFileTrigger onSelectedFile;
    [SerializeField] SaveFileTrigger onLoadGame;
    [SerializeField] BoolTrigger onSelectedPresent;
    SaveFile selectedFile;

    public override void Enable()
    {
        base.Enable();
        GetComponent<Button>().onClick.AddListener(ContinueGame);
        onSelectedPresent?.AddListener(SetActive);
        onSelectedFile?.AddListener(SelectFile);
    }

    public virtual void Disable()
    {
        GetComponent<Button>().onClick.RemoveListener(ContinueGame);
        onSelectedPresent.RemoveListener(SetActive);
        onSelectedFile?.RemoveListener(SelectFile);
    }

    private void OnDisable()
    {
        Disable();
    }
    protected void SelectFile(SaveFile file)
    {
        selectedFile = file;
    }

    private void ContinueGame()
    {
        if (onLoadGame != null)
        {
            onLoadGame.Invoke(selectedFile);
        }
    }
}