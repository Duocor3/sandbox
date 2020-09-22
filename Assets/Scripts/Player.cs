using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask ground;

    public float speed;
    public float jumpHeight;

    private Rigidbody2D rb;

    private float inputX;
    private float inputY;

    // Start is called before the first frame update
    void Start()
    {
        // sets the rigidbody to whatever the script is attached to
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // gets the axis/controls for the player
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Jump");
    }

    private void FixedUpdate()
    {
        // sets the jump velocity if the player is touching the ground
        if (AxisInUse(inputY) && IsGrounded())
        {
            rb.velocity = Vector2.up * jumpHeight;
        }

        // sets the horizontal velocity
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
    }

    // checks if axis is being currently pressed
    private bool AxisInUse(float axis)
    {
        return axis != 0 ? true : false;
    }
    
    // creates a raycast to check if the ground is under the player
    private bool IsGrounded()
    {
        float bounds = (rb.GetComponent<Renderer>().bounds.size.y) / 2;

        return Physics2D.BoxCast(transform.position, new Vector2(bounds, bounds-0.25f), 0f, Vector2.down, bounds, ground);
    }
}