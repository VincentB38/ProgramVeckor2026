using TMPro;
using UnityEngine;

public class UIProvider : MonoBehaviour
{
    public static UIProvider Instance;
    public TextMeshProUGUI reloadText;

    private void Awake()
    {
        Instance = this;
    }
}
