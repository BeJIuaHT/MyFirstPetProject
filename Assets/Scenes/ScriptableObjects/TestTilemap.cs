using UnityEditor;
using UnityEngine;

namespace Assets.Scenes.Scripts
{
    public class TestTilemap : ScriptableObject
    {
        
        [MenuItem("Tools/MyTool/My Button")]
        static void DoIt()
        {
            bool b = EditorUtility.DisplayDialog("MyTool", "My Button# !", "OK", "Cancel");
            if (b)
            {
                Debug.Log(b);
            }
            else
            {
                Debug.LogWarning(b);
            }
        }
    }
}