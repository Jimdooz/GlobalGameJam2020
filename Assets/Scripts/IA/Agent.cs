using System.Collections.Generic;
using UnityEngine;
public class Agent : MonoBehaviour
{
    #region Inspector Variables
    public string debugState;
    public bool startAsWatch;
    public LayerMask obstacleLayerMask;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    [Header("Vision")]
    public float fovRotateSpeed = 5f;
    public float rangeAlert = 50f;
    public LayerMask visionLayerMask;
    [Header("PathFinding")]
    public bool drawPath;
    public float marginDistance = 0.1f; // margin used to know if agent is arrived at target poition
    [Header("Idle")]
    public float rangeIdleZone = 30f;
    public float minWaitTimer;
    public float maxWaitTimer;
    [Header("Search")]
    public float distanceArrived; //distance from ally to know he arrived and need to look arround
    [Header("Follow")]
    public float maxFollowTimer; //time before the agent stop to follow player if it isn't in vision
    [Header("LookArround")]
    public float minLookTimer = 0.1f;
    public float maxLookTimer = 0.3f;
    public float toIdleTimerMax = 3f;


    #endregion

    #region private vars
    Vector3 startPosition;
    bool startPosDefined = false;

    Rigidbody2D rb2d;
    Animator animator;
    FieldOfView fov;
    SpriteRenderer spriteRenderer;

    Vector3 targetPosition;
    float speed;
    [HideInInspector]
    public bool alert;
    float waitTimer;


    List<Vector2> path = new List<Vector2>();
    int destinationIndex;
    Vector3 destination;
    bool atDestination;
    bool completedPath;

    Vector3 playerLastPosition;
    Agent alerter;
    Agent tempAlerter;

    Vector3 lookArroundPosition;

    float notInVisionTimer;
    float lookArroundTimer;
    float toIdleTimer;

