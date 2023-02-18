using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Test))]
// ^ T$$anonymous$$s is the script we are making a custom editor for.
public class YourScriptEditor: Editor
{
    
     override public void OnInspectorGUI () 
    {
        //Called whenever the inspector is drawn for t$$anonymous$$s object.
        DrawDefaultInspector();
        //T$$anonymous$$s draws the default screen.  You don't need t$$anonymous$$s if you want
        //to start from scratch, but I use t$$anonymous$$s when I'm just adding a button or
        //some small addition and don't feel like recreating the whole inspector.
        if (GUILayout.Button("YourScriptEditor Test")) {
            //add evert$$anonymous$$ng the button would do.
            ConsoleTest();
         }
     }
    private void ConsoleTest()
    {
        Debug.Log("Тест");
    }

 }