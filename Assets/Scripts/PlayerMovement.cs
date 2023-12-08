using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private ParticleSystem walkParticle;
    private float horizontalMovement;
    private float jumpForce = 6;
    private float lastTimeGrounded;
    private float lastTimeJumpPressed;
    private bool grounded = true;
    private bool wallJump;

    private Rigidbody2D rb;

    private void Start()
    {
        walkParticle.Stop();
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
            if(horizontalMovement >= -_speed)
            {
                horizontalMovement -= 40 * Time.deltaTime;
            }
            else
            {
                horizontalMovement = -_speed;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if(horizontalMovement <= _speed)
            {
                horizontalMovement += 40 * Time.deltaTime;
            }
            else
            {
                horizontalMovement = _speed;
            }
        }
        else
        {
            if(horizontalMovement != 0)
            {
                if (horizontalMovement < 0)
                {
                    horizontalMovement += 25 * Time.deltaTime;
                }
                else
                {
                    horizontalMovement -= 25 * Time.deltaTime;
                }
            }
        }


        if (Input.GetButtonDown("Jump") && lastTimeJumpPressed - lastTimeGrounded < 0.2f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            walkParticle.Play();
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }


        if (rb.velocity.y < 1 && !grounded)
        {

            if(rb.gravityScale <= 4f)
            {
                rb.gravityScale += 20 * Time.deltaTime;
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
            wallJump = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
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
            wallJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            grounded = false;
        }
    }

}
