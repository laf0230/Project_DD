using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackboardSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "Blackboard/New Enty Data")]
    public class BlackboardEntryData : ScriptableObject, ISerializationCallbackReceiver
    {
        public string keyName;
        public AnyValue.ValueType valueType;
        public AnyValue value;

        public Blackboard _blackboard {get; private set;}
        public BlackboardKey _key { get; private set; }

        private void OnEnable()
        {
            _blackboard = null;
        }

        public void SetValueOnBlackboard(Blackboard blackboard)
        {
            _blackboard = blackboard;
            _key = blackboard.GetOrRegisterKey(keyName);
            setValueDispatchTable[value.type](blackboard, _key, value);
        }

        public void UpdateEntryData()
        {
            _blackboard.SetValue(_key, value);
        }

        // Dispatch table to set different types of value on the blackboard
        static Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>> setValueDispatchTable = new()
        {
            { AnyValue.ValueType.Bool, (blackboard, key, anyValue) => blackboard.SetValue<bool>(key, anyValue) },
            { AnyValue.ValueType.Int, (blackboard, key, anyValue) => blackboard.SetValue<int>(key, anyValue) },
            { AnyValue.ValueType.Float, (blackboard, key, anyValue) => blackboard.SetValue<float>(key, anyValue) },
            { AnyValue.ValueType.String, (blackboard, key, anyValue) => blackboard.SetValue<string>(key, anyValue) },
            { AnyValue.ValueType.Vector3, (blackboard, key, anyValue) => blackboard.SetValue<Vector3>(key, anyValue) },
        };

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() => value.type = valueType;
    }
}
