using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public int Health = 3;
    int Score = 0;

    public Transform HeartContainer;
    public GameObject HeartPreFab;
    public TextMeshProUGUI PlayerScore;

    private List<RawImage> Hearts = new List<RawImage>();
    void Start()
    {
        CreateHearts(); // Creates the hearts based of hp
    }

    void Update()
    {
        PlayerScore.text = "Score: " + Score;
    }

    public void ChangeHealth(int amount) // Update hp
    {
        Health += amount;

        CalculateHearts(); // Recalculate the visual hearts
    }

    public void UpdateScore(int amount) // Update score
    {
        Score += amount;
    }

    void CreateHearts()
    {
        for (int i = 0; i < Health; i++) // Skappar hjärtan baserat på health
        {
            GameObject heartObj = Instantiate(HeartPreFab, HeartContainer);
            heartObj.name = "Heart" + i;

            RectTransform rt = heartObj.GetComponent<RectTransform>(); // Placera Hjärtan
            rt.anchoredPosition = new Vector2(-1158 + (178 *i), 569); // Start Position, ökar med 178 på x led varje ny hjärta

            RawImage heartImage = heartObj.GetComponent<RawImage>(); // lägga til i listan

            if (heartImage != null)
               Hearts.Add(heartImage);
        }
    }

    void CalculateHearts() // if they should be active or not
    {
        for (int i = 0; i < Hearts.Count; i++) // loop through
        {
          Hearts[i].gameObject.SetActive(i < Health); // checks
        }
    }
}