using System.Collections.Generic;
using UnityEngine;

public class Navigation2D : MonoBehaviour
{
    public static Navigation2D instance;
    #region Inspector Variables
    public bool draw = true;
    public bool drawUnavailable = false;
    public float precision = 0.1f;
    public float Width;
    public float Height;
    public LayerMask layerMask;
    public Vector2 agentColliderSize;
    public Vector2 agentColliderOffSet;
    #endregion  
    Node[,] nodes;
    float drawNodeSize = 0.05f;
    int w, h;
    #region Unity API
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            Debug.Log("Multiples instances of Navigation2D");
            return;
        }
        instance = this;
        GenererateNavGrid();
    }
    private void OnDrawGizmos()
    {
        if (nodes == null)
        {
            return;
        }
        if (draw)
        {
            foreach (Node n in nodes)
            {
                if (n == null)
                {
                    continue;
                }
                if (n.ConnectionsAvailables() > 0)
                {
                    Gizmos.color = Color.yellow;
                }
                else if (drawUnavailable)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    continue;
                }
                Gizmos.DrawSphere(n.position, drawNodeSize);
                foreach (Connection c in n.connections)
                {
                    if (c.available)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else if (drawUnavailable)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        continue;
                    }
                    Gizmos.DrawLine(c.n1.position, c.n2.position);
                }
            }
        }
    }
    #endregion
    #region Generation
    public void GenererateNavGrid()
    {
        float bx = transform.position.x - Width / 2;
        float by = transform.position.y - Height / 2;
        w = Mathf.RoundToInt(Width * 1 / precision);
        h = Mathf.RoundToInt(Height * 1 / precision);
        nodes = new Node[w, h];
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                float nx = bx + x * precision;
                float ny = by + y * precision;
                if (x % 2 == 0)
                {
                    ny += precision / 2;
                }
                nodes[x, y] = new Node(nx, ny, x, y);
            }
        }
        GenerateConnections();
    }
    void GenerateConnections()
    {
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Node n = nodes[x, y];
                if (y > 0)
                {
                    n.AddConnection(nodes[x, y - 1], layerMask);
                }
                if (x > 1)
                {
                    n.AddConnection(nodes[x - 2, y], layerMask);
                }
                if (x % 2 == 0)
                {
                    if (x > 0)
                    {
                        n.AddConnection(nodes[x - 1, y], layerMask);
                        if (y < h - 1)
                        {
                            n.AddConnection(nodes[x - 1, y + 1], layerMask);
                        }
                    }
                    if (x < w - 1)
                    {
                        n.AddConnection(nodes[x + 1, y], layerMask);
                        if (y < h - 1)
                        {
                            n.AddConnection(nodes[x + 1, y + 1], layerMask);
                        }
                    }

                }
            }
        }
    }
    #endregion 
    #region Navigation API
    public List<Vector2> GetPathToPoint(Vector2 origin, Vector2 target)
    {
        Node nOrigin = GetClosestNode2(origin);
        Node nTarget = GetClosestNode2(target);
        List<Vector2> l = GetPathToNode(nOrigin, nTarget);
        l.RemoveAt(0);
        return l;
    }
    #endregion
    #region Pathfinding
    List<Vector2> GetPathToNode(Node origin, Node target)
    {
        float t = Time.realtimeSinceStartup;
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        origin.cameFrom = null;
        float[,] score = new float[w, h];
        open.Add(origin);
        Node current;
        while (open.Count > 0)
        {
            current = GetClosestNodeFromTarget(open, target);
            if (current == target)
            {
                t = Time.realtimeSinceStartup - t;
                return ReconstructPath(current);
            }
            open.Remove(current);
            foreach (Connection c in current.connections)
            {

                if (!c.available)
                {
                    continue;
                }
                Node n = c.GetOtherNode(current);
                if (closed.Contains(n))
                {
                    continue;
                }
                float tempScore = score[current.x, current.y] + c.distance;
                if (score[n.x, n.y] == 0 || tempScore < score[n.x, n.y])
                {
                    open.Add(n);
                    n.cameFrom = current;
                    score[n.x, n.y] = tempScore;
                }
            }
            closed.Add(current);
        }
        return null;
    }
    List<Vector2> ReconstructPath(Node end)
    {
        List<Vector2> l = new List<Vector2>();
        Node n = end;
        l.Insert(0, n.position);
        while (n.cameFrom != null)
        {
            n = n.cameFrom;
            l.Insert(0, n.position);
        }
        return l;

    }
    Node GetClosestNodeFromTarget(List<Node> l, Node t)
    {
        Node r = null;
        float distance = Mathf.Infinity;
        foreach (Node n in l)
        {
            if (n == null)
            {
                continue;
            }
            float d = Vector2.Distance(n.position, t.position);
            if (d < distance)
            {
                r = n;
                distance = d;
            }
        }
        return r;
    }
    Node GetClosestNode2(Vector2 v)
    {
        Node n = null;
        float d = Mathf.Infinity;
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (!nodes[x, y].IsAvailable())
                {
                    continue;
                }
                float d2 = Vector2.Distance(v, nodes[x, y].position);
                if (d2 < d)
                {
                    d = d2;
                    n = nodes[x, y];
                }
            }
        }
        return n;
    }
    Node GetClosestNode(Vector2 v)
    {
        int x = Mathf.RoundToInt((v.x + Width / 2) / precision);
        int y = Mathf.RoundToInt((v.y + Height / 2) / precision);
        x = x < 0 ? 0 : x;
        y = y < 0 ? 0 : y;
        x = x > w - 1 ? w - 1 : x;
        y = y > w - 1 ? w - 1 : y;
        Node current = nodes[x, y];
        Node closest;
        float distance;
        if (!current.IsAvailable())
        {
            closest = null;
            distance = Mathf.Infinity;
            foreach (Node n in current.Neighbors)
            {
                if (!n.IsAvailable())
                {
                    continue;
                }
                float d = Vector2.Distance(v, n.position);
                if (d < distance)
                {
                    closest = n;
                    distance = d;
                }
            }
            current = closest;
        }
        while (current == null)
        {
            closest = null;
            distance = Mathf.Infinity;
            foreach (Node n in nodes[x, y].GetNeighborsOfNeighbors(0))
            {
                if (!n.IsAvailable())
                {
                    continue;
                }
                float d = Vector2.Distance(v, n.position);
                if (d < distance)
                {
                    closest = n;
                    distance = d;
                }
            }
            current = closest;
        }
        if (current == null)
        {
            Debug.Log("xy : " + x + "," + y);
            Debug.Log("v : " + v.x + "," + v.y);
            Debug.Log("hw : " + h + "," + w);
        }
        return current;
    }
    bool IsTargetAccessible(Vector2 target, Vector2 origin)
    {
        Vector2 dir = (target - origin).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, Vector2.Distance(origin, target), layerMask);
        return hit.collider == null;
    }
    #endregion
}
public class Node
{
    public Node(float nx, float ny, int x, int y)
    {
        position = new Vector2(nx, ny);
        connections = new List<Connection>();
        Neighbors = new List<Node>();
        this.x = x;
        this.y = y;
    }
    public Vector2 position;
    public List<Connection> connections;
    public List<Node> Neighbors;
    public Node cameFrom;
    public int x;
    public int y;
    public int ConnectionsAvailables()
    {
        int i = 0;
        foreach (Connection c in connections)
        {
            i += c.available ? 1 : 0;
        }
        return i;
    }
    public bool IsAvailable()
    {
        return ConnectionsAvailables() > 0;
    }
    public void AddConnection(Node n, LayerMask layerMask)
    {
        Connection c = new Connection(this, n);
        connections.Add(c);
        n.connections.Add(c);
        Neighbors.Add(n);
        n.Neighbors.Add(this);
        c.Raycast(layerMask);
    }
    public List<Node> GetNeighborsAvailable()
    {
        List<Node> l = new List<Node>();
        foreach (Connection c in connections)
        {
            if (c.available)
            {
                l.Add(c.GetOtherNode(this));
            }
        }
        return l;
    }
    public List<Node> GetNeighborsOfNeighbors(int lvl)
    {

        List<Node> l = new List<Node>();
        foreach (Node n in Neighbors)
        {
            l.AddRange(n.Neighbors);
        }

        return l;
    }
    public override string ToString()
    {
        return "Node : " + x + ',' + y;
    }
}
public class Connection
{
    public Node n1;
    public Node n2;
    public bool available;
    public float distance;
    public Connection(Node n1, Node n2)
    {
        this.n1 = n1;
        this.n2 = n2;
        distance = Vector2.Distance(n1.position, n2.position);
        available = true;
    }
    public Node GetOtherNode(Node n)
    {
        Node toret;
        if (n == n1)
        {
            toret = n2;
        }
        else if (n == n2)
        {
            toret = n1;
        }
        else
        {
            toret = null;
        }
        return toret;
    }
    public void Raycast(LayerMask layerMask)
    {
        Vector2 dir = (n2.position - n1.position).normalized;
        RaycastHit2D rhit = Physics2D.Raycast(n1.position, dir, distance, layerMask.value);
        if (rhit.collider != null)
        {
            available = false;
        }
    }
}
