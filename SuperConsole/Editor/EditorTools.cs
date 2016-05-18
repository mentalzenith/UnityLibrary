using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SuperConsole
{
    public static class EditorTools
    {
        public static void OpenScript(string fileName, int lineNumber)
        {
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fileName, lineNumber);
            return;
            string s = fileName + " t:Script";
            var asset = AssetDatabase.FindAssets(s);
            if (asset.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset[0]);
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path+".cs", lineNumber);
            }
            else
                Debug.Log("asset " + fileName + " not found");
        }
    }
}