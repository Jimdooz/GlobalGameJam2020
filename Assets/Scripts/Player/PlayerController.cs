using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs inputScript;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5.0f;
    private Vector2 movement;

    private bool facingRight = true;
    private bool walking;

    private Rigidbody2D rb;

    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        inputScript = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        HandleDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        movement = inputScript.normalizedDirection;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleDirection()
    {
        if (movement.y < 0)
        {
            animator.SetTrigger("front");
            walking = true;
        }
        else if (movement.y > 0)
        {
            animator.SetTrigger("back");
            walking = true;
        }

        else
        {
            if (movement.x < 0 || movement.x > 0)
            {
                flipPlayer();
                animator.SetTrigger("side");
                walking = true;
            }

            else
            {
                walking = false;
            }
        }

        if (walking && !animator.GetBool("walking"))
        {
            animator.SetBool("walking", true);
        }
        else if (!walking && animator.GetBool("walking"))
        {
            animator.SetBool("walking", false);
        }
    }

    void flipPlayer()
    {
        if (facingRight && movement.x <= 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            facingRight = !facingRight;
        }
        else if (!facingRight && movement.x >= 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            facingRight = !facingRight;
        }
    }
}
