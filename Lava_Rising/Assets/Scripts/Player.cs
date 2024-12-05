using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] public AudioClip[] avaSounds;
    [SerializeField] public AudioClip[] harrySounds;
    private AudioSource playerAS;
    public AudioSource backgroundMusicSource;
    
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip[] playerSounds;
    
    private Camera mainCamera;
    
    private Rigidbody2D rb;
    private LevelManager levelManager;
    
    private float speed = 2f;
    private float jumpHeight = 10f;
    private float wallCheckDistance = 0.7f;
    private float horizontalval;
    private float runSpeedMutiplier = 2;
    private float elevation;
    private float targetElevation;

    private int airJumps;
    private int maxAirJumps;
    
    private Vector2 crown;
    private Vector2 feet;
    
    private bool isGrounded;
    private bool isRunning;
    private bool facingRight = true;
    private bool closeToWin;
    private bool canMove = true;
    
    private void Awake()
    {
        //Initialization
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        backgroundMusicSource = audioSources[0];
        playerAS = audioSources[1];
        
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        targetElevation = levelManager.getTarget();
        
        closeToWin = false;
        maxAirJumps = 1;
    }
    
    void Start()
    {
        playerAS.volume = 0.7f;
        if (SceneManager.GetActiveScene().buildIndex != 6)
        {
            if (StartScreenManagerController.selectedMusic == "Harry")
            {
                backgroundMusicSource.clip = harrySounds[0];
                backgroundMusicSource.volume = 0.3f;
            }
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
        else
        {
            backgroundMusicSource.clip = harrySounds[3];
            backgroundMusicSource.volume = 0.5f;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }
    
    void Update()
    {
        if (!canMove)
        {
            speed = 0;
            animator.SetBool("Jump", false);
            return;
        }
        
        elevation = transform.position.y;
        
        if (elevation >= targetElevation)
        {
            Debug.Log("Win");
        }
        if (SceneManager.GetActiveScene().buildIndex == 5 && elevation >= targetElevation * 0.75f && !closeToWin)
        {
            closeToWin = true;
            levelManager.intensifyMusic();
        }
        
        //Get horizontal value
        horizontalval = Input.GetAxisRaw("Horizontal");
        
        //Running input
        isRunning = Input.GetKey(KeyCode.LeftShift);
        
        // Jumping & Double Jumping
        // Jumping & Double Jumping & Wall Jumping
        crown = new Vector2(transform.position.x, transform.position.y + 0.5f);
        feet = new Vector2(transform.position.x, transform.position.y - 1f);
        checkGroundCollision();
        int wall = wallCling();
        
        if (Input.GetButtonDown("Jump") && (airJumps > 0 || wall !=0))
        {
            animator.SetBool("Jump", true);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, 25 * jumpHeight));
            if (!isGrounded)
            {
                if (wall == 0)
                {
                    playerAS.clip = playerSounds[1];
                    airJumps--;
                }
                else
                {
                    playerAS.clip = playerSounds[2];
                }
            }
            else
            {
                playerAS.clip = playerSounds[0];
            }
            
            playerAS.Play();
            
            //TODO: Control jumps by letting go at desired height
        }
        // Set yVelocity
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void FixedUpdate()
    {
        Move(horizontalval);
    }
    
    void Move(float dir)
    {
        //movement speed
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        
        if (isRunning)
        {
            xVal *= runSpeedMutiplier;
        }
        
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;
        
        //Mirroring the move if go opposite direction
        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
        
        // idle 0; walk 4; run 8
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }

    private void checkGroundCollision()
    {
        bool groundCheckL = Physics2D.Raycast(feet, Vector2.left, 0.5f, groundMask);
        bool groundCheckR = Physics2D.Raycast(feet, Vector2.right, 0.5f, groundMask);

        // Check if grounded, refresh jumps if so
        if (groundCheckL || groundCheckR)
        {
            if (!isGrounded)
            {
                playerAS.clip = playerSounds[2];
                playerAS.Play();
            }

            isGrounded = true;
            airJumps = maxAirJumps;
            animator.SetBool("Jump", false);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("Jump", true);
        }
    }
    private int wallCling()
    {
        bool rayCrownL= Physics2D.Raycast(crown, Vector2.left, wallCheckDistance, groundMask);
        bool rayCrownR= Physics2D.Raycast(crown, Vector2.right, wallCheckDistance, groundMask);
        bool rayFeetL= Physics2D.Raycast(feet, Vector2.left, wallCheckDistance, groundMask);
        bool rayFeetR= Physics2D.Raycast(feet, Vector2.right, wallCheckDistance, groundMask);
        if (rayCrownL && rayFeetL)
        {
            return 1;
        }
        if (rayCrownR && rayFeetR)
        {
            return 2;
        }
        
        return 0;
}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Check if we are colliding with a Tilemap
        Tilemap tilemap = collision.collider.GetComponentInParent<Tilemap>();
        if (tilemap != null)
        {
            ContactPoint2D contact = collision.contacts[0];
            
            // Convert the contact point to grid coordinates
            Vector3Int gridPosition = tilemap.WorldToCell(contact.point);
            Vector3Int belowGridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 1, gridPosition.z);
            
            // Get the tile at the grid position
            TileBase tile = tilemap.GetTile(belowGridPosition);
            
            //End game if the player touches lava
            if (tile != null)
            {
                if (tile.name == "tileset_23")
                {
                    StartCoroutine(EndGameWithSound());
                    canMove = false;
                    playerAS.Stop();
                    backgroundMusicSource.Stop();
                    playerAS.clip = playerSounds[3];
                    //playerAS.time = .5f;
                    playerAS.Play();
                }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Secret"))
        {
            if (LevelManager.chooseMusic == "Harry")
            {
                Debug.Log("Entering Secret Level!");
                SceneManager.LoadScene("Scenes/LevelS");
            }
            else
            {
                Debug.Log("Secret level inaccessible with Ava's music.");
            }
        }

        if (collision.CompareTag("doorBack"))
        {
            SceneManager.LoadScene("Scenes/Level3");
        }
        
    }
    
    IEnumerator EndGameWithSound()
    {
            yield return new WaitForSeconds(playerSounds[3].length - 0.5f); 
            levelManager.endGame(false);
    }
    
}
