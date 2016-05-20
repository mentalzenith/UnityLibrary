using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace SuperConsole
{
    public static class EditorTools
    {
        public static void OpenScript(string fileName, int lineNumber)
        {
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fileName, lineNumber);
        }
    }
}