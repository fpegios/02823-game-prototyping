using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{

    public delegate void Action();
    public event Action OnHitGround;
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
    private GameObject GameOverMenu, Pause;
    private float initialYCameraValue;
    private bool firstTime = true;
    public float minimumYPosition;
    private Animator animator;
    private bool isGrounded, isDoubleJumpActive;
    private List<PlayerState> storedPlayerStates;
    private string powerUp;
    private bool isRespawning;
    private Vector3 respawnPosition;
    private float playerStateSaveCount;
    public enum GameState {Play, Pause, Death, Complete};
    public static GameState gameState;
    private GameObject bubble;
    private Inventory inventory;
    public AudioClip jumpSound, groundSound, gameOverSound, destroyEnemySound;
    public AudioClip mineSound;
    public AudioClip gameLevelMusic;
    public float samplingTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameOverMenu = GameObject.Find("GameOverMenu");
        GameOverMenu.SetActive(false);
        Pause = GameObject.Find("Pause");
        Pause.SetActive(false);

        inventory = GameObject.FindObjectOfType<Inventory>();

        bubble = GameObject.Find("Bubble");
        if(!bubble)
            throw new UnityException("Bubble could not be found, ensure that it exists in the scene.");

        storedPlayerStates = new List<PlayerState>();
        gameState = GameState.Play;
    }
    void Start()
    {
        bubble.SetActive(false);
        initialYCameraValue = camera.transform.position.y;
        tempCamera = camera;
        SoundManager.instance.PlayMusic(gameLevelMusic);
    }
    
    void FixedUpdate()
    {

    }

    void OnEnable (){
        OnHitGround += playGroundSound;
    }

    void OnDisable(){
        OnHitGround -= playGroundSound;
    }

    private void ConsumePowerUp()
    {
        if(powerUp.Equals("Rewind")){
            RespawnPlayer();
        }
        if(powerUp.Equals("Shield")){
            bubble.SetActive(true);
        }

        powerUp = null;
        inventory.ConsumePowerUp();
    }  

    private void RespawnPlayer()
    {
        isRespawning = true;
        var latestStates = storedPlayerStates.Skip(storedPlayerStates.Count - 4);
        respawnPosition = latestStates.First().Position;
        GetComponent<BoxCollider2D>().enabled = false;
    }    

    private void Jump(){
        SoundManager.instance.PlayUserSfx_1(jumpSound);
        rb.velocity = Vector2.up * jumpVelocity;
        isGrounded = false;
    }

    private void HandlePhysics(){
        HandleJumpPhysics();
        SetConstantRunSpeed();
    }

    private void HandleJumpPhysics(){
        if(rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1 ) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)){
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

    private void MovePlayerToRespawnPosition(){
        rb.velocity = Vector2.zero;
        transform.position = Vector3.Lerp(transform.position, respawnPosition, 2f * Time.deltaTime);
    }

    private bool IsPositionCloseEnough(Vector3 src, Vector3 dest, float threshold){
        return Vector3.Distance(src, dest) < threshold;
    }

    private void StopRespawning(){
        isRespawning = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleGameState();

        if (gameState == GameState.Play) {
            if(!isRespawning){
                if((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && (isGrounded || isDoubleJumpActive))
                    Jump();

                if((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) && !string.IsNullOrEmpty(powerUp))
                    ConsumePowerUp();

                if (isBoostActive)
                    Boost();

                HandlePhysics(); 
        
    
            }
            else{
                MovePlayerToRespawnPosition();

                if(IsPositionCloseEnough(transform.position, respawnPosition, 0.1f))
                    StopRespawning();
            }
            
            StorePlayerState();
            TransformCamera();

            if (HasFallenDown())
                InvokeDeath();
        }
    }

    void StorePlayerState(){
        if (playerStateSaveCount < samplingTime) {
            playerStateSaveCount += Time.deltaTime;
        } else {
            if (isGrounded)
            {
                playerStateSaveCount = 0;
                var playerState = ScriptableObject.CreateInstance<PlayerState>();
                playerState.Init(rb.velocity, rb.position, isGrounded);
                storedPlayerStates.Add(playerState);
            }
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
            if (OnHitGround != null)
                OnHitGround ();
        }
        else if(collision.transform.CompareTag("Climb")){
            isGrounded = true;
           // if (!isGrounded) {
                if (OnHitGround != null)
                    OnHitGround ();
            //}
            animator.SetBool("IsGrounded", true);
        }
        else if (collision.transform.CompareTag("Trampoline"))
        {
            rb.AddForce(new Vector2(0, jumpSpeed + coeff + 20), ForceMode2D.Impulse);
            onTrampoline = true;
            if (!isGrounded) {
                if (OnHitGround != null)
                    OnHitGround ();
            }
            animator.SetBool("IsGrounded", true);
        }
        else if (collision.transform.CompareTag("Rock"))
        {
            InvokeDeath();
        }
        else if (collision.transform.CompareTag("Mine")) {
            InvokeDeath();
            SoundManager.instance.PlayCollisionSfx(mineSound);
        }
        else if (collision.transform.CompareTag("MovingEnemy"))
        {
            if (transform.position.y > collision.gameObject.transform.position.y + collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y) {
                SoundManager.instance.PlayCollisionSfx(destroyEnemySound);
                Destroy(collision.gameObject);
                rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            } else {
                InvokeDeath();
            }
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
        else if (collision.CompareTag("Rewind")){
            var sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            inventory.AddPowerUp(sprite);
            powerUp = "Rewind";
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Shield"))
        {
            var sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            inventory.AddPowerUp(sprite);
            powerUp = "Shield";
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Goal")){
            gameState = GameState.Complete;
            FreezePlayer();
            SoundManager.instance.StopMusic(gameLevelMusic);
        }
    }

    private void InvokeDeath(){
        SoundManager.instance.PlayUserSfx_2(gameOverSound);
        SoundManager.instance.StopMusic(gameLevelMusic);
        gameState = GameState.Death;
        GameOverMenu.SetActive(true);
        this.gameObject.SetActive(false);
        Debug.Log(storedPlayerStates.Count);
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
            collision.gameObject.transform.parent.GetComponent<Rigidbody2D>().gravityScale = 2.0f;
        }
    }

    private void playGroundSound() {
        SoundManager.instance.PlayUserSfx_2(groundSound);
    }

    private void ToggleGameState() {
        if (gameState == GameState.Play) {
            gameState = GameState.Pause;
            Pause.SetActive(true);
            FreezePlayer();
        } else if (gameState == GameState.Pause){
            gameState = GameState.Play;
            Pause.SetActive(false);
            animator.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    private void FreezePlayer(){
        animator.enabled = false;
        rb.velocity = Vector2.zero;
        if (isClimbing) {
            rb.bodyType = RigidbodyType2D.Static;
        } else {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}