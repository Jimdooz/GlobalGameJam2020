using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(Navigation2D))]
public class Navigation2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Navigation2D navigation2d = (Navigation2D)target;
        if (GUILayout.Button("Generate"))
            navigation2d.GenererateNavGrid();
            
    }
}
