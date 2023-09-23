using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("----------Float----------")]
    private float horizontal;
    public float speed = 4f;
    public float jumpForce = 18f;
    public float crouchForce = 8f;
    public float doubleJumpForce = 14f;

    [Header("----------Bool----------")]
    private bool isCrouching;
    private bool isJumping;
    private bool _sebasIsSelected;
    private bool _mrGuestIsSelected;
    private bool doubleJump;


    [Header("----------Colliders----------")]
    public Vector2 standingSize;
    public Vector2 crouchingSize;

    [Header("----------Player----------")]
    private BoxCollider2D Collider;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("----------Animator----------")]
    private Animator animator;

    [Header("----------Coins----------")]
    public TextMeshProUGUI coinsText;
    public static int numberOfCoins;

    [Header("----------AudioManager----------")]
    AudioManager audioManager;

    [Header("----------Skins----------")]
    public AnimatorOverrideController skinSebas;
    public AnimatorOverrideController skinMrGuest;

    [Header("----------GameObject----------")]
    public GameObject doubleJumpImage;

    void Awake()
    {
        numberOfCoins = PlayerPrefs.GetInt("numberOfCoins", 0);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        _sebasIsSelected = (PlayerPrefs.GetInt("sebasIsSelected") != 0); //Checks if sebas is selected in the menu
        _mrGuestIsSelected = (PlayerPrefs.GetInt("mrGuestIsSelected") != 0); //Checks if guest is selected in the menu
    }

    void Start()
    {
        Collider = FindObjectOfType<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.size = standingSize;
        standingSize = Collider.size;

        animator = GetComponent<Animator>();

        if(_sebasIsSelected)
        {
            SkinSebas();
        }
        else if(_mrGuestIsSelected)
        {
            SkinMrGuest();
        }
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        //Plays audio when player is grounded and moves right/left
        if(IsGrounded())
        {
            if(Input.GetButtonDown("Right") || Input.GetButtonDown("Left"))
            {
                audioManager.PlaySFX(audioManager.crouch);
            }
        }

        if(Input.GetButtonDown("Jump")) // Jump
        {
            if(IsGrounded())
            {
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                audioManager.PlaySFX(audioManager.jump);
            }
            else if(doubleJump) // Jump again if doubleJump is true
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                audioManager.PlaySFX(audioManager.doubleJump);
                doubleJumpImage.gameObject.SetActive(false);
            }
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        //Displays the number of coins
        coinsText.text = Mathf.FloorToInt(numberOfCoins).ToString("D3");

        Crouch();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isCrouching", isCrouching);
    }

    private void Crouch()
    {
        if(Input.GetButtonDown("Down"))
        {
            rb.velocity = Vector3.down * crouchForce;
            Collider.size = crouchingSize;
            isCrouching = true;
            audioManager.PlaySFX(audioManager.crouch);
        }
        
        if(Input.GetButtonUp("Down"))
        {
            Collider.size = standingSize;
            isCrouching = false;
        }
    }
    
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Obstacle"))
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Coin")
        {
            numberOfCoins++;
            audioManager.PlaySFX(audioManager.coin);
            PlayerPrefs.SetInt("numberOfCoins", numberOfCoins);
            collision.gameObject.SetActive(false);
        }

        if(collision.gameObject.tag == "DoubleJump")
        {
            doubleJump = true;
            doubleJumpImage.gameObject.SetActive(true);
            audioManager.PlaySFX(audioManager.powerUp);
            collision.gameObject.SetActive(false);
        }
    }

    public void SkinSebas()
    {
        GetComponent<Animator>().runtimeAnimatorController = skinSebas as RuntimeAnimatorController;
    }

    public void SkinMrGuest()
    {
        GetComponent<Animator>().runtimeAnimatorController = skinMrGuest as RuntimeAnimatorController;
    }
}