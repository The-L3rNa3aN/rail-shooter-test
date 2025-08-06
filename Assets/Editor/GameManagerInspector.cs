using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameManager gm = (GameManager)target;

        if(GUILayout.Button("Reorder 'Nodes' list..."))
            gm.ReorderNodesList();
    }
}
