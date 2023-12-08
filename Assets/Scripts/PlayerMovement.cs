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
    private bool haveJump;
    private bool wallJump;
    private float wallJumpMultiplier;

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
                wallJumpMultiplier = -1;
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
                wallJumpMultiplier = 1;
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
                if (horizontalMovement <= 0.05 && horizontalMovement >= -0.05)
                {
                    horizontalMovement = 0;
                }

                if (horizontalMovement < 0)
                {
                    horizontalMovement += 25 * Time.deltaTime;
                }
                else if(horizontalMovement > 0)
                {
                    horizontalMovement -= 25 * Time.deltaTime;
                }
            }
        }


        if (Input.GetButtonDown("Jump") && lastTimeJumpPressed - lastTimeGrounded < 0.2f)
        {
            if(!haveJump && !wallJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                walkParticle.Play();
                haveJump = true;
            }

            else if(wallJump)
            {
                horizontalMovement = 0;
                rb.velocity = new Vector2(15 * -wallJumpMultiplier, jumpForce*1.5f);
            }
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
        if (!wallJump)
        {
            rb.velocity = new Vector3(horizontalMovement, rb.velocity.y, 0);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            wallJump = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        haveJump = false;
        wallJump = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            grounded = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        haveJump = false;
        grounded = true;
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
