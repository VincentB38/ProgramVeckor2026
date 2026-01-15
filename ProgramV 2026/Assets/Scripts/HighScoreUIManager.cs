using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PlayerScoreEntry
{
    public string playerName;
    public int score;
}

[System.Serializable]
public class Leaderboard
{
    public List<PlayerScoreEntry> entries = new List<PlayerScoreEntry>();
}

public class HighScoreUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI leaderboardText;   // Display leaderboard
    public TMP_InputField nameInputField;     // Player types their name

    private const string LeaderboardKey = "LeaderboardTop10";
    private const int MaxScores = 10;
    private Leaderboard leaderboard;

    [HideInInspector] public int currentPlayerScore = 0; // Set this before submitting

    void Start()
    {
        LoadLeaderboard();
        UpdateUI();

        // Optional: placeholder text
        if (nameInputField != null)
            nameInputField.placeholder.GetComponent<TMP_Text>().text = "Enter Name...";
    }
    public void SubmitScore(int PlayerScore)
    {
        if (nameInputField == null) return;

        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
            playerName = "TestPlayer"; // Defualt name if no user chosen

        // Add the new entry
        leaderboard.entries.Add(new PlayerScoreEntry
        {
            playerName = playerName,
            score = PlayerScore
        });

        // Sort descending
        leaderboard.entries.Sort((a, b) => b.score.CompareTo(a.score));

        // Keep top 10
        if (leaderboard.entries.Count > MaxScores)
            leaderboard.entries.RemoveRange(MaxScores, leaderboard.entries.Count - MaxScores);

        SaveLeaderboard();
        UpdateUI();

        // Clear input field for next use
        nameInputField.text = "";
    }
    private void UpdateUI() // Update the ui
    {
        if (leaderboardText == null) return;

        leaderboardText.text = "Top 10 High Scores:\n";

        for (int i = 0; i < leaderboard.entries.Count; i++)
        {
            PlayerScoreEntry entry = leaderboard.entries[i];
            leaderboardText.text += $"{i + 1}. {entry.playerName} - {entry.score}\n";
        }
    }

    private void SaveLeaderboard() // Save the stats to the playerpref
    {
        string json = JsonUtility.ToJson(leaderboard);
        PlayerPrefs.SetString(LeaderboardKey, json);
        PlayerPrefs.Save();
    }
    private void LoadLeaderboard() // Load the ldb 
    {
        if (PlayerPrefs.HasKey(LeaderboardKey))
        {
            string json = PlayerPrefs.GetString(LeaderboardKey);
            leaderboard = JsonUtility.FromJson<Leaderboard>(json);
        }
        else
        {
            leaderboard = new Leaderboard();
        }
    }
    public void ResetLeaderboard() // Clear Ldb
    {
        leaderboard = new Leaderboard();
        PlayerPrefs.DeleteKey(LeaderboardKey);
        UpdateUI();
    }
}
