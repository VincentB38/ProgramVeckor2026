using UnityEngine;

public class ItemToEquip : MonoBehaviour
{
    public GameObject WeaponPreFab;
    public int PointGain;
    public float floatAmplitude; // How high/low it floats
    public float floatFrequency;   // Speed of floating
    public int HeartGain;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = tempPos;
    }

    public GameObject GetItem() // Get the item
    {
        return WeaponPreFab;
    }
}
