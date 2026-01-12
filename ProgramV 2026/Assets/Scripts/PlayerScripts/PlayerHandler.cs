using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public int Health = 3;

    public Transform HeartContainer;
    public GameObject HeartPreFab;

    private List<RawImage> Hearts = new List<RawImage>();
    void Start()
    {
        CreateHearts();
    }

    void Update()
    {

    }

    public void ChangeHealth(int amount)
    {
        Health += amount;

        CalculateHearts();
    }

    void CreateHearts()
    {
        for (int i = 0; i < Health; i++) // Skappar hjärtan baserat på health
        {
            GameObject heartObj = Instantiate(HeartPreFab, HeartContainer);
            heartObj.name = "Heart" + i;

            RectTransform rt = heartObj.GetComponent<RectTransform>(); // Placera Hjärtan
            rt.anchoredPosition = new Vector2(-1158 + (178 *i), 569);

            RawImage heartImage = heartObj.GetComponent<RawImage>(); // lägga til i listan

            if (heartImage != null)
                Hearts.Add(heartImage);
        }
    }

    void CalculateHearts()
    {
        for (int i = 0; i < Hearts.Count; i++)
        {
                Hearts[i].gameObject.SetActive(i < Health);
        }
    }
}