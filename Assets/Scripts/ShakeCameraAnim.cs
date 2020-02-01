using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCameraAnim : MonoBehaviour
{

    public Vector2 Amplitude;
    public bool startShake = false;
    private Vector3 startPos;

    private void Awake()
    {
        startPos = Camera.main.transform.localPosition;    
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (startShake == true)
        {
            StartCoroutine(shakeScreenSimple());
        }
        else
        {
            StopCoroutine(shakeScreenSimple());
            Camera.main.transform.localPosition = startPos;
        }
    }

    IEnumerator shakeScreenSimple()
    {
        Camera.main.transform.localPosition = new Vector3(startPos.x + Random.Range(-Amplitude.x, Amplitude.x),
                                                          startPos.y + Random.Range(-Amplitude.y, Amplitude.y),
                                                          startPos.z);
        yield return new WaitForEndOfFrame();

    }
}
