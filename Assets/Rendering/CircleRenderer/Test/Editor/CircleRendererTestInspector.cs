using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CircleRendererTest))]
public class CircleRendererTestInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CircleRendererTest myScript = (CircleRendererTest)target;
        if (GUILayout.Button("Run"))
        {
            myScript.Run();
        }
    }
}
