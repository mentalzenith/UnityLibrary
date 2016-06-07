using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Test myScript = (Test)target;
        if (GUILayout.Button("Run"))
        {
            myScript.Run();
        }
    }
}