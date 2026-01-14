using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    // Sprite and animator variables to allow EnemyView to assign var.

    float speed;
    int damage;
    float damageRate;
    float distanceToPlayer;
    float jumpPower;

    bool cooldown = false;

    Rigidbody2D rb;
    Transform player;

    // Function that sets all values as a replacement for a constructor
    public void SetValues(Rigidbody2D rb, float speed, float distanceToPlayer, float damageRate, Transform player, float jumpPower)
    {
        this.speed = speed;
        this.distanceToPlayer = distanceToPlayer;
        this.damageRate = damageRate;
        this.player = player;
        this.rb = rb;
        this.jumpPower = jumpPower;
    }

    // Virtual function to make changes in each subclass
    // Move function calculates position and speed and when to Attack
    public virtual void Move(int direction, Transform enemyTransform, PlayerHandler playerHandler) // 1 is right, -1 is left
    {
        float x = Mathf.Abs(enemyTransform.position.x - player.position.x);
        float y = Mathf.Abs(enemyTransform.position.y - player.position.y);

        if (x > distanceToPlayer) // If enemy is not close to Player, it will move forward
        {
            rb.linearVelocityX = direction * speed; // Changed so it affects only the x axis
        }
        else // If enemy is close to player, it will stop
        {
            rb.linearVelocity = new Vector2(0, 0);

            if (y <= distanceToPlayer)
            {
                // When the distance is enough, attack function will run

                if (!cooldown)
                {
                    // Add weapon system where it either shoots or hits
                    StartCoroutine(WaitToAttack());
                    Attack(playerHandler);
                }
            }
        }
    }

    // In Subclass, uses a ontrigger fn to run LayerMove fn and checks which z pos the Player is in and calculates if it needs to go down or up
    public void LayerMove(GameObject groundObject, float distance, GameObject flooringHolder)
    {
        // Checks z player pos
        PlayerMovement pM = player.GetComponent<PlayerMovement>();


        // Layer check and decides if it should jump or go down
        if (transform.position.y * distance < player.position.y) // Player is Above
        {
            // Code for moving upwards
            rb.linearVelocityY = jumpPower; // Only affects y axis
            Debug.Log("Enemy Jumped");
        }
        else if (transform.position.y * (distance - 0.2f) > player.position.y) // Player is Under
        {
            // Code for moving Downwards
            if (groundObject.GetComponent<PlatformEffector2D>() != null)
            {
                Debug.Log("Enemy went down");

                StartCoroutine(FallThrough(flooringHolder));
            }
            else
            {
                Debug.Log("PlatformEffector2D is null in Enemy Class Script");
            }
        }
        else // Player is on the same level
        {
            Debug.Log("Enemy stayed the same");
        }
    }

    public virtual void Attack(PlayerHandler player)
    {
        player.ChangeHealth(-damage);
    }

    IEnumerator WaitToAttack()
    {
        cooldown = true;
        yield return new WaitForSecondsRealtime(damageRate);
        cooldown = false;
    }

    private IEnumerator FallThrough(GameObject flooringHolder)
    {
        Collider2D enemyCollider = GetComponent<Collider2D>();

        foreach (Transform child in flooringHolder.transform)
        {
            Collider2D platformCollider = child.GetComponent<Collider2D>();
            if (platformCollider != null)
            {
                // Ignore collisions between player and platform
                Physics2D.IgnoreCollision(enemyCollider, platformCollider, true);
            }
        }

        yield return new WaitForSeconds(0.3f);

        foreach (Transform child in flooringHolder.transform)
        {
            Collider2D platformCollider = child.GetComponent<Collider2D>();
            if (platformCollider != null)
            {
                // Re-enable collisions
                Physics2D.IgnoreCollision(enemyCollider, platformCollider, false);
            }
        }
    }

    // Get Functions
    public Rigidbody2D GetRigidBody2D()
    {
        return rb;
    }

    // Set Functions

    public void SetRigidBody2D(Rigidbody2D rb)
    {
        this.rb = rb;
    }
}
