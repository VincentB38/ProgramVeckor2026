using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    string name;

    float speed;
    int damage;
    float distanceToPlayer;
    Rigidbody2D rb;

    // Construct unused currently
    public Enemy(float speed, float distanceToPlayer)
    {
        this.speed = speed;
        this.distanceToPlayer = distanceToPlayer;
    }

    // Temporary manual constructor
    public void SetValues(float speed, float distanceToPlayer)
    {
        this.speed = speed;
        this.distanceToPlayer = distanceToPlayer;
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
    public virtual void Move(int direction, Transform playerTransform) // 1 is right, -1 is left
    {
        float x = gameObject.transform.position.x - playerTransform.position.x;

        float distance = Vector2.Distance(gameObject.transform.position, playerTransform.position);

        if (Mathf.Abs(distance) > distanceToPlayer) // If enemy is not close to Player, it will move forward
        {
            rb.linearVelocity = new Vector2(direction, 0) * speed;
            Debug.Log("Moving to Player" + distance);
        }
        else // If enemy is close to player, it will stop
        {
            rb.linearVelocity = new Vector2(0, 0);
            Debug.Log("Stopping near Player" + distance);
        }
    }

    // In Subclass, uses a ontrigger fn to decide layer by adding an index to each trigger
    public void LayerMove(int layerIndex)
    {
        gameObject.layer = layerIndex;

        // Run function to change collision layer
    }

    public virtual void Attack(PlayerHandler player)
    {
        player.ChangeHealth(-damage);
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
