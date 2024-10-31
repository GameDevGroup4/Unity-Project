
using System;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float speed = 4f;
    
    private Rigidbody2D rb;
    private float horizontalval;
    
    bool facingRight = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        //Get horizontal value
        horizontalval = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        Move(horizontalval);
    }

    void Move(float dir)
    {
        float xVal = dir * speed * 100 * Time.deltaTime;
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
    }
}
