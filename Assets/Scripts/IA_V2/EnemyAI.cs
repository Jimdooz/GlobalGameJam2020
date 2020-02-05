using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public enum states { Idle, Warning, Attention };

    public float speed = 1f;
    public float nextWayPointDistance = 3f;

    public Transform target;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.25f);
    }

    void UpdatePath(){
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
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
        if(path == null) return;

        if(currentWayPoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            rb.velocity = new Vector2(0,0);
            return;
        } else reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if(distance < nextWayPointDistance){
            currentWayPoint++;
        }

        if(rb.velocity != new Vector2(0,0)){
            animator.SetBool("moving", true);
            if(rb.velocity.x > 0){
                transform.localScale = new Vector3(1,1,1);
            }else if(rb.velocity.x < 0){
                transform.localScale = new Vector3(-1,1,1);
            }
        }else{
            animator.SetBool("moving", false);
        }
    }

    void PlayWalk(){
        MusicManager.Effect("Footstep Terre", rb.position, 0.2f);
    }

    void GoToPosition(Vector3 position){

    }
}
