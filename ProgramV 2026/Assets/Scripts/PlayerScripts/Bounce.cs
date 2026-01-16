using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
public class Bounce : MonoBehaviour
{
    public float bouncePower;

    public GameObject Player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.GetComponent<PlayerHandler>().EndGame("False");
        }
    }
}
