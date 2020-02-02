using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Agent))]
public class AgentEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Alert"))
        {
            Agent agent = (Agent)target;
            agent.Alert();
        }
    }
}
