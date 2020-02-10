using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public enum states { Idle, Warning, Angry };
    [HideInInspector]
    public states state = states.Idle;

    float speed = 1f;
    public float speedWalk = 80f;
    public float speedRun = 160f;
    public float nextWayPointDistance = 3f;

    public Transform target;
    public FieldOfView2 view;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = true;

    Vector3 positionToReach;
    public SpriteRenderer spriteRenderer;

    public GameObject surpriseVFX;

    //Next View
    float nextView = 0f;

    float timerRandView = 3f;
    float currentTimerRandView = 0f;
    //Change position
    float timerRandPosition = 5f;
    float currentTimerRandPosition = 0f;

    bool NeedNewPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.2f);
        positionToReach = transform.position;
        currentTimerRandPosition = timerRandPosition;
    }

    void UpdatePath(){
        if(seeker.IsDone() && NeedNewPath || state == states.Angry ) {
            seeker.StartPath(rb.position, positionToReach, OnPathComplete);
            NeedNewPath = false;
        }
    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWayPoint = 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(state == states.Idle) RunIDLE();
            rb.velocity = new Vector2(0,0);

        //Debug.Log(view.visibleTargets.Count);
        if(view.visibleTargets.Count > 0){
            if(state != states.Angry){
                GameObject fx = Instantiate(surpriseVFX);
                fx.transform.parent = gameObject.transform;
                fx.transform.localPosition = new Vector3(0f,0f,0f);
            }
            if(positionToReach != view.visibleTargets[0].position) MusicManager.Play("Course Poursuite", 2f);
            positionToReach = view.visibleTargets[0].position;
            //Look at 2D
            view.transform.up = view.visibleTargets[0].position - view.transform.position;
            speed = speedRun;
            state = states.Angry;
            view.viewMinRadius = 3f;
        }else{
            state = states.Idle;
            if(reachedEndOfPath) view.viewMinRadius = 1f;
        }

        if(path == null) return;

        if(currentWayPoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            rb.velocity = new Vector2(0,0);
        } else reachedEndOfPath = false;

        if(!reachedEndOfPath){
            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.velocity = force;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

            if(distance < nextWayPointDistance){
                currentWayPoint++;
            }
        }

        if(rb.velocity != new Vector2(0,0)){
            animator.SetBool("moving", true);
            if(rb.velocity.x > 0){
                spriteRenderer.flipX = false;
                //transform.localScale = new Vector3(1,1,1);
            }else if(rb.velocity.x < 0){
                spriteRenderer.flipX = true;
                //transform.localScale = new Vector3(-1,1,1);
            }
        }else{
            animator.SetBool("moving", false);
        }
        if(state == states.Idle){
            var desiredRotQ = Quaternion.Euler(view.transform.eulerAngles.x, view.transform.eulerAngles.y, nextView);
            view.transform.rotation = Quaternion.Lerp(view.transform.rotation, desiredRotQ, Time.deltaTime * 1);
        }

        Debug.Log(rb.velocity);
    }

    void PlayWalk(){
        MusicManager.Effect("Footstep Terre", rb.position, 0.2f);
    }

    void RunIDLE(){
        currentTimerRandView += Time.deltaTime;
        currentTimerRandPosition += Time.deltaTime;
        if(currentTimerRandView >= timerRandView){
            currentTimerRandView = 0f;
            nextView = Random.Range(0f,360f);
        }
        if(currentTimerRandPosition >= timerRandPosition && reachedEndOfPath){
            currentTimerRandPosition = 0;
            positionToReach = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f),0);
            speed = speedWalk;
            NeedNewPath = true;
            timerRandPosition = 3f + Random.Range(0f, 4f);
        }
    }
}
