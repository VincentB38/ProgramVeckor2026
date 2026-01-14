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

    [SerializeField] int PlayerLayer;

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

    public int PlayerLayerCheck()
    {
        return PlayerLayer;
    }

    // Update is called once per frame
    void Update()
    {
        float direction = 0f;


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
            GunPart.transform.position = new Vector2(transform.position.x *1.11f, transform.position.y);
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
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
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

        print(collision.gameObject.name);
        
            if (int.TryParse(collision.gameObject.name, out int number))
            {
                Debug.Log(number);
                PlayerLayer = number;
            }
     }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OnGround"))
        {
            isGrounded = false;
        }

        PlayerLayer = 0;
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
