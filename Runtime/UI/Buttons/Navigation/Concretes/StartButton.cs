using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] SaveFileTrigger onCreateCharacter;
    [SerializeField] DifficultyTrigger onDifficultyChange;
    [SerializeField] ClassTrigger onClassChange;
    [SerializeField] StringTrigger onNameChange;
    [SerializeField] CharacterBuilder characterBuilder;
    [SerializeField] ErrorMessageTrigger onInvalidUsername;
    [SerializeField] ErrorMessageTrigger onSaveError;

    [SerializeField] TextMeshProUGUI errorViewer;

    string ErrorMessage { get { return "New Game ERROR: "; } }

    bool error = false;
    string chosenName = null;
    Difficulty chosenDifficulty = null;
    Class chosenClass = null;

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(StartGame);
        onDifficultyChange?.AddListener(SetDifficulty);
        onClassChange?.AddListener(SetClass);
        onNameChange?.AddListener(SetName);
        onInvalidUsername?.AddListener(SetError);
        onSaveError?.AddListener(SetError);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(StartGame);
        onDifficultyChange?.RemoveListener(SetDifficulty);
        onClassChange?.RemoveListener(SetClass);
        onNameChange?.RemoveListener(SetName);
        onInvalidUsername?.RemoveListener(SetError);
        onSaveError?.RemoveListener(SetError);
    }

    private void StartGame()
    {
        error = false;

        if (characterBuilder == null) SetError("Ask the dev!");
        if (chosenName == null) SetError("No name!");
        if (chosenDifficulty == null) SetError("No difficulty!");
        if (chosenClass == null) SetError("Noa class!");
        if (error) return;

        var character = new Character(characterBuilder, chosenName, chosenClass);
        var saveFile = new SaveFile(character, chosenDifficulty);

        if (onCreateCharacter != null)
        {
            onCreateCharacter.Invoke(saveFile);
        }
    }

    private void SetError(string errorMessage)
    {
        if (errorViewer != null)
        {
            errorViewer.text = ErrorMessage + errorMessage;
            errorViewer.color = Color.red;
        }

        error = true;
    }

    private void SetName(string name)
    {
        chosenName = name;
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        chosenDifficulty = difficulty;
    }

    private void SetClass(Class _class)
    {
        chosenClass = _class;
    }
}
