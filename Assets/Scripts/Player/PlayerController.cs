using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public FieldOfView2 vision;

    public static PlayerController Instance;

    public RepairObjectsHandler handler;
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

    public float speedSoundWalk = 0.3f;
    public float speedSoundRun = 0.2f;
    float timeSound = 0f;

    float timerCalmeMusic = 6f;
    float timerCalmeMusicNow = 0f;

    private bool canMove = true;

    public Transform initialPos;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        inputScript = GetComponent<PlayerInputs>();
    }

    private void Start()
    {
        if (initialPos != null)
        {
            transform.position = initialPos.position;
        }
        MusicManager.Play("Calme Tempete", 1f);
    }

    private void Update()
    {
        HandleDirection();
        HandleVision();

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    if (movementStatus != MovementStates.running)
        //    {
        //        movementStatus = MovementStates.running;
        //    }
        //    else
        //    {
        //        movementStatus = MovementStates.walking;
        //    }
        //}
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    void HandleVision(){
        if(vision.visibleTargets.Count > 0){
            bool playWarningSound = true;
            for(int i = 0; i < vision.visibleTargets.Count; i++){
                if(vision.visibleTargets[i].gameObject.GetComponent<EnemyAI>().state == EnemyAI.states.Angry){
                    playWarningSound = false;
                    movementStatus = PlayerController.MovementStates.running;
                }
            }
            if(playWarningSound){
                timerCalmeMusicNow = 0;
                MusicManager.Play("Suspens", 5f);
            }
        }else{
            timerCalmeMusicNow += Time.deltaTime;
            if(timerCalmeMusicNow > timerCalmeMusic){
                MusicManager.Play("Calme Tempete", 5f);
                    movementStatus = PlayerController.MovementStates.walking;
            }
        }
    }

    void Move()
    {
        movement = inputScript.normalizedDirection;
        UpdateMovementStatus();
        HandleAnimations();
        timeSound += Time.deltaTime;
        if (!(movementStatus == MovementStates.running))
        {
            rb.MovePosition(rb.position + movement * walkSpeed * Time.fixedDeltaTime);
            if (movementStatus == MovementStates.none)
            {
                timeSound = 0f;
            }
            else
            {
                if (timeSound >= speedSoundWalk)
                {
                    timeSound = 0f;
                    MusicManager.Effect("Footstep Terre", 0.5f);
                }
            }
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
            rb.MovePosition(rb.position + movement * (runSpeed) * Time.fixedDeltaTime);
            if (timeSound >= speedSoundRun)
            {
                timeSound = 0f;
                MusicManager.Effect("Footstep Terre", 0.5f);
            }
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
            else
            {
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
