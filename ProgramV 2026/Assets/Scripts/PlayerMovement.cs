using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int Speed;
    public int JumpPower;

    Rigidbody2D Player;

    private bool isGrounded;
    void Start()
    {
        Player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction = 0f;

        if (Keyboard.current.aKey.isPressed) // Åka vänster
        {
            direction = -1f;
        }
        else if (Keyboard.current.dKey.isPressed) // Åka höger
        {
            direction = 1f;
        }


        Player.linearVelocity = new Vector2(direction * Speed, Player.linearVelocity.y); // Saken som gör att det funkar

        if (Keyboard.current.wKey.isPressed && isGrounded) // Hoppa 
        {
            Player.linearVelocity = new Vector2(Player.linearVelocity.x, JumpPower);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // För att se om man är på marken eller inte
    {
        if (collision.gameObject.CompareTag("OnGround"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OnGround"))
        {
            isGrounded = false;
        }
    }
}
