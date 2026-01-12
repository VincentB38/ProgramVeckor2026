using UnityEngine;

public class Enemy : MonoBehaviour
{
    string name;

    float speed;
    Rigidbody2D rb;

    public Enemy(float speed, Rigidbody2D rb)
    {
        this.speed = speed;
        this.rb = rb;
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
    public virtual void Move(Vector3 playerPosition)
    {
        
    }

    // In Subclass, uses a ontrigger fn to decide layer by adding an index to each trigger
    public void LayerMove(int layerIndex)
    {
        gameObject.layer = layerIndex;

        // Run function to change collision layer
    }

    public virtual void Attack()
    {
        
    }

    // Get Functions
    public Rigidbody2D GetRigidBody2D()
    {
        return rb;
    }
}
