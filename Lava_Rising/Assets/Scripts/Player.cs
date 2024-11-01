
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float speed = 2f;
    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpHeight = 130f;
    
    const float groundRadius = 0.2f;
    
    
    
    private Rigidbody2D rb;
    private float horizontalval;
    private float runSpeedMutiplier = 2;
    
    
    Animator animator;

    bool jump = false;
    bool isRunning = false;
    bool facingRight = true;
    private void Awake()
    {
        //Initialization
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        //Get horizontal value
        horizontalval = Input.GetAxisRaw("Horizontal");
        
        //Running input
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
        
        //jumping input
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
            jump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump = false;
        }
        
        //Set yVelocity
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalval, jump);
    }
    
    //Check if player is on the ground
    void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundRadius, groundMask);
        //Grounded is true if hit the collider
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
        
        //Disable jump when grounded
        animator.SetBool("Jump", !isGrounded);
    }
    
    
    void Move(float dir, bool jumpFlag)
    {
        #region Jump & Shoot
        
        //Jumping
        if (isGrounded)
        {
            if (jumpFlag)
            {
                // isGrounded = false;
                jumpFlag = false;
                
                //jumping to force
                rb.AddForce(new Vector2(0f, jumpHeight));
            }
        }
        #endregion
        //horizontal movement
        #region Move and Run
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
        #endregion
    }
}
