using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int Speed;
    public int JumpPower;

    public GameObject GunPart;
    public GameObject FlooringHolder; // Store all the jump things

    Rigidbody2D Player;
    Animator animator;

    private bool isGrounded;

    public float DashSpeed;      // Hur snabb dashen är
    public float DashDuration;  // Under hur lång tid
    public float DashCooldownTime;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private bool DashCooldown = false;


    void Start()
    {
        Player = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction = 0f;

        if (Player.linearVelocityX != 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (Keyboard.current.dKey.isPressed)
        {
            direction = 1f; // höger
            GunPart.transform.position = new Vector2(transform.position.x * 0.89f, transform.position.y);

            transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            0f,
            transform.eulerAngles.z
            );
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            direction = -1f; // vänster
            GunPart.transform.position = new Vector2(transform.position.x * 1.11f, transform.position.y);
            transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            180f,
            transform.eulerAngles.z
);
        }


        if (Mouse.current.rightButton.isPressed && !DashCooldown)
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
            else if (Keyboard.current.sKey.wasPressedThisFrame && isGrounded)
            {
                StartCoroutine(FallThrough());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // För att se om man är på marken eller inte
    {
        // Ground check
        if (collision.gameObject.CompareTag("OnGround") &&
            collision.gameObject.transform.position.y * 1.05f < Player.transform.position.y)
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

    private IEnumerator DashCooldownTimer() // Väntetid för dash
    {
        yield return new WaitForSeconds(DashCooldownTime);
        DashCooldown = false;
    }

    private IEnumerator FallThrough()
    {
        Collider2D playerCollider = Player.GetComponent<Collider2D>();

        foreach (Transform child in FlooringHolder.transform)
        {
            Collider2D platformCollider = child.GetComponent<Collider2D>();
            if (platformCollider != null)
            {
                // Ignore collisions between player and platform
                Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
            }
        }

        yield return new WaitForSeconds(0.3f);

        foreach (Transform child in FlooringHolder.transform)
        {
            Collider2D platformCollider = child.GetComponent<Collider2D>();
            if (platformCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
            }
        }
    }
}
