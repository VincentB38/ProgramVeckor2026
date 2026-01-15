using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy_Melee : Enemy
{
    [SerializeField] Transform playerTransform;

    bool isGrounded;

    // Variables that corresponds to the base but still can be accessed through the editor
    public float moveSpeed;
    public int damageAmount;
    public float damageRateSeconds;
    public float attackRange;
    public float jumpHeight;
    public float yOffset;


    public GameObject flooringHolder;

    public float jumpCooldown = 2f;  // Cooldown for jump
    public float jumpTimer;  // The variable that is used for calculating the time
    bool hasJumped = false;
    bool shouldJump = false;

    GameObject groundObject;


    // Adding variables to sprite and animator


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        flooringHolder = GameObject.Find("Floors");

        base.SetValues(GetComponent<Rigidbody2D>(), moveSpeed, attackRange, damageRateSeconds, playerTransform, jumpHeight, damageAmount);

    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            if (!hasJumped && shouldJump) // Checks if it has jumped and if it should
            {
                base.LayerMove(groundObject, yOffset, flooringHolder);
                hasJumped = true;
                shouldJump = false;
                jumpTimer = jumpCooldown;
            }
            else if (hasJumped)
            {
                CalculateCooldown();
            }
        }

        Move(CheckPlayerPosition(), transform, playerTransform.gameObject.GetComponent<PlayerHandler>());

    }

    void CalculateCooldown()
    {
        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.deltaTime;
        }
        else
        {
            hasJumped = false;
        }
    }

    // True if the player is to the right and false if left
    int CheckPlayerPosition()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(
          transform.eulerAngles.x,
          180f,
          transform.eulerAngles.z
          );
            return -1; // Player is on the left
        }
        else
        {
            transform.eulerAngles = new Vector3(
          transform.eulerAngles.x,
          0f,
          transform.eulerAngles.z
          );
            return 1; // Player is to the right
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        groundObject = collision.gameObject;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) // These are for the layer triggers to tell the enemy when it is time to jump
        {
            shouldJump = true;
        }
    }
}
