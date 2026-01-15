using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneManageLoader : MonoBehaviour
{
    static public bool gameIsPaused;
    private HighScoreUIManager hSM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hSM = FindAnyObjectByType<HighScoreUIManager>();
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
    }

    // Opens "TestScene"
    public void OpenLevel1()
    {
        //SceneManager.LoadScene(0);


        hSM.SubmitScore(2);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
