using UnityEngine;
using TMPro;

public class SaveInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField; // Reference to the TextMeshPro Input Field
    private const string INPUT_KEY = "username"; // Key to save and retrieve the input from PlayerPrefs

    private void Start()
    {
        // Load the saved input from PlayerPrefs and set it to the input field
        if (PlayerPrefs.HasKey(INPUT_KEY))
        {
            inputField.text = PlayerPrefs.GetString(INPUT_KEY);
        }
    }

    public void SaveInputToPlayerPrefs()
    {
        // Save the current input to PlayerPrefs
        PlayerPrefs.SetString(INPUT_KEY, inputField.text);
        PlayerPrefs.Save(); // Save changes to PlayerPrefs
    }

    // Call this method when the input field content changes
    public void OnInputValueChanged()
    {
        SaveInputToPlayerPrefs();
    }
}
