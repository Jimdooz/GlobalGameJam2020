using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveConstant : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.5f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0);
    }
}
