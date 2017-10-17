using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed;
    public float jumpSpeed;

    private Rigidbody2D rb;
    
    // Use this for initialization
    void Start()
    {
        // Getting the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Updating the velocity property of the object
        rb.velocity = new Vector2(maxSpeed, rb.velocity.y);

        // Getting the input - if Space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Then we update the y value of the velocity property to jump
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
}