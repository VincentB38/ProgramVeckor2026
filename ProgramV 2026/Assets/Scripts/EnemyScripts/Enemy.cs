using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    // Sprite and animator variables to allow EnemyView to assign var.

    float speed;
    int damage;
    float damageRate;
    float distanceToPlayer;
    Rigidbody2D rb;

    // Construct unused currently
    public Enemy(float speed, float distanceToPlayer)
    {
        this.speed = speed;
        this.distanceToPlayer = distanceToPlayer;
    }

    // Temporary manual "constructor"
    public void SetValues(float speed, float distanceToPlayer, float damageRate)
    {
        this.speed = speed;
        this.distanceToPlayer = distanceToPlayer;
        this.damageRate = damageRate;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Virtual function to make changes in each subclass
    public virtual void Move(int direction, Transform playerTransform, Transform enemyTransform, PlayerHandler playerHandler) // 1 is right, -1 is left
    {
        float x = Mathf.Abs(enemyTransform.position.x - playerTransform.position.x);

        float distance = Vector2.Distance(gameObject.transform.position, playerTransform.position);

        if (x > distanceToPlayer) // If enemy is not close to Player, it will move forward
        {
            rb.linearVelocity = new Vector2(direction, 0) * speed;
        }
        else // If enemy is close to player, it will stop
        {
            rb.linearVelocity = new Vector2(0, 0);

            // When the distance is enough, attack function will run
            Attack(playerHandler);
        }
    }

    // In Subclass, uses a ontrigger fn to decide layer by adding an index to each trigger
    public void LayerMove(GameObject layerObject, GameObject enemy)
    {
        enemy.transform.position = layerObject.transform.position;

        // Run function to change collision layer
    }

    public virtual void Attack(PlayerHandler player)
    {
        Debug.Log("Attacked player");

        if (player == null)
        {
            player.ChangeHealth(-damage);
        }
        else
        {
            Debug.LogError("PlayerHandler in Attack Function is missing"); // Have to add player functions to the actual player
        }
        
        StartCoroutine(WaitToAttack());
    }

    IEnumerator WaitToAttack()
    {
        yield return new WaitForSeconds(damageRate);
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
