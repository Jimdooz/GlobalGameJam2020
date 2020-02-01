using System.Collections.Generic;
using UnityEngine;
public class Agent : MonoBehaviour
{
    #region Inspector Variables
    public string tribut = "alpha";
    public float rangeAlert = 50f;
    public bool startAsWatch;
    [Header("PathFinding")]
    public bool drawPath;
    public float marginDistance = 0.1f; // margin used to know if agent is arrived at target poition
    [Header("Idle")]
    public float rangeIdleZone = 30f;
    public float minWaitTimer;
    public float maxWaitTimer;
    public float idleSpeed;
    [Header("Search")]
    public float searchSpeed;
    [Header("Follow")]
    public float followSpeed;
    
    #endregion

    #region private vars
    Vector3 startPosition;
    bool startPosDefined = false;

    Rigidbody2D rb2d;

    Vector3 targetPosition;
    float speed;
    bool alert;
    float waitTimer;


    List<Vector2> path=new List<Vector2>();
    int destinationIndex;
    Vector3 destination;
    bool atDestination;
    bool completedPath;
    #endregion
    #region Agent API
    public void Alert()
    {
        alert = true;
    }
    #endregion
    #region Unity API
    void Awake()
    {
        startPosition = transform.position;
        startPosDefined = true;
        completedPath = true;
        speed = idleSpeed;
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        if (startAsWatch)
        {
            state = Watch;
        }
        else
        {
            state = Idle;
        }
    }

    void Update()
    {
        state?.Invoke();
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
    #endregion

    #region States
    delegate void State();
    State state;
    void Idle()
    {
        if (alert == true)
        {
            ChangeToState(Search,searchSpeed);
            return;
        }
        if (atDestination)
        {
            if (destinationIndex>=path.Count-1)
            {
                path = Navigation2D.instance.GetPathToPoint(transform.position, GetRandomPointInIdleZone());
                while (path.Count==0)
                {
                    path = Navigation2D.instance.GetPathToPoint(transform.position, GetRandomPointInIdleZone());
                }
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
        if (waitTimer<0)
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
        if (alert == true)
        {
            ChangeToState(Search,searchSpeed);
            return;
        }
    }
    void Search()
    {
        if (alert == false)
        {
            ChangeToState(Idle,idleSpeed);
            return;
        }
        else if (PlayerIsInVision())
        {
            ChangeToState(Follow,followSpeed);
            return;
        }
    }
    void Follow()
    {
        if (alert == false)
        {
            ChangeToState(Idle,idleSpeed);
            return;
        }
        else if (!PlayerIsInVision())
        {
            ChangeToState(Follow,followSpeed);
            return;
        }

    }
    void ChangeToState(State newState,float newSpeed)
    {
        speed = newSpeed;
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
        Vector3 dir = targetPos-agentPos;
        rb2d.velocity = dir.normalized * speed;
    }
    Vector3 GetRandomPointInIdleZone()
    {
        float a = Random.value * 2 * Mathf.PI;
        float r = rangeIdleZone * Mathf.Sqrt(Random.value);
        return new Vector3(r*Mathf.Cos(a),r*Mathf.Sin(a),transform.position.z);
    }
    void StopAgent()
    {
        rb2d.velocity = new Vector2(0,0);
    }
    #endregion

    #region Vision
    bool PlayerIsInVision()
    {
        return false;
    }
    Vector3 GetPlayerPos()
    {
        return new Vector3();
    }
    #endregion


}
