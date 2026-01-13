using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public GameObject Holder;
    public GameObject PlayerCanvas;

    private TextMeshProUGUI interactText;
    public float Distance;
    void Start()
    {
        Transform textTransform = PlayerCanvas.transform.Find("InteractText");
        interactText = textTransform.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isNearAny = false;
        GameObject nearestItem = null;

        // Loop through all Items
        foreach (Transform child in Holder.transform)
        {
            GameObject item = child.gameObject;
            float dist = Vector2.Distance(item.transform.position, transform.position);

            if (dist < Distance)
            {
                isNearAny = true;
                nearestItem = item;
                break; // Stop at the first nearby item
            }
        }

        if (isNearAny)
        {
            SetText(nearestItem);
            PlayerCanvas.SetActive(true);

            // Check if they press E (Interact)
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                InteractWithItem(nearestItem);
            }
        }
        else   // Player is not near any item
        {
            PlayerCanvas.SetActive(false);
        }
    }

    void InteractWithItem(GameObject Item)
    {
        if (Item.CompareTag("HealthGiver")) // Gives the player an extra life
        {
            Destroy(Item, 0);
            gameObject.GetComponent<PlayerHandler>().ChangeHealth(1);
        } 
        else if (Item.CompareTag("WeaponType")) // weapon function
        {

        }
    }

    void SetText(GameObject Item) // set the text depending on what item it is
    {
        if (Item.CompareTag("HealthGiver"))
        {
            interactText.text = "Press 'E' to Consume";
        }
        else if (Item.CompareTag("WeaponType"))
        {
            interactText.text = "Press 'E' to Pickup";
        }
    }
}
