using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class DataListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Render the list using default Unity GUI (includes +, -, foldout, and reordering)
        SerializedProperty dataList = serializedObject.FindProperty("nodeEventList");
        EditorGUILayout.PropertyField(dataList, new GUIContent("Event List"), true);

        if(GUILayout.Button("Reposition camera to node")) RepositionCameraToNode();

        serializedObject.ApplyModifiedProperties();
    }

    private void RepositionCameraToNode()
    {
        Node node = (Node)target;

        if(Physics.Raycast(node.transform.position, Vector3.down, out RaycastHit hit))
        {
            if(hit.collider.gameObject.layer == 6)
            {
                Vector3 n = hit.point + hit.normal * Util.CamGroundDist;
                Camera.main.transform.position = n;

            }
        }
    }
}