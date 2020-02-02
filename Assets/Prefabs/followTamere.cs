using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTamere : MonoBehaviour
{

    public Transform target;

    public Vector3 decalage = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + decalage;
    }
}
