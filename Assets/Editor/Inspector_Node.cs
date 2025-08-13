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

        serializedObject.ApplyModifiedProperties();
    }
}