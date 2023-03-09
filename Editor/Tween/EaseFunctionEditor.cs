using UnityEngine;
using UnityEditor;

namespace Zlitz.Animation
{
    [CustomPropertyDrawer(typeof(EaseFunction))]
    public class EaseFunctionPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 3.0f * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeProperty   = property.FindPropertyRelative("m_type");
            SerializedProperty customProperty = property.FindPropertyRelative("m_custom");

            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(position, label);

            ++EditorGUI.indentLevel;

            position.y += position.height;
            EditorGUI.PropertyField(position, typeProperty, new GUIContent("Type"));

            position.y += position.height;
            EditorGUI.BeginDisabledGroup(((EaseFunction.Type)typeProperty.enumValueIndex) != EaseFunction.Type.Custom);
            EditorGUI.PropertyField(position, customProperty, new GUIContent("Custom Function"));
            EditorGUI.EndDisabledGroup();

            --EditorGUI.indentLevel;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
