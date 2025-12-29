using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace BlackboardSystem
{

    [CreateAssetMenu(menuName = "New Blackboard Data")]
    public class BlackboardData : ScriptableObject
    {
        public List<BlackboardEntryData> entries = new();

        public void SetValuesOnBlackboard(Blackboard blackboard)
        {
            foreach (var entry in entries)
            {
                entry.SetValueOnBlackboard(blackboard);
            }
        }
    }

    [CustomEditor(typeof(BlackboardEntryData))]
    public class BlackboardEntryDataEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var keyName = serializedObject.FindProperty("keyName");
            var valueType = serializedObject.FindProperty("valueType");
            var value = serializedObject.FindProperty("value");
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(keyName, GUIContent.none, GUILayout.Width(100));
            EditorGUILayout.PropertyField(valueType, GUIContent.none, GUILayout.Width(80));

            switch ((AnyValue.ValueType)valueType.enumValueIndex)
            {
                case AnyValue.ValueType.Bool:
                    var boolValue = value.FindPropertyRelative("boolValue");
                    EditorGUILayout.PropertyField(boolValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Int:
                    var intValue = value.FindPropertyRelative("intValue");
                    EditorGUILayout.PropertyField(intValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Float:
                    var floatValue = value.FindPropertyRelative("floatValue");
                    EditorGUILayout.PropertyField(floatValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.String:
                    var stringValue = value.FindPropertyRelative("stringValue");
                    EditorGUILayout.PropertyField(stringValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Vector3:
                    var vector3Value = value.FindPropertyRelative("vector3Value");
                    EditorGUILayout.PropertyField(vector3Value, GUIContent.none);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

    //[CustomPropertyDrawer(typeof(BlackboardEntryData))]
    public class BlackboardEntryDataPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var keyName = property.FindPropertyRelative("keyName");
            var valueType = property.FindPropertyRelative("valueType");
            var value = property.FindPropertyRelative("value");

            var keyNameRect = new Rect(position.x, position.y, position.width * 0.3f, EditorGUIUtility.singleLineHeight);
            var valueTypeRect = new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.3f, EditorGUIUtility.singleLineHeight);
            var valueRect = new Rect(position.x + position.width * 0.6f, position.y, position.width * 0.4f, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(keyNameRect, keyName, GUIContent.none);
            EditorGUI.PropertyField(valueTypeRect, valueType, GUIContent.none);

            switch ((AnyValue.ValueType)valueType.enumValueIndex)
            {
                case AnyValue.ValueType.Bool:
                    var boolValue = value.FindPropertyRelative("boolValue");
                    EditorGUI.PropertyField(valueRect, boolValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Int:
                    var intValue = value.FindPropertyRelative("intValue");
                    EditorGUI.PropertyField(valueRect, intValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Float:
                    var floatValue = value.FindPropertyRelative("floatValue");
                    EditorGUI.PropertyField(valueRect, floatValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.String:
                    var stringValue = value.FindPropertyRelative("stringValue");
                    EditorGUI.PropertyField(valueRect, stringValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Vector3:
                    var vector3Value = value.FindPropertyRelative("vector3Value");
                    EditorGUI.PropertyField(valueRect, vector3Value, GUIContent.none);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [Serializable]
    public class AnyValue: IEquatable<AnyValue>, IComparable<AnyValue>
    {
        public enum ValueType { Bool, Int, Float, String, Vector3 }
        public ValueType type;

        // Storage Different types of values
        public bool boolValue;
        public int intValue;
        public float floatValue;
        public string stringValue;
        public Vector3 vector3Value;

        // Implicit conversion operators to convert AnyValue to different types
        public static implicit operator bool(AnyValue value) => value.ConvertValue<bool>();
        public static implicit operator int(AnyValue value) => value.ConvertValue<int>();
        public static implicit operator float(AnyValue value) => value.ConvertValue<float>();
        public static implicit operator string(AnyValue value) => value.ConvertValue<string>();
        public static implicit operator Vector3(AnyValue value) => value.ConvertValue<Vector3>();

        T ConvertValue<T>()
        {
            return type switch
            {
                ValueType.Bool => AsBool<T>(boolValue),
                ValueType.Int => AsInt<T>(intValue),
                ValueType.Float => AsFloat<T>(floatValue),
                ValueType.String => (T)(object)stringValue,
                ValueType.Vector3 => (T)(object)vector3Value,
                _ => throw new NotSupportedException($"Not supported value type: {typeof(T)}")
            };
        }

        // Methods to convert primitive types to generic types with type safety and without boxing
        T AsBool<T>(bool value) => typeof(T) == typeof(bool) && value is T correctType ? correctType : default;
        T AsInt<T>(int value) => typeof(T) == typeof(int) && value is T correctType ? correctType : default;
        T AsFloat<T>(float value) => typeof(T) == typeof(float) && value is T correctType ? correctType : default;

        public override bool Equals(object obj) => Equals(obj as AnyValue);

        public bool Equals(AnyValue other)
        {
            if (ReferenceEquals(null, other)) return false;
            if(ReferenceEquals(this, other)) return true;

            if(type != other.type) return false;

            return type switch
            {
                ValueType.Bool => boolValue == other.boolValue,
                ValueType.Int => intValue == other.intValue,
                ValueType.Float => floatValue == other.floatValue,
                ValueType.String => stringValue == other.stringValue,
                ValueType.Vector3 => vector3Value == other.vector3Value,
                _ => false
            };
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)type;
                hashCode = (hashCode * 397) ^ type switch
                {
                    ValueType.Bool => boolValue.GetHashCode(),
                    ValueType.Int => intValue.GetHashCode(),
                    ValueType.Float => floatValue.GetHashCode(),
                    ValueType.String => stringValue.GetHashCode(),
                    ValueType.Vector3 => vector3Value.GetHashCode(),
                    _ => 0
                };
                return hashCode;
            }
        }

        public int CompareTo(AnyValue other)
        {
            if (other == null) return 1;

            if(IsNumber(this.type) && IsNumber(other.type))
            {
                float selfVal = this.type == ValueType.Int ? this.intValue : this.floatValue;
                float otherVal = other.type == ValueType.Int ? other.intValue : other.floatValue;
                return selfVal.CompareTo(other);
            }

            if(this.type == other.type)
            {
                return type switch
                {
                    ValueType.String => string.Compare(this.stringValue, other.stringValue),
                    ValueType.Int => intValue.CompareTo(other.intValue),
                    ValueType.Float => floatValue.CompareTo(other.floatValue),
                    _ => 0
                };
            }

            return 0;
        }

        public bool IsNumber(ValueType t) => t == ValueType.Int || t == ValueType.Float;

        public static bool operator ==(AnyValue left, AnyValue right) => Equals(left, right);
        public static bool operator !=(AnyValue left, AnyValue right) => !Equals(left, right);
    }
}


