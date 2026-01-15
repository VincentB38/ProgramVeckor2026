using TMPro;
using UnityEngine;

public class UIProvider : MonoBehaviour
{
    public static UIProvider Instance;
    public TextMeshProUGUI reloadText;

    private void Awake() // This script is used to help prefabs find the right thing
    {
        Instance = this;
    }
}
