using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component
    private int score = 0;
    [SerializeField] private popUpPlusFive popup;

    private const string SERVER_URL = "https://3duverse.com/score.php"; // Replace with your server's URL
    private string USERNAME = "YourUsername"; // Replace with the user's username

    private void Start()
    {
        try
        {
            USERNAME = PlayerPrefs.GetString("username", "DefaultUsername");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error retrieving username from PlayerPrefs: " + e.Message);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        // Check if the current score is higher than the saved high score
        try
        {
            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score);
                if (!string.IsNullOrEmpty(USERNAME))
                {
                    UpdateServerWithHighScore(USERNAME, score);
                }
                else
                {
                    Debug.LogWarning("Username is empty or invalid.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error updating high score in PlayerPrefs: " + e.Message);
        }
    }

    private void UpdateScoreText()
    {
        try
        {
            popup.Show();
            Debug.Log("Show Was Called");
            scoreText.text = "Score: " + score;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error updating score text: " + e.Message);
        }
    }

    private void UpdateServerWithHighScore(string username, int highScore)
    {
        StartCoroutine(UpdateHighScoreCoroutine(username, highScore));
    }

    private IEnumerator UpdateHighScoreCoroutine(string username, int highScore)
    {
        // Create a form with the username and high score
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("highScore", highScore.ToString());

        // Send the POST request
        using (UnityWebRequest www = UnityWebRequest.Post(SERVER_URL, form))
        {
            www.timeout = 10; // Set a timeout (in seconds) for the request

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error updating high score: " + www.error);
            }
            else
            {
                Debug.Log("High score updated successfully!");
            }
        }
    }
}
