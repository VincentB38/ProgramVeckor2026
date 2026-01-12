using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Interact : MonoBehaviour
{
    public GameObject Holder;
    public GameObject playerCanvas;
    public float Distance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in Holder.transform)
        {
            GameObject item = child.gameObject;

            float dist = Vector2.Distance(item.transform.position, transform.position); // distance mellan spelare och item

            if (dist < Distance)
            {
                playerCanvas.SetActive(true);
                print("Player is close to " + item.name);

            } else
            {
                playerCanvas.SetActive(false);
            }
        }
    }
}
