using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [HideInInspector]
    public Vector2 normalizedDirection;

    private Vector2 directionInput;

    [SerializeField]
    private float moveMargin = 0.2f;


    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        directionInput = Vector2.zero;

        if (x + moveMargin < 0)
        {
            directionInput += Vector2.left;
        }
        if (x - moveMargin > 0)
        {
            directionInput += Vector2.right;
        }

        if (y + moveMargin < 0)
        {
            directionInput += Vector2.down;
        }
        if (y - moveMargin > 0)
        {
            directionInput += Vector2.up;
        }

        if (directionInput.magnitude > 1)
        {
            Debug.Log("DIAGONAL");
            normalizedDirection = directionInput / directionInput.magnitude;
        }
        else
        {
            normalizedDirection = directionInput;
        }
    }
}
