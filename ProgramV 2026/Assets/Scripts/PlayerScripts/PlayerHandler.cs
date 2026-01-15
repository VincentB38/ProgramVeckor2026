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

        CreateHearts(); // Recalculate the visual hearts
    }

    public void UpdateScore(int amount) // Update score
    {
        Score += amount;
    }

    void CreateHearts()
    {

        for (int i = HeartContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(HeartContainer.GetChild(i).gameObject);
        }

        Hearts.Clear(); // Clear list

        for (int i = 0; i < Health; i++) // Creates hearts based of health
        {
            GameObject heartObj = Instantiate(HeartPreFab, HeartContainer);
            heartObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            heartObj.name = "Heart" + i;

            RectTransform rt = heartObj.GetComponent<RectTransform>(); // Place the hearts
            rt.anchoredPosition = new Vector2(-365 + (43 *i), 180); // Start Position, increase with 40 on x axis for every new heart

            RawImage heartImage = heartObj.GetComponent<RawImage>(); // add into the list

            if (heartImage != null)
               Hearts.Add(heartImage);
        }
    }
}