using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Reflection.Emit;
using System;
using ServiceLocator;

namespace BlackboardSystem
{
    public interface IExpert
    {
        int GetInsistence(Blackboard blackboard);
        void Execute(Blackboard blackboard);
    }

    public class Expert : MonoBehaviour, IExpert
    {
        // Value for tracking
        public string keyName;
        public BlackboardEntryData trackedData;

        // Curve Variable Setting
        public AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public List<KeyFrameEntryData> entries = new();
        public Blackboard blackboard;
        public int multiplyValue;
        // 현재 위치
        [SerializeField] float currentFrame;
        [SerializeField] int selectedEntry;

        private void Start()
        {
            Locator.Get<BlackboardController>(this).RegisterExpert(this);
            blackboard = Locator.Get<BlackboardController>(this).GetBlackboard();
        }

        public virtual void Execute(Blackboard blackboard)
        {
            blackboard.AddAction(() =>
            {
                KeyFrameEntryData bestMatch = null;
                float minDifference = 0.11f; // Slightly larger than your 0.1 tolerance

                foreach (var entry in entries)
                {
                    float diff = Mathf.Abs(currentFrame - entry.time);

                    // If the difference is within the 0.1 margin
                    if (diff <= 0.1f)
                    {
                        // Ensure we pick the absolute closest one if multiple exist
                        if (diff < minDifference)
                        {
                            minDifference = diff;
                            bestMatch = entry;
                        }
                    }
                }

                if (bestMatch != null)
                {
                    var finalValue = bestMatch.blackboardEntryTargetData.value;
                    bestMatch.blackboardEntryData.value = finalValue;

                    selectedEntry = entries.IndexOf(bestMatch);
                }
            });
        }

        public virtual int GetInsistence(Blackboard blackboard)
        {
            var weightValue = weightCurve.Evaluate(trackedData.value);

            var finalValue = Mathf.Floor(weightValue * 10) / 10;

            currentFrame = finalValue;
            Debug.Log($"Expert FameDebug: {currentFrame}");

            // Select Nearest Data

            var result = currentFrame * multiplyValue;
            return Mathf.FloorToInt(result);
        }
    }

        [Serializable]
    public class KeyFrameEntryData : ISerializationCallbackReceiver
    {
        public float time;
        public float weight;
        public BlackboardEntryData blackboardEntryData;
        public BlackboardEntryData blackboardEntryTargetData;
        public BlackboardKey key;

        public void OnAfterDeserialize() { }

        public void OnBeforeSerialize() { }
    }

    [CustomEditor(typeof(Expert), true)]
    public class ExpertEditor : Editor
    {
        SerializedProperty trackedData;
        SerializedProperty curveProperty;
        SerializedProperty targetKey;
        ReorderableList entryList;

        private void OnEnable()
        {
            trackedData = serializedObject.FindProperty("trackedData");
            curveProperty = serializedObject.FindProperty("weightCurve");
            targetKey = serializedObject.FindProperty("keyName");
            //var targetKeyRect = new Rect(0, 0, EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight);
            //EditorGUI.PropertyField(targetKeyRect, targetKey);

            entryList = new ReorderableList(serializedObject, serializedObject.FindProperty("entries"), true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, "Keyframe Data (Time | Weight | Blackboard Data)");
                },
                elementHeightCallback = (index) =>
                {
                    return (EditorGUIUtility.singleLineHeight * 2) + 10;
                },
                // 리스트 각 항목의 높이를 설정 (한 줄에 다 넣기 위해 기본 높이 사용)
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var element = entryList.serializedProperty.GetArrayElementAtIndex(index);
                    var time = element.FindPropertyRelative("time");
                    var weight = element.FindPropertyRelative("weight");
                    var blackboardEntryDataField = element.FindPropertyRelative("blackboardEntryData");
                    var blackboardEntryTargetDataField = element.FindPropertyRelative("blackboardEntryTargetData");
                    //var keyName = blackboardData.FindPropertyRelative("keyName");
                    //var valueType = blackboardData.FindPropertyRelative("valueType");
                    //var value = blackboardData.FindPropertyRelative("value");

                    rect.y += 2;
                    float spacing = 0.5f;
                    // 가로 너비를 3등분하여 배치
                    float width = (rect.width - (spacing * 2)) / 2f;
                    float prevWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 120;

                    // 1. Time 필드 (수정 시 커브 반영을 위해 PropertyField 사용)
                    Rect timeRect = new Rect(rect.x, rect.y, width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(timeRect, time, new GUIContent("위치"));

                    // 2. Weight 필드
                    Rect weightRect = new Rect(rect.x + width + spacing, rect.y, width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(weightRect, weight, new GUIContent("가중치"));

                    // Blackboard SO 필드
                    Rect blackboardEntryDataFieldRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing, width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(blackboardEntryDataFieldRect, blackboardEntryDataField, new GUIContent("변경 전 데이터"));

                    // TargetBlackboard SO 필드
                    Rect blackboardEntryTargetDataFieldRect = new Rect(rect.x + width + spacing, rect.y + EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing, width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(blackboardEntryTargetDataFieldRect, blackboardEntryTargetDataField, new GUIContent("변경 후 데이터"));

                    EditorGUIUtility.labelWidth = prevWidth;

                    //// 3. Blackboard 데이터 필드
                    //var keyNameRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                    //var valueTypeRect = new Rect(rect.x + rect.width * 0.3f, rect.y + EditorGUIUtility.singleLineHeight, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                    //var valueRect = new Rect(rect.x + rect.width * 0.6f, rect.y + EditorGUIUtility.singleLineHeight, rect.width * 0.4f, EditorGUIUtility.singleLineHeight);

                    //EditorGUI.PropertyField(keyNameRect, keyName, GUIContent.none);
                    //EditorGUI.PropertyField(valueTypeRect, valueType, GUIContent.none);

                    //switch ((AnyValue.ValueType)valueType.enumValueIndex)
                    //{
                    //    case AnyValue.ValueType.Bool:
                    //        var boolValue = value.FindPropertyRelative("boolValue");
                    //        EditorGUI.PropertyField(valueRect, boolValue, GUIContent.none);
                    //        break;
                    //    case AnyValue.ValueType.Int:
                    //        var intValue = value.FindPropertyRelative("intValue");
                    //        EditorGUI.PropertyField(valueRect, intValue, GUIContent.none);
                    //        break;
                    //    case AnyValue.ValueType.Float:
                    //        var floatValue = value.FindPropertyRelative("floatValue");
                    //        EditorGUI.PropertyField(valueRect, floatValue, GUIContent.none);
                    //        break;
                    //    case AnyValue.ValueType.String:
                    //        var stringValue = value.FindPropertyRelative("stringValue");
                    //        EditorGUI.PropertyField(valueRect, stringValue, GUIContent.none);
                    //        break;
                    //    case AnyValue.ValueType.Vector3:
                    //        var vector3Value = value.FindPropertyRelative("vector3Value");
                    //        EditorGUI.PropertyField(valueRect, vector3Value, GUIContent.none);
                    //        break;
                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(trackedData, new GUIContent("추적할 데이터"));

            var multiplyValue = serializedObject.FindProperty("multiplyValue");
            EditorGUILayout.PropertyField(multiplyValue, new GUIContent("MultiplyValue"));

            // 1. 커브 수정 감지
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(curveProperty, new GUIContent("Weight Curve"));
            if (EditorGUI.EndChangeCheck())
            {
                SyncListFromCurve();
            }

            EditorGUILayout.Space(10);

            // 2. 리스트 수정 감지
            EditorGUI.BeginChangeCheck();
            entryList.DoLayoutList();
            if (EditorGUI.EndChangeCheck())
            {
                SyncCurveFromList();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void SyncListFromCurve()
        {
            var curve = curveProperty.animationCurveValue;
            var entriesProp = serializedObject.FindProperty("entries");

            entriesProp.arraySize = curve.length;
            for (int i = 0; i < curve.length; i++)
            {
                var element = entriesProp.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("time").floatValue = curve.keys[i].time;
                element.FindPropertyRelative("weight").floatValue = curve.keys[i].value;
            }
        }

        private void SyncCurveFromList()
        {
            var entriesProp = serializedObject.FindProperty("entries");
            Keyframe[] newKeys = new Keyframe[entriesProp.arraySize];

            for (int i = 0; i < entriesProp.arraySize; i++)
            {
                var element = entriesProp.GetArrayElementAtIndex(i);
                float time = element.FindPropertyRelative("time").floatValue;
                float weight = element.FindPropertyRelative("weight").floatValue;
                newKeys[i] = new Keyframe(time, weight);
            }

            // 새 커브 생성 (시간 순서 자동 정렬됨)
            AnimationCurve newCurve = new AnimationCurve(newKeys);
            curveProperty.animationCurveValue = newCurve;
        }
    }
}