using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

[CustomPropertyDrawer(typeof(NodeListItem))]
public class NodeListItemDrawer : PropertyDrawer
{
    public bool NoDurationEvents(int i) { return i == (int)Util.EventType.walk || i == (int)Util.EventType.open || i == (int)Util.EventType.stop || i == (int)Util.EventType.hold; }
    public bool NoParamEvents(int i) { return i == (int)Util.EventType.stop || i == (int)Util.EventType.hold || i == (int)Util.EventType.open || i == (int)Util.EventType.wait; }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get properties
        SerializedProperty eventType = property.FindPropertyRelative("eventType");
        SerializedProperty param_v = property.FindPropertyRelative("param_v");
        SerializedProperty param_f = property.FindPropertyRelative("param_f");
        //SerializedProperty param_b = property.FindPropertyRelative("param_b");
        SerializedProperty duration = property.FindPropertyRelative("duration");

        // Get the index of the list element for serial number
        int index = GetElementIndex(property);
        string eventName = eventType.enumValueIndex >= 0 ? eventType.enumDisplayNames[eventType.enumValueIndex] : "None";
        float durationValue = duration.floatValue;
        label.text = !NoDurationEvents(eventType.enumValueIndex) ? $"#{index + 1} {eventName} {durationValue:F1}s" : $"#{index + 1} {eventName}";
        if (NoDurationEvents(eventType.enumValueIndex)) duration.floatValue = 0f;       // Set 'duration' of any non-duration event to 0.

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
            switch(eventType.enumValueIndex)
            {
                case (int)Util.EventType.look:
                    EditorGUI.PropertyField(lineRect, param_v, new GUIContent("Look At"));
                    break;

                case (int)Util.EventType.walk:
                    EditorGUI.PropertyField(lineRect, param_f, new GUIContent("Speed"));
                    break;

                //case (int)Util.EventType.hold:
                //    EditorGUI.PropertyField(lineRect, param_b, new GUIContent("Indefinite?"));
                //    break;
            }

            if(!NoParamEvents(eventType.enumValueIndex)) lineRect.y += lineHeight;
            if(!NoDurationEvents(eventType.enumValueIndex)) EditorGUI.PropertyField(lineRect, duration, new GUIContent("Duration"));
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
            if (!NoParamEvents(eventType.enumValueIndex))
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // For the "param_v" / "param_f" / "isIndefinite"
            }
            if(!NoDurationEvents(eventType.enumValueIndex)) height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // For the "duration" field.
        }

        return height;
    }

    private int GetElementIndex(SerializedProperty property)
    {
        string path = property.propertyPath;
        Regex regex = new(@"\.Array\.data\[(\d+)\]");
        Match match = regex.Match(path);
        if (match.Success && int.TryParse(match.Groups[1].Value, out int index))
        {
            return index;
        }
        return -1; // Fallback if index cannot be determined
    }
}
