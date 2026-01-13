using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int Speed;
    public int JumpPower;

    public GameObject GunPart;
    public GameObject FlooringHolder; // Store all the jump things

    Rigidbody2D Player;

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

        
    }

    // Update is called once per frame
    void Update()
    {
        float direction = 0f;


        if (Keyboard.current.dKey.isPressed)
        {
            direction = 1f; // höger
           //GunPart.transform.position = new Vector2(1.4f, 0.14f);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            direction = -1f; // vänster
          //  GunPart.transform.position = new Vector2(-1.4f, 0.14f);
        }


        if (Keyboard.current.spaceKey.wasPressedThisFrame && !DashCooldown)
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

    private IEnumerator DashCooldownTimer() // Väntetid för dash
    {
        yield return new WaitForSeconds(DashCooldownTime);
        DashCooldown = false;
    }

    private IEnumerator FallThrough() // Väntetid för dash
    {
        foreach (Transform child in FlooringHolder.transform) // rotate to change the direction of it, making the player fall through
        {
            GameObject item = child.gameObject;
            PlatformEffector2D effector = item.GetComponent<PlatformEffector2D>();

            effector.rotationalOffset = 180f;
        }

        yield return new WaitForSeconds(0.3f);


        foreach (Transform child in FlooringHolder.transform) // fixing the rotation
        {
            GameObject item = child.gameObject;
            PlatformEffector2D effector = item.GetComponent<PlatformEffector2D>();

            effector.rotationalOffset = 0f;
        }
    }
}
