using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SuperConsole
{
    public static class EditorTools
    {
        public static void OpenScript(string fileName, int lineNumber)
        {
            string s = fileName + " t:Script";
            var asset = AssetDatabase.FindAssets(s);
            if (asset.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset[0]);
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path, lineNumber);
            }
            else
                Debug.Log("asset " + s + " not found");
        }
    }
}