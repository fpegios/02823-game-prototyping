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

    private float coeff = 1;
    private float stableCoeff;

    private bool isJumping;

    private bool isClimbing;

    public GameObject camera;

    private bool isDropping;

    // Use this for initialization
    void Start()
    {
        // Getting the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3;
        stableCoeff = coeff;
    }

    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !isJumping)
        {
            Debug.Log("Pressed!");
            rb.AddForce(new Vector2(0, jumpSpeed + coeff), ForceMode2D.Impulse);
            this.isJumping = true;

        }
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
            if (Time.frameCount > tempCount + frameRange)
            {
                // Reset the variable which stored the previous frame number 
                tempCount = 0;
                // Setting to false the most external boolean variable
                toCount = false;
            }
            else
            {
                // Otherwise, we accelerate the player's speed by a certain amount per frame
                rb.velocity = new Vector2(maxSpeed + speedUpValue, rb.velocity.y);
                // Setting the speed indicated by the maxSpeed variable with the current one
                maxSpeed = rb.velocity.x;
            }
        }
        // If it's not the right time to boost the player, then we make the player move at a constant speed
        else if (!isClimbing)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        // Updating the velocity property of the object

        //Debug.Log(rb.velocity.x);

        // Getting the input - if Space is pressed
        
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, -13);
        camera.transform.position = newPos;

        //print(isJumping);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Climb"))
        {
            rb.velocity = new Vector2(maxSpeed + 3, rb.velocity.y);
            /*transform.rotation = collision.transform.rotation;
            Debug.Log(collision.transform.rotation);*/
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = 45;
            transform.eulerAngles = eulerAngles;
            //isJumping = true;
        }
        if (collision.transform.CompareTag("Drop"))
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = -45;
            transform.eulerAngles = eulerAngles;
            isDropping = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.CompareTag("Ground") || collision.transform.CompareTag("Climb") || collision.transform.CompareTag("Drop")))
        {
            isJumping = false;
        }

        if (collision.transform.CompareTag("Rock")) {
            Debug.Log("DEATH FROM ROCK!");
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Climb"))
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            isClimbing = false;
            //isJumping = false;
        }
        if (collision.transform.CompareTag("Drop"))
        {
            isClimbing = false;
            isDropping = false;
        }
        if (collision.transform.CompareTag("Ground"))
        {
            //isJumping = true;
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
            coeff += 1.5f;
        }

        if (collision.CompareTag("EndClimb"))
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = 0;
            transform.eulerAngles = eulerAngles;
        }

        if (collision.CompareTag("Death"))
        {
            Debug.Log("DEATH");
        }

        // player triggers the rock fall
        if (collision.tag == "RockFall") {
            Debug.Log("ROCK IS FALLING!");
            // get trigger's parent object -> get first child -> increase gravity
            collision.gameObject.transform.parent.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>().gravityScale = 2.0f;
        }
        
    }
}