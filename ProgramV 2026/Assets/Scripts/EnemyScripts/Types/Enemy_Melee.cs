using UnityEngine;

public class Enemy_Melee : Enemy
{
    [SerializeField] Transform playerTransform;

    bool isGrounded;

    // Variables that corresponds to the base but still can be accessed through the editor
    [SerializeField] float moveSpeed;
    [SerializeField] int damageAmount;
    [SerializeField] float damageRateSeconds;
    [SerializeField] float attackRange;
    [SerializeField] float jumpHeight;

    [SerializeField] float jumpCooldown = 2f;  // Cooldown for jump
    [SerializeField] float jumpTimer;  // The variable that is used for calculating the time
    bool hasJumped = false;
    bool shouldJump = false;


    // Adding variables to sprite and animator


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.SetValues(GetComponent<Rigidbody2D>(), moveSpeed, attackRange, damageRateSeconds, playerTransform, jumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            if (!hasJumped && shouldJump) // Checks if it has jumped and if it should
            {
                base.LayerMove();
                hasJumped = true;
                shouldJump = false;
                jumpTimer = jumpCooldown;
            }
            else if (hasJumped)
            {
                CalculateCooldown();
            }
        }

        Move(CheckPlayerPosition(), transform, playerTransform.GetComponent<PlayerHandler>());
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
            return -1; // Player is on the left
        }
        else
        {
            return 1; // Player is to the right
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) // These are for the layer triggers to tell the enemy when it is time to jump
        {
            shouldJump = true;
        }
    }
}
