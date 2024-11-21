using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private AudioClip[] playerSounds;

    private bool canMove = true;
    
    private Rigidbody2D rb;
    private AudioSource playerAS;
    private LevelManager levelManager;
    
    private float speed = 2f;
    private float jumpHeight = 10f;
    private float wallCheckDistance = 0.7f;
    
    private float horizontalval;
    private float runSpeedMutiplier = 2;

    private int airJumps;
    private int maxAirJumps;
    
    private bool isGrounded;
    private bool isRunning;
    private bool facingRight = true;

    private float elevation;
    
    private void Awake()
    {
        //Initialization
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAS = GetComponent<AudioSource>();
        
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();

        maxAirJumps = 1;
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
        
        //Get horizontal value
        horizontalval = Input.GetAxisRaw("Horizontal");
        
        //Running input
        isRunning = Input.GetKey(KeyCode.LeftShift);
        
        // Jumping & Double Jumping
        if (Input.GetButtonDown("Jump") && airJumps > 0)
        {
            animator.SetBool("Jump", true);
            rb.velocity = new Vector2(rb.velocity.x,0);
            rb.AddForce(new Vector2(0f, 25 * jumpHeight));
            if (!isGrounded)
            {
                playerAS.clip = playerSounds[1];
                airJumps--;
            }
            else
            {
                playerAS.clip = playerSounds[0];
            }
            
            playerAS.Play();
            
            //TODO: Control jumps by letting go at desired height
        }

        checkCollision();
        
        // Set yVelocity
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void FixedUpdate()
    {
        Move(horizontalval);
    }
    
    void Move(float dir)
    {
        //horizontal movement
        
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
        // Debug.Log(rb.velocity.x);
        
        // idle 0; walk 4; run 8
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }

    private void checkCollision()
    {
        Vector2 crown = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Vector2 feet = new Vector2(transform.position.x, transform.position.y - 1f);
        bool groundCheckL = Physics2D.Raycast(feet, Vector2.left, 0.5f, groundMask);
        bool groundCheckR = Physics2D.Raycast(feet, Vector2.right, 0.5f, groundMask);
        bool rayCrownL= Physics2D.Raycast(crown, Vector2.left, wallCheckDistance, groundMask);
        bool rayCrownR= Physics2D.Raycast(crown, Vector2.right, wallCheckDistance, groundMask);
        bool rayFeetL= Physics2D.Raycast(feet, Vector2.left, wallCheckDistance, groundMask);
        bool rayFeetR= Physics2D.Raycast(feet, Vector2.right, wallCheckDistance, groundMask);

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

        // Wall jumping
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we are colliding with a Tilemap
        Tilemap tilemap = collision.collider.GetComponentInParent<Tilemap>();
        if (tilemap != null)
        {
            // Get the contact point of the collision
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
                    canMove = false;
                    playerAS.Stop();
                    playerAS.clip = playerSounds[3];
                    //playerAS.time = .5f;
                    playerAS.Play();

                    StartCoroutine(EndGameWithSound());
                }
            }
        }
    }
    IEnumerator EndGameWithSound()
    {
        yield return new WaitForSeconds(playerSounds[3].length - 0.5f);
        levelManager.endGame(false);
    }
}
