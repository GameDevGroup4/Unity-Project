
using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private Tilemap tilemap;
    
    private Rigidbody2D rb;
    private float speed = 2f;
    private float jumpHeight = 10f;
    
    private float horizontalval;
    private float runSpeedMutiplier = 2;

    private int airJumps;
    private int maxAirJumps;
    
    private bool isGrounded;
    private bool isRunning;
    private bool facingRight = true;

    private bool gameOver = false;
    private bool gameWon = true;
    
    private void Awake()
    {
        //Initialization
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        maxAirJumps = 1;
    }
    
    void Update()
    {
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
                airJumps--;
            }
            
            //TODO: Control jumps by letting go at desired height
        }

        // Grounded check
        isGrounded = rb.IsTouchingLayers(groundMask);
        
        // Set yVelocity
        animator.SetFloat("yVelocity", rb.velocity.y);
        
        //game ends when player steps on lava
        
        
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        airJumps = maxAirJumps;

        //Disable jump when grounded
        animator.SetBool("Jump", false);

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

            if (tile != null)
            {
                if (tile.name == "tileset_23")
                {
                    gameOver = true;
                    Debug.Log("Game Over");
                }
            }
        }
    }
}
