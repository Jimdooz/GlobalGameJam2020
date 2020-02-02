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

        GameObject light2D = Instantiate(Resources.Load("Prefabs/2D_LIGHT", typeof(GameObject))) as GameObject;
        Light2D observerLight = light2D.GetComponent<Light2D>();
        light2D.transform.parent = transform;
        light2D.transform.position = transform.position;
        light2D.transform.rotation = transform.rotation;

        CopyComponent(observerLight, gameObject);

        observerLight.pointLightInnerAngle = field.viewAngle;
        observerLight.pointLightOuterAngle = field.viewAngle;
        observerLight.pointLightInnerRadius = field.viewRadius;
        observerLight.pointLightOuterRadius = field.viewRadius;

        observerLight.color = color;
        observerLight.shadowIntensity = 1f;
        observerLight.shadowVolumeIntensity = 1f;
        observerLight.intensity = intensity;
    }

    Component CopyComponent(Component original, GameObject destination) {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}
