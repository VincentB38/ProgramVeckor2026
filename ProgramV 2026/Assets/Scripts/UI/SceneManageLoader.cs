using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class SceneManageLoader : MonoBehaviour
{
    static public bool gameIsPaused;
    private HighScoreUIManager hSM;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI SelectedText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hSM = FindAnyObjectByType<HighScoreUIManager>();

        if (nameInputField != null)
            nameInputField.placeholder.GetComponent<TMP_Text>().text = "Enter Name...";
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.isPressed)
        {
            if (gameIsPaused)
            {
                Time.timeScale = 0f;
                // Add pause menu
            }

            if (!gameIsPaused)
            {
                // Hide pause menu
                Time.timeScale = 1f;
            }
        }

        if (string.IsNullOrEmpty(nameInputField.text))
        {
            SelectedText.text = nameInputField.text;
        } else
        {
            SelectedText.text = "No Name Selected";
        }
    }

    // Opens "TestScene"
    public void OpenLevel1()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenMainMenu()
    {
        int PlayerScore = PlayerPrefs.GetInt("CurrentScore");
        string PlayerName = nameInputField.text;

        if (string.IsNullOrEmpty(PlayerName))
            PlayerName = "Player";

        hSM.SubmitScore(PlayerScore, PlayerName);

        nameInputField.text = "";
        PlayerPrefs.SetInt("CurrentScore", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(1);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
