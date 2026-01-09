using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int Speed;
    public int JumpPower;

    Rigidbody2D Player;

    private bool isGrounded;

    public float DashSpeed = 15f;      // Hur snabb dashen är
    public float DashDuration = 0.2f;  // Under hur lång tid
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private bool DashCooldown = false;
    public float DashCooldownTime = 1f;
    void Start()
    {
        Player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction = 0f;

        if (Keyboard.current.dKey.isPressed) direction = 1f;
        else if (Keyboard.current.aKey.isPressed) direction = -1f;


        if (Keyboard.current.qKey.wasPressedThisFrame && !DashCooldown)
        {
            if (direction != 0) // Bara dasha om man rör sig vänster eller höger
            {
                isDashing = true;
                dashTimeLeft = DashDuration;
                DashCooldown = true;
                StartCoroutine(DashCooldownTimer());
            }
        }

        // Gör dash om den är aktiv
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            Player.linearVelocity = new Vector2(direction * DashSpeed, 0);

            if (dashTimeLeft <= 0)
                isDashing = false;
        }
        else
        {
            // Vanlig movement
            Player.linearVelocity = new Vector2(direction * Speed, Player.linearVelocity.y);

            // Hoppa
            if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
            {
                Player.linearVelocity = new Vector2(Player.linearVelocity.x, JumpPower);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // För att se om man är på marken eller inte
    {
        if (collision.gameObject.CompareTag("OnGround") && (collision.gameObject.transform.position.y *1.05f < Player.transform.position.y))
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

    private IEnumerator DashCooldownTimer() // Väntetid
    {
        yield return new WaitForSeconds(DashCooldownTime);
        DashCooldown = false;
    }
}
