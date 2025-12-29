using UnityEngine;
using System.Collections.Generic;
using ServiceLocator;
using System;

namespace BlackboardSystem
{
    public class Beacon : MonoBehaviour
    {
        public string[] blackboardKeyNames;
        private BlackboardKey[] blackboardKeys;
        private Blackboard blackboard;
        public List<BlackboardEntryData> entries = new();

        public void Start()
        {
            blackboard = Locator.Get<BlackboardController>(this).GetBlackboard();
            blackboardKeys = new BlackboardKey[blackboardKeyNames.Length];

            for (int i = 0; i < blackboardKeyNames.Length; i++)
            {
                blackboardKeys[i] = blackboard.GetOrRegisterKey(entries[i].keyName);
            }

            entries.Add(new BlackboardEntryData() { keyName = "Hi", valueType = AnyValue.ValueType.Bool, value = new AnyValue { boolValue = false } });
            //UpdateBlackboardKey("Hi", new AnyValue { boolValue = true});
        }

        public void UpdateBlackboard(object key)
        {
            foreach (BlackboardEntryData entry in entries)
            {
                //if(entry.keyName == keyName)
                //{
                //    entry.value = value;
                //    Debug.Log($"Updated Entry Value {entry.value}, AnyValueType: {value.type}");
                //}
            }
        }

        //public void UpdateBlackboardKey()
        //{
        //    for (int i = 0; i < blackboardKeys.Length; i++)
        //    {
        //    }
        //}
    }
}
