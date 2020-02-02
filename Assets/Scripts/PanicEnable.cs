using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicEnable : MonoBehaviour
{

    private followTamere followScript;
    private PlayerController playerScript;
    private GameObject render;

    void Start()
    {
        followScript = GetComponent<followTamere>();
        playerScript = followScript.target.gameObject.GetComponent<PlayerController>();

        render = transform.GetChild(0).gameObject;
        render.SetActive(false);
    }

    void Update()
    {
        if (playerScript.movementStatus == PlayerController.MovementStates.running && !render.activeSelf)
        {
            render.SetActive(true);
        }
        else if (playerScript.movementStatus != PlayerController.MovementStates.running && render.activeSelf)
        {
            render.SetActive(false);
        }
    }
}
