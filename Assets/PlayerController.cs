using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float maxSpeed;
    public float jumpSpeed;

    // Reference to the Rigidbody2D component attached to the player
    private Rigidbody2D rb;

    // Frame range settable in Unity
    public float frameRange;

    // Boolean variable which indicates if the time to boost the player has come
    private bool toCount = false;

    // Variable which will store the frame number at a certain point in time
    private float tempCount;
    
    // Boolean variable which indicates if we have to save the current frame number
    private bool toSave;

    // Variable which indicates the speed up coefficient
    public float speedUpValue;

    // Use this for initialization
    void Start()
    {
        // Getting the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if it's time to boost the player
        if (toCount)
        {
            // Check if it's necessary to save the current frame number
            if (toSave)
            {
                // Setting to false the variable
                toSave = false;
                // Saving the current frame number
                tempCount = Time.frameCount;
            }
            // If the current frame number is higher than the sum of the one previously saved plus the frame range set publicly
            else if (Time.frameCount > tempCount + frameRange)
            {
                // Reset the variable which stored the previous frame number 
                tempCount = 0;
                // Setting to false the most external boolean variable
                toCount = false;
            }
            else
            {
                // Otherwise, we accelerate the player's speed by a certain amount per frame
                rb.velocity = new Vector2(rb.velocity.x + speedUpValue, rb.velocity.y);
                // Setting the speed indicated by the maxSpeed variable with the current one
                maxSpeed = rb.velocity.x;
            }
        }
        // If it's not the right time to boost the player, then we make the player move at a constant speed
        else
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        // Updating the velocity property of the object

        Debug.Log(rb.velocity.x);

        // Getting the input - if Space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Then we update the y value of the velocity property to jump
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    // Checking the triggers the players enters 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the tag of the collided trigger object is "BoostPoint"
        if (collision.tag == "BoostPoint")
        {
            // It means we have to boost the player's speed
            toCount = true;
            toSave = true;
        }
    }
}