using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Node))]
//public class DataListEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        // Render the list using default Unity GUI (includes +, -, foldout, and reordering)
//        SerializedProperty dataList = serializedObject.FindProperty("testListItem");
//        EditorGUILayout.PropertyField(dataList, new GUIContent("Test List"), true);

//        serializedObject.ApplyModifiedProperties();
//    }
//}

[CustomEditor(typeof(Node))]
public class Inspector_Node : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Render the list using default Unity GUI
        SerializedProperty dataList = serializedObject.FindProperty("testListItem");

        // Track changes to detect new elements
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(dataList, new GUIContent("Test List"), true);
        if (EditorGUI.EndChangeCheck())
        {
            // Check if the list size increased (new element added via "+")
            if (dataList.arraySize > 0)
            {
                // Set default values for the last element
                var newElement = dataList.GetArrayElementAtIndex(dataList.arraySize - 1);
                var eventType = newElement.FindPropertyRelative("eventType");
                var param_v = newElement.FindPropertyRelative("param_v");
                var param_f = newElement.FindPropertyRelative("param_f");

                // Only set defaults if the element is uninitialized
                if (eventType.enumValueIndex == 0 && param_v.vector3Value == Vector3.zero && param_f.floatValue == 0f)
                {
                    eventType.enumValueIndex = (int)DataType.Vector3; // Default to Vector3
                    param_v.vector3Value = Vector3.zero;
                    param_f.floatValue = 0f;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}