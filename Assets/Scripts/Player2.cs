using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public LayerMask ground;

    public float speed;
    public float jumpForce;
    public float jumpDuration;
    public float wallJumpDuration;

    private Rigidbody2D rb;

    private float jumpTime;
    private float inputX;
    private float boundsX;
    private float boundsY;

    private bool inputY;
    private bool isJumping;
    private bool isWallClimbing;

    // Start is called before the first frame update
    void Start()
    {
        // sets the rigidbody to whatever the script is attached to
        rb = GetComponent<Rigidbody2D>();

        // sets the width and height of the player
        boundsX = rb.GetComponent<Renderer>().bounds.size.x;
        boundsY = rb.GetComponent<Renderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        // gets the axis/controls for the player
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetKey(KeyCode.Space);

        // controls what type of jump the player is making
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // changes jump mode from regular jumping to wallclimbing
            if (IsGrounded()) {
                isJumping = true;
                isWallClimbing = false;

            } else if (IsTouchingWall() != 0 && inputX != 0)
            {
                
                if (inputX == IsTouchingWall())
                {
                    jumpTime = wallJumpDuration;
                    isJumping = false;
                    isWallClimbing = true;
                } else
                {
                    // this causes wallbouncing
                    isJumping = true;
                    isWallClimbing = false;
                    Debug.Log("wallbounce " + inputX + inputY);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || jumpTime <= 0)
        {
            isJumping = false;
            isWallClimbing = false;
        }
    }

    private void FixedUpdate()
    {
        // increases Y velocity as long as the player is jumping
        if (isJumping || isWallClimbing)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpTime -= Time.deltaTime;
        }

        // sets the horizontal velocity
        if (isWallClimbing)
        {
            // for wall climbing
            rb.velocity = new Vector2(inputX * speed * -1, jumpForce);
        } else
        {
            // for regular movement
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        }
    }

    // creates a raycast to check if the ground is under the player
    private bool IsGrounded()
    {
        // creates the raycast
        RaycastHit2D boxCast = BoxCast(new Vector2(transform.position.x, transform.position.y - boundsY/2), new Vector2(boundsX, boundsY/2), 0f, Vector2.down, 0f, ground);

        // resets the jumptime
        jumpTime = jumpDuration;

        // draws the raycast
        return boxCast;
    }

    // creates a raycast to check if the player is touching a potential wall
    private float IsTouchingWall()
    {
        // creates the left raycast
        RaycastHit2D boxCastLeft = BoxCast(new Vector2(transform.position.x - boundsX/2, transform.position.y), new Vector2(boundsX/1.7f, boundsY), 0f, Vector2.down, 0f, ground);

        // creates the right raycast
        RaycastHit2D boxCastRight = BoxCast(new Vector2(transform.position.x + boundsX / 2, transform.position.y), new Vector2(boundsX/1.7f, boundsY), 0f, Vector2.down, 0f, ground);

        // various cases to figure out which walls the player is touching
        if (boxCastLeft && boxCastRight || !boxCastLeft && !boxCastRight)
        {
            return 0;
        } else if (boxCastLeft)
        {
            return -1;
        } else if (boxCastRight)
        {
            return 1;
        } else
        {
            Debug.Log("IsTouchingWall() broke");
            return 0;
        }
    }

    // quick little method to create and draw a boxcast
    private RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
    {
        // creates the raycast
        RaycastHit2D boxCast = Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask);

        // draws the raycast
        BoxCastDrawer.Draw(boxCast, origin, size, angle, direction, distance);

        return boxCast;
    }
}