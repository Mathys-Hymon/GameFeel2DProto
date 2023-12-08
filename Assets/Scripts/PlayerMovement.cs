using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    private float horizontalMovement;
    private float jumpForce = 8;
    private float lastTimeGrounded;
    private float lastTimeJumpPressed;
    private bool grounded = true;
    private bool ctrlDown;

    private Rigidbody2D rb;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if(grounded)
        {
            lastTimeGrounded = Time.time;
        }
        if(Input.GetButtonDown("Jump"))
        {
            lastTimeJumpPressed = Time.time;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontalMovement = -_speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalMovement = _speed;
        }
        else
        {
            if(horizontalMovement != 0)
            {
                horizontalMovement *= 0.3f;
            }
        }


        if (Input.GetButtonDown("Jump") && lastTimeJumpPressed - lastTimeGrounded < 0.2f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }


        if (rb.velocity.y < 0 && !grounded)
        {

            if(rb.gravityScale <= 2.5f)
            {
                rb.gravityScale += Time.deltaTime*7;
            }
        }
        else rb.gravityScale = 1f;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalMovement, rb.velocity.y, 0);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            grounded = false;
        }
    }

}
