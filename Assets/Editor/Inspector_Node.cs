using PlasticPipe.PlasticProtocol.Messages;
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

        if(GUILayout.Button("Set node height from ground"))
        {
            Node node = (Node)target;
            node.transform.position = Util.PosOffsetFromGround(node.transform.position, Util.NodeGroundDist);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void RepositionCameraToNode()
    {
        Node node = (Node)target;
        Camera.main.transform.position = Util.PosOffsetFromGround(node.transform.position, Util.CamGroundDist);

        GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
        int no = g.nodes.IndexOf(node.transform);

        // Align camera along node path.
        if(no < g.nodes.Count - 1)
        {
            Transform t = g.nodes[no + 1];
            Vector3 dir = Vector3.Normalize(t.position - node.transform.position);
            Camera.main.transform.rotation = Quaternion.LookRotation(dir);
        }
        else
        {
            Debug.Log("Camera can't be aligned due to the absence of further nodes.");
        }
    }
}