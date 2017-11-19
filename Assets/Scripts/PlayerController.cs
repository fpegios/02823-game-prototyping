using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float maxSpeed;
    public float jumpSpeed;

    [Range(1, 20)]
    public float jumpVelocity = 12;
    public float fallMultiplier = 5f;
    public float lowJumpMultiplier = 17f;

    // Reference to the Rigidbody2D component attached to the player
    private Rigidbody2D rb;

    // Frame range settable in Unity
    public float frameRange;

    // Boolean variable which indicates if the time to boost the player has come
    private bool isBoostActive = false;

    // Variable which will store the frame number at a certain point in time
    private float frameCountOnBoostActivation;

    // Boolean variable which indicates if we have to save the current frame number
    private bool toSave;

    // Variable which indicates the speed up coefficient
    public float speedUpValue;

    private float coeff = 1;
    private float stableCoeff;

    private bool isJumping;

    private bool isClimbing;

    public Camera camera;
    private Camera tempCamera;

    private bool isDropping;

    public float doubleJumpCoefficient;

    private bool onTrampoline;
    
    private GameObject GameOverMenu;

    private float initialYCameraValue;

    private bool firstTime = true;

    public float minimumYPosition;

    public float jumpMultiplier;

    private Animator animator;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameOverMenu = GameObject.Find("GameOverMenu");
        GameOverMenu.SetActive(false);
    }
    void Start()
    {
        stableCoeff = coeff;
        initialYCameraValue = camera.transform.position.y;
        tempCamera = camera;
    }
    
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space) && isGrounded)
            Jump();

        if (isBoostActive)
            Boost();

        HandlePhysics();      
    }

    private void Jump(){
        rb.velocity = Vector2.up * jumpVelocity;
        isGrounded = false;
    }

    private void HandlePhysics(){
        HandleJumpPhysics();
        SetConstantRunSpeed();
    }

    private void HandleJumpPhysics(){
        if(rb.velocity.y < 0 && !isClimbing){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1 ) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && !isClimbing){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    
    private void SetConstantRunSpeed(){
        rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
    }

    private void Boost(){
        if (Time.frameCount > frameCountOnBoostActivation + frameRange)
        {
            frameCountOnBoostActivation = 0;
            isBoostActive = false;
        }
        else
        {
            rb.velocity = new Vector2(maxSpeed + speedUpValue, rb.velocity.y);
            maxSpeed = rb.velocity.x;
        }
    }

    void Update()
    {

        TransformCamera();
        
        // Check Y position of the player -> if < minimumYPosition, then the player has fallen and must be killed
        if (transform.position.y < minimumYPosition)
        {
            // TODO: kill player for falling
        }
        
    }

    private void TransformCamera(){
        if (firstTime)
        {
            var newPos = new Vector3(transform.position.x, initialYCameraValue, -30);
            camera.transform.position = newPos;
            firstTime = false;
            tempCamera.transform.position = camera.transform.position;
        }
        
        if (transform.position.y > -45f)
        {
            float offset = transform.position.y + 45f;
            camera.transform.position = new Vector3(transform.position.x, initialYCameraValue+offset*0.92f, -30);
        }
        else
        {
            camera.transform.position = new Vector3(transform.position.x, -49, -30);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Climb"))
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = 45;
            transform.eulerAngles = eulerAngles;
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
        if ((collision.transform.CompareTag("Ground") || collision.transform.CompareTag("Drop")))
        {
            isGrounded = true;
            isJumping = false;
            onTrampoline = false;
            animator.SetBool("IsGrounded", true);
        }
        else if(collision.transform.CompareTag("Climb")){
            isClimbing = true;
            animator.SetBool("IsGrounded", true);
        }
        else if (collision.transform.CompareTag("Trampoline"))
        {
            rb.AddForce(new Vector2(0, jumpSpeed + coeff + 20), ForceMode2D.Impulse);
            isJumping = true;
            onTrampoline = true;
            animator.SetBool("IsGrounded", true);
        }
        else if (collision.transform.CompareTag("Rock"))
        {
            // make the Rock static to avoid any movement due to player's velocity
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        else if (collision.transform.CompareTag("MovingEnemy") && transform.position.y > collision.gameObject.transform.position.y)
        {
            Destroy(collision.gameObject);
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Climb"))
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            isClimbing = false;
            animator.SetBool("IsGrounded", false);
        }
        else if (collision.transform.CompareTag("Drop"))
        {
            isClimbing = false;
            isDropping = false;
            animator.SetBool("IsGrounded", false);
        }
        if (collision.transform.CompareTag("Ground"))
        {
            animator.SetBool("IsGrounded", false);
        }
        if (collision.transform.CompareTag("Trampoline"))
        {
            isJumping = true;
            animator.SetBool("IsGrounded", false);
        }
    }

    // Checking the triggers the players enters 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the tag of the collided trigger object is "BoostPoint"
        if (collision.tag == "BoostPoint")
        {
            // It means we have to boost the player's speed
            isBoostActive = true;
            toSave = true;
            frameCountOnBoostActivation = Time.frameCount;
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
            // activate and show the game over menu
            GameOverMenu.SetActive(true);
            // deacivate and hide the player
            this.gameObject.SetActive(false);
        }

        if (collision.CompareTag("DoubleJump"))
        {
            isJumping = false;
            this.coeff += doubleJumpCoefficient;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DoubleJump"))
        {
            isJumping = true;
            this.coeff -= doubleJumpCoefficient;
        }
        // player triggers the rock fall
        if (collision.tag == "RockFall")
        {
            // get trigger's parent object -> get first child -> increase gravity
            collision.gameObject.transform.parent.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>().gravityScale = 2.0f;
        }

    }
}