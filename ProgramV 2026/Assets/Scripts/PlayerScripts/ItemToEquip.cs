using UnityEngine;

public class ItemToEquip : MonoBehaviour
{
    public GameObject WeaponPreFab;
    public int PointGain;
    public GameObject GetItem()
    {
        return WeaponPreFab;
    }
}
