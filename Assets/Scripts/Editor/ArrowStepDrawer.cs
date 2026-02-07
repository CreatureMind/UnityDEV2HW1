using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ArrowStep))]
    public class ArrowStepDrawer : PropertyDrawer {
        private const float HEADER_HEIGHT = 30f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var index = GetArrayIndex(property);
            EditorGUI.BeginProperty(position, label, property);
        
            if (index % 8 == 0) {
                var lineRect = new Rect(position.x, position.y + 5, position.width, 1);
                EditorGUI.DrawRect(lineRect, Color.gray);

                var labelRect = new Rect(position.x, position.y + 8, position.width, 18);
                EditorGUI.LabelField(labelRect, $"MEASURE {(index / 8) + 1}", EditorStyles.boldLabel);
                
                position.y += HEADER_HEIGHT;
            }
            
            position.height = EditorGUIUtility.singleLineHeight;
            
            var buttonArea = EditorGUI.PrefixLabel(position, new GUIContent($"Step {index}"));
            var width = buttonArea.width / 4f;
            var r = new Rect(buttonArea.x, buttonArea.y, width, buttonArea.height);

            DrawArrow(r, property.FindPropertyRelative("left"), "←");
            r.x += width;
            DrawArrow(r, property.FindPropertyRelative("down"), "↓");
            r.x += width;
            DrawArrow(r, property.FindPropertyRelative("up"), "↑");
            r.x += width;
            DrawArrow(r, property.FindPropertyRelative("right"), "→");

            EditorGUI.EndProperty();
        }

        private void DrawArrow(Rect rect, SerializedProperty prop, string icon) {
            prop.boolValue = GUI.Toggle(rect, prop.boolValue, icon, "Button");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var baseHeight = EditorGUIUtility.singleLineHeight;
            var index = GetArrayIndex(property);
            
            return (index % 8 == 0) ? baseHeight + HEADER_HEIGHT : baseHeight;
        }

        private int GetArrayIndex(SerializedProperty property) {
            var path = property.propertyPath;
            var start = path.LastIndexOf('[') + 1;
            var end = path.LastIndexOf(']');
            if (start <= 0 || end <= 0) return 0;
            return int.Parse(path.Substring(start, end - start));
        }
    }
}