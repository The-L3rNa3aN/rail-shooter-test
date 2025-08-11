using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;

[CustomPropertyDrawer(typeof(NodeListItem))]
public class NodeListItemDrawer : PropertyDrawer
{
    public bool IsParamedEvent(int i) { return i == (int)Util.EventType.look || i == (int)Util.EventType.walk; }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get properties
        SerializedProperty eventType = property.FindPropertyRelative("eventType");
        SerializedProperty param_v = property.FindPropertyRelative("param_v");
        SerializedProperty param_f = property.FindPropertyRelative("param_f");
        SerializedProperty duration = property.FindPropertyRelative("duration");

        // Get the index of the list element for serial number
        int index = GetElementIndex(property);
        string eventName = eventType.enumValueIndex >= 0 ? eventType.enumDisplayNames[eventType.enumValueIndex] : "None";
        float durationValue = duration.floatValue;
        label.text = $"#{index + 1} {eventName} {durationValue:F1}";

        // Draw foldout
        Rect foldoutRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw fields if expanded
        if (property.isExpanded)
        {
            // Indent the content
            EditorGUI.indentLevel++;
            Rect lineRect = new(position.x, position.y + lineHeight, position.width, EditorGUIUtility.singleLineHeight);

            // Draw eventType field
            EditorGUI.PropertyField(lineRect, eventType, new GUIContent("Type"));
            lineRect.y += lineHeight;

            // Draw conditional field based on eventType
            if (eventType.enumValueIndex == (int)Util.EventType.look)
            {
                EditorGUI.PropertyField(lineRect, param_v, new GUIContent("Look At"));
            }
            else if (eventType.enumValueIndex == (int)Util.EventType.walk)
            {
                EditorGUI.PropertyField(lineRect, param_f, new GUIContent("Speed"));
            }

            if(IsParamedEvent(eventType.enumValueIndex)) lineRect.y += lineHeight;
            EditorGUI.PropertyField(lineRect, duration, new GUIContent("Duration"));
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if (property.isExpanded)
        {
            SerializedProperty eventType = property.FindPropertyRelative("eventType");
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // For the "eventType" field
            if (IsParamedEvent(eventType.enumValueIndex))
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // For the "param_v" or "param_f'
            }
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // For the "duration" field.
        }

        return height;
    }

    private int GetElementIndex(SerializedProperty property)
    {
        // Extract the index from the property path using regex
        string path = property.propertyPath;
        // Match patterns like "testListItem.Array.data[123]"
        Regex regex = new(@"\.Array\.data\[(\d+)\]");
        Match match = regex.Match(path);
        if (match.Success && int.TryParse(match.Groups[1].Value, out int index))
        {
            return index;
        }
        return -1; // Fallback if index cannot be determined
    }
}