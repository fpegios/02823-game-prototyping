using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    [Range(1,30)]
    public float runSpeed;
    public float jumpSpeed;

    [Range(1, 30)]
    public float jumpVelocity = 12;
    [Range(1, 10)]
    public float fallMultiplier = 5f;
    [Range(1, 40)]
    public float lowJumpMultiplier = 17f;
    private Rigidbody2D rb;
    public float frameRange;
    private bool isBoostActive = false;
    private float frameCountOnBoostActivation; // Variable which will store the frame number at a certain point in time
    public float speedUpValue; // Variable which indicates the speed up coefficient
    private float coeff = 1;
    private bool isClimbing;
    public Camera camera;
    private Camera tempCamera;
    private bool isDropping;
    private bool onTrampoline;
    private GameObject GameOverMenu;
    private float initialYCameraValue;
    private bool firstTime = true;
    public float minimumYPosition;
    private Animator animator;
    private bool isGrounded, isDoubleJumpActive;
    public enum GameState {Play, Pause};
    public static GameState gameState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameOverMenu = GameObject.Find("GameOverMenu");
        GameOverMenu.SetActive(false);
        gameState = GameState.Play;
    }
    void Start()
    {
        initialYCameraValue = camera.transform.position.y;
        tempCamera = camera;
    }
    
    void FixedUpdate()
    {   
        if (gameState == GameState.Play) {
            if (isBoostActive)
            Boost();

            HandlePhysics(); 
        }
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
        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    }

    private void Boost(){
        if (Time.frameCount > frameCountOnBoostActivation + frameRange)
        {
            frameCountOnBoostActivation = 0;
            isBoostActive = false;
        }
        else
        {
            rb.velocity = new Vector2(runSpeed + speedUpValue, rb.velocity.y);
            runSpeed = rb.velocity.x;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleGameState();

        if (gameState == GameState.Play) {
            if (Input.GetKey(KeyCode.Space) && isGrounded)
                Jump();

            TransformCamera();

            if (HasFallenDown())
                InvokeDeath(); 
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

    private bool HasFallenDown(){
        return transform.position.y < minimumYPosition;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Climb"))
        {
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
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
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
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
            animator.SetBool("IsGrounded", false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BoostPoint")
        {
            isBoostActive = true;
            frameCountOnBoostActivation = Time.frameCount;
            coeff += 1.5f;
        }
        else if (collision.CompareTag("EndClimb"))
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = 0;
            transform.eulerAngles = eulerAngles;
        }
        else if (collision.CompareTag("Death")){
            InvokeDeath();
        }
        else if (collision.CompareTag("DoubleJump"))
        {
            isDoubleJumpActive = true;
        }
    }

    private void InvokeDeath(){
        GameOverMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DoubleJump"))
        {
            isDoubleJumpActive = false;
        }
        if (collision.tag == "RockFall")
        {
            // get trigger's parent object -> get first child -> increase gravity
            collision.gameObject.transform.parent.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>().gravityScale = 2.0f;
        }
    }

    private void ToggleGameState() {
        if (gameState == GameState.Play) {
            gameState = GameState.Pause;
            animator.enabled = false;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        } else {
            gameState = GameState.Play;
            animator.enabled = true;
            rb.isKinematic = false;
        }
    }
}