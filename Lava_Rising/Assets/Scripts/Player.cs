
using System;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float speed = 2f;
    [SerializeField] private Transform groundCheckCollider;
    
    private Rigidbody2D rb;
    private float horizontalval;
    private float runSpeedMutiplier = 2;
    
    Animator animator;

    private bool isGrounded = false;
    bool isRunning = false;
    bool facingRight = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        //Get horizontal value
        horizontalval = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
    }

    void FixedUpdate()
    {
        Move(horizontalval);
    }

    void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, .2f);
    }

    void Move(float dir)
    {
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        
        if (isRunning)
        {
            xVal *= runSpeedMutiplier;
        }
        
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;
        
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
}
