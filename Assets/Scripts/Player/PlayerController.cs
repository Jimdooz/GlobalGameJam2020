using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs inputScript;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField]
    private float walkSpeed = 4.0f;
    [SerializeField]
    private float runSpeed = 8.0f;

    private Vector2 movement;
    private Vector2 lastDirection = Vector2.right;
    private bool facingRight = true;

    public enum MovementStates { none, walking, running};
    public MovementStates movementStatus = MovementStates.none;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        inputScript = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        HandleDirection();

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (movementStatus != MovementStates.running)
            {
                movementStatus = MovementStates.running;
            }
            else
            {
                movementStatus = MovementStates.walking;
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        movement = inputScript.normalizedDirection;
        UpdateMovementStatus();
        HandleAnimations();
        if (!(movementStatus == MovementStates.running))
        {
            rb.MovePosition(rb.position + movement * walkSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if (movement != Vector2.zero)
            {
                lastDirection = movement;
            }
            else
            {
                movement = lastDirection;
            }
            rb.MovePosition(rb.position + movement * runSpeed * Time.fixedDeltaTime);
        }
    }

    void UpdateMovementStatus()
    {
        if (!(movementStatus == MovementStates.running))
        {
            if (movement != Vector2.zero)
            {
                movementStatus = MovementStates.walking;
            }
            else
            {
                movementStatus = MovementStates.none;
            }
        }
    }

    void HandleAnimations()
    {
        if (movementStatus == MovementStates.running)
        {
            if (animator.GetBool("walking"))
            {
                animator.SetBool("walking", false);
                animator.SetBool("running", true);
            }
        }
        else
        {
            if (animator.GetBool("running"))
            {
                animator.SetBool("running", false);
            }
            if (movementStatus == MovementStates.walking && !animator.GetBool("walking"))
            {
                animator.SetBool("walking", true);
            }
            else if (!(movementStatus == MovementStates.walking) && animator.GetBool("walking"))
            {
                animator.SetBool("walking", false);
            }
        }
    }

    void HandleDirection()
    {
        if (movement.x > 0 && !facingRight)
        {
            FlipPlayer();
        }
        else if (movement.x < 0 && facingRight)
        {
            FlipPlayer();
        }
    }

    void FlipPlayer()
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