    bool angry = false;
    #endregion
    #region Agent Alert
    void BecomeAlert(Agent alerter)
    {
        if (alert)
        {
            return;
        }
        angry = true;
        animator.SetTrigger("alert");
        alert = true;
        if (this == alerter)
        {
            Debug.Log("yo");
            return;
        }
        this.alerter = alerter;
    }
    public void Alert()
    {
        Debug.Log("y");
        BecomeAlert(this);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rangeAlert, visionLayerMask);
        foreach (Collider2D c in colliders)
        {
            Agent a = c.GetComponent<Agent>();
            a?.BecomeAlert(this);
        }
    }
    bool AlerterIsAlert()
    {
        if (alerter == null)
        {
            Debug.Log("no alerter");
            return false;
        }
        return alerter.alert;
    }
    bool SomeOneIsAlert()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rangeAlert, visionLayerMask);
        foreach (Collider2D c in colliders)
        {
            Agent a = c.GetComponent<Agent>();
            if (a != null)
            {
                if (a.alert)
                {
                    tempAlerter = a;
                    return true;
                }
            }
        }
        return AlerterIsAlert();
    }
    #endregion
    #region Unity API
    void Awake()
    {
        startPosition = transform.position;
        startPosDefined = true;
        completedPath = true;
        speed = walkSpeed;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        fov = GetComponentInChildren<FieldOfView>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void Start()
    {
        if (startAsWatch)
        {
            state = Watch;
            debugState = "Watch";

        }
        else
        {
            state = Idle;
            debugState = "Idle";

        }
    }

    void Update()
    {
        state?.Invoke();
        UpdateTimers();
        UpdateAnimator();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAlert);
        Gizmos.color = Color.yellow;
        if (startPosDefined)
        {
            Gizmos.DrawWireSphere(startPosition, rangeIdleZone);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, rangeIdleZone);
        }

        if (drawPath && path != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController p = collision.GetComponent<PlayerController>();
        // @TODO call player get caught
    }
    #endregion

    #region States
    delegate void State();
    State state;
    void Idle()
    {
        if (PlayerIsInVision() && PlayerController.Instance.handler.pickedItem)
        {
            ChangeToState(Follow);
            return;
        }
        if (alert)
        {
            ChangeToState(Search);
            return;
        }
        if (angry && PlayerIsInVision())
        {
            ChangeToState(Follow);
            return;
        }
        if (atDestination)
        {
            if (destinationIndex >= path.Count - 1)
            {
                GetNewPath(GetRandomPointInIdleZone());
                StopAgent();
                waitTimer = Random.Range(minWaitTimer, maxWaitTimer);
                destinationIndex = 0;
            }
            else
            {
                destinationIndex += 1;
            }
            atDestination = false;
            destination = path[destinationIndex];
        }
        if (waitTimer < 0)
        {
            MoveToPosition(destination);
        }
        else
        {
            waitTimer -= Time.deltaTime;
        }
    }
    void Watch()
    {
        if (PlayerIsInVision() && PlayerController.Instance.handler.pickedItem)
        {
            ChangeToState(Follow);
            return;
        }
        if (alert)
        {
            ChangeToState(Search);
            return;
        }
    }
    void Search()
    {
        if (PlayerIsInVision())
        {
            ChangeToState(Follow);
            return;
        }
        else if (!SomeOneIsAlert())
        {
            ChangeToState(Idle);
            return;
        }
        else if (alerter != null && !ArrivedToTarget(alerter.transform.position))
        {
            MoveToTarget(alerter.transform.position);
        }
        else if (alerter == null || !alerter.alert)
        {
            alerter = GetAlerterAlly();
            if (alerter==null)
            {
                ChangeToState(LookArround);
                return;
            }
            else
            {
                MoveToTarget(alerter.transform.position);
            }
        }
        else
        {
            ChangeToState(LookArround);
            return;
        }
    }
    void Follow()
    {
        if (!alert)
        {
            ChangeToState(Idle);
            return;
        }
        else if (!PlayerIsInVision() && notInVisionTimer > maxFollowTimer)
        {
            ChangeToState(Search);
            alert = false;
            return;
        }
        MoveToTarget(GetPlayerPos());
    }
    void LookArround()
    {

        if (PlayerIsInVision() && PlayerController.Instance.handler.pickedItem)
        {
            ChangeToState(Follow);
            return;
        }else if (PlayerIsInVision())
        {
            ChangeToState(Follow);
            return;
        }
        else if (alert)
        {
            ChangeToState(Search);
            return;
        }
        else if (toIdleTimer >= toIdleTimerMax)
        {
            ChangeToState(Idle);
            return;
        }
        if (atDestination)
        {
            if (destinationIndex >= path.Count - 1)
            {
                GetNewPath(GetRandomPointInLookZone());
                StopAgent();
                waitTimer = Random.Range(minLookTimer, maxLookTimer);
                destinationIndex = 0;
            }
            else
            {
                destinationIndex += 1;
            }
            atDestination = false;
            destination = path[destinationIndex];
        }
        if (waitTimer < 0)
        {
            MoveToPosition(destination);
        }
        else
        {
            waitTimer -= Time.deltaTime;
        }
        toIdleTimer += Time.deltaTime;
    }
    void ChangeToState(State newState)
    {
        atDestination = true;
        path.Clear();
        destinationIndex = 0;
        if (newState == Search)
        {
            animator.SetBool("run", true);
            speed = runSpeed;
            debugState = "Search";

        }
        else if (newState == Follow)
        {
            Alert();
            animator.SetBool("run", true);
            speed = runSpeed;
            debugState = "Follow";
        }
        else if (newState == LookArround)
        {
            alert = false;
            alerter = null;
            animator.SetBool("run", true);
            toIdleTimer = 0;
            speed = runSpeed;
            lookArroundPosition = transform.position;
            debugState = "LookArround";
        }
        else if (newState == Idle)
        {
            alert = false;
            alerter = null;
            animator.SetBool("run", false);
            speed = walkSpeed;
            debugState = "Idle";
        }
        state = newState;
        state?.Invoke();
    }
    #endregion

    #region Move
    void MoveToPosition(Vector3 targetPos)
    {
        Vector3 agentPos = transform.position;
        float distance = Vector2.Distance(agentPos, targetPos);
        if (distance < marginDistance)
        {
            atDestination = true;
        }
        Vector2 dir = targetPos - agentPos;
        dir = dir.normalized;
        rb2d.velocity = dir.normalized * speed;
        SetPlayerDir(dir);
    }
    bool ArrivedToTarget(Vector3 t)
    {
        if (Vector2.Distance(transform.position, t) < distanceArrived)
        {
            return true;
        }
        return false;
    }
    void MoveToTarget(Vector3 t)
    {
        if (TargetIsAccessible(t, transform.position))
        {
            Vector2 dir = (t - transform.position).normalized;
            SetPlayerDir(dir);
        }
        else
        {
            GetNewPath(t);
            if (path == null || path.Count < 0)
            {
                path.Add(t);
            }
            Vector2 dir = (path[0] - new Vector2(transform.position.x, transform.position.y)).normalized;
            SetPlayerDir(dir);
        }
    }
    void SetPlayerDir(Vector2 dir)
    {
        rb2d.velocity = dir * speed;
        RotateFOV(dir);
        FaceToDir();
    }
    bool TargetIsAccessible(Vector2 target, Vector2 origin)
    {
        Vector2 dir = (target - origin).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, Vector2.Distance(origin, target), obstacleLayerMask);
        return hit.collider == null;
    }
    void FaceToDir()
    {
        if (rb2d.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb2d.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    void RotateFOV(Vector3 dir)
    {
        Vector3 v = new Vector3();
        float angle = Vector2.SignedAngle(Vector2.up, dir);
        if (fov.transform.eulerAngles.z == angle)
        {
            return;
        }
        else if (Mathf.Abs(fov.transform.eulerAngles.z - angle) <= fovRotateSpeed * Time.deltaTime)
        {
            v.z = angle;
        }
        else if (fov.transform.eulerAngles.z > angle)
        {
            v.z = fov.transform.eulerAngles.z - fovRotateSpeed * Time.deltaTime;
        }
        else
        {
            v.z = fov.transform.eulerAngles.z + fovRotateSpeed * Time.deltaTime;
        }
        fov.transform.eulerAngles = v;
    }
    Vector3 GetRandomPointInIdleZone()
    {
        float a = Random.value * 2 * Mathf.PI;
        float r = rangeIdleZone * Mathf.Sqrt(Random.value);
        return new Vector3(r * Mathf.Cos(a) + startPosition.x, r * Mathf.Sin(a) + startPosition.y, transform.position.z);
    }
    Vector3 GetRandomPointInLookZone()
    {

        float a = Random.value * 2 * Mathf.PI;
        float r = rangeIdleZone * Mathf.Sqrt(Random.value);
        return new Vector3(r * Mathf.Cos(a) + lookArroundPosition.x, r * Mathf.Sin(a) + lookArroundPosition.y, transform.position.z);
    }
    void GetNewPath(Vector3 targetPos)
    {
        path = Navigation2D.instance.GetPathToPoint(transform.position, targetPos);
        while (path.Count == 0)
        {
            path = Navigation2D.instance.GetPathToPoint(transform.position, targetPos);
        }
    }
    void StopAgent()
    {
        rb2d.velocity = new Vector2(0, 0);
    }

    #endregion
    #region Vision
    bool PlayerIsInVision()
    {
        return fov.player != null;
    }
    Vector3 GetPlayerPos()
    {
        if (fov.player == null)
        {
            Debug.Log("error y");
            return playerLastPosition;
        }
        playerLastPosition = fov.player.position;
        return fov.player.position;
    }
    #endregion

    void UpdateAnimator()
    {
        animator.SetBool("moving", rb2d.velocity.sqrMagnitude > 1);
    }
    void UpdateTimers()
    {
        notInVisionTimer += Time.deltaTime;
    }
    Agent GetAlerterAlly()
    {
        List<Agent> l= fov.visibleAgents.FindAll(c => c.alert == true);
        if (l.Count ==0)
        {
            return null;
        }
        Agent a = l[0];// Find closer to player
        if (a==this)
        {
            return null;
        }
        else
        {
            return a;
        }
    }

}
