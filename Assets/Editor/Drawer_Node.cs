using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum DataType { Float, Vector3 }

//[CustomPropertyDrawer(typeof(TestListItem))]
//public class TestListItemDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);

//        // Get properties
//        SerializedProperty eventType = property.FindPropertyRelative("eventType");
//        SerializedProperty param_v = property.FindPropertyRelative("param_v");
//        SerializedProperty param_f = property.FindPropertyRelative("param_f");

//        // Calculate rects for fields
//        Rect lineRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

//        // Draw eventType field
//        EditorGUI.PropertyField(lineRect, eventType, new GUIContent("Type"));
//        lineRect.y += lineHeight;

//        // Draw conditional field based on eventType
//        if (eventType.enumValueIndex == (int)DataType.Vector3)
//        {
//            EditorGUI.PropertyField(lineRect, param_v, new GUIContent("Vector3 Value"));
//        }
//        else if (eventType.enumValueIndex == (int)DataType.Float)
//        {
//            EditorGUI.PropertyField(lineRect, param_f, new GUIContent("Float Value"));
//        }

//        EditorGUI.EndProperty();
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        // Calculate height: one line for eventType, one line for either param_v or param_f
//        SerializedProperty eventType = property.FindPropertyRelative("eventType");
//        float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

//        if (eventType.enumValueIndex == (int)DataType.Vector3 || eventType.enumValueIndex == (int)DataType.Float)
//        {
//            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//        }

//        return height;
//    }
//}

[CustomPropertyDrawer(typeof(TestListItem))]
public class TestListItemDrawer : PropertyDrawer
{
    // Static dictionary to store foldout states per property path
    private static readonly Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get properties
        SerializedProperty eventType = property.FindPropertyRelative("eventType");
        SerializedProperty param_v = property.FindPropertyRelative("param_v");
        SerializedProperty param_f = property.FindPropertyRelative("param_f");

        // Use property path as a unique key for foldout state
        string propertyPath = property.propertyPath;
        if (!foldoutStates.ContainsKey(propertyPath))
        {
            foldoutStates[propertyPath] = false; // Default to collapsed
        }

        // Calculate rects for fields
        Rect lineRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw foldout
        foldoutStates[propertyPath] = EditorGUI.Foldout(lineRect, foldoutStates[propertyPath], label, true);
        lineRect.y += lineHeight;

        // Draw fields if expanded
        if (foldoutStates[propertyPath])
        {
            EditorGUI.indentLevel++;
            // Draw eventType field
            EditorGUI.PropertyField(lineRect, eventType, new GUIContent("Type"));
            lineRect.y += lineHeight;

            // Draw conditional field based on eventType
            if (eventType.enumValueIndex == (int)DataType.Vector3)
            {
                EditorGUI.PropertyField(lineRect, param_v, new GUIContent("Vector3 Value"));
            }
            else if (eventType.enumValueIndex == (int)DataType.Float)
            {
                EditorGUI.PropertyField(lineRect, param_f, new GUIContent("Float Value"));
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Base height: one line for the foldout
        float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Add height for fields if expanded
        string propertyPath = property.propertyPath;
        if (foldoutStates.ContainsKey(propertyPath) && foldoutStates[propertyPath])
        {
            // One line for eventType
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // One line for either param_v or param_f
            SerializedProperty eventType = property.FindPropertyRelative("eventType");
            if (eventType.enumValueIndex == (int)DataType.Vector3 || eventType.enumValueIndex == (int)DataType.Float)
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        return height;
    }
}