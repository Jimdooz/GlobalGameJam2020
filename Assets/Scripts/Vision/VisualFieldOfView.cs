using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(FieldOfView))]
public class VisualFieldOfView : MonoBehaviour
{
    public Color color;
    [Range(0f,1f)]
    public float intensity = 1f;

    FieldOfView field;

    // Start is called before the first frame update
    void Start() {
        field = GetComponent<FieldOfView>();

        Light2D observerLight = gameObject.AddComponent(typeof(Light2D)) as Light2D;

        observerLight.pointLightInnerAngle = field.viewAngle;
        observerLight.pointLightOuterAngle = field.viewAngle;
        observerLight.pointLightInnerRadius = field.viewRadius;
        observerLight.pointLightOuterRadius = field.viewRadius;

        observerLight.color = color;
        observerLight.shadowIntensity = 1f;
        observerLight.shadowVolumeIntensity = 1f;
        observerLight.volumeOpacity = intensity;
    }
}
