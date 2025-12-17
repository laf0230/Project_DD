using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackboardSystem
{
    [Serializable]
    public readonly struct BlackboardKey : IEquatable<BlackboardKey>
    {
        readonly string name;
        readonly int hashedKey;

        public BlackboardKey(string name)
        {
            this.name = name;
            hashedKey = name.ComputeFNV1Hash();
        }

        public bool Equals(BlackboardKey other) => hashedKey == other.hashedKey;

        public override bool Equals(object obj) => obj is BlackboardKey other && Equals(other);
        public override int GetHashCode() => hashedKey;
        public override string ToString() => name;

        public static bool operator ==(BlackboardKey left, BlackboardKey right) => left.name == right.name;
        public static bool operator !=(BlackboardKey left, BlackboardKey right) => !(left.name == right.name);
    }

    [Serializable]
    public class BlackboardEntry<T>
    {
        public BlackboardKey Key { get; }
        public T Value { get; }
        public Type ValueType { get; }

        public BlackboardEntry(BlackboardKey key, T value)
        {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }
        public override bool Equals(object obj) => obj is BlackboardEntry<T> other && other.Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
    }

    [Serializable]
    public class Blackboard
    {
        Dictionary<string, BlackboardKey> keyRegistry = new();
        Dictionary<BlackboardKey, object> entries = new();

        public List<Action> PassedActions { get; } = new();

        public void AddAction(Action action)
        {
            PassedActions.Add(action);
        }

        public void ClearActions()
        {
            PassedActions.Clear(); 
        }

        public void Debug()
        {
            foreach(var entry in entries)
            {
                var entryType = entry.Value.GetType();

                if(entryType.IsGenericType && entryType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>))
                {
                    var valueProperty = entryType.GetProperty("Value");
                    if (valueProperty == null) continue;
                    var value = valueProperty.GetValue(entry.Value);
                    UnityEngine.Debug.Log($"Key: {entry.Value.ToString()}, Value: {value}");
                }
            }
        }

        public bool TrygetValue<T>(BlackboardKey key, out T value)
        {
            if(entries.TryGetValue(key, out var entry) && entry is BlackboardEntry<T> castedEntry)
            {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        public void SetValue<T>(BlackboardKey key, T value)
        {
            entries[key] = new BlackboardEntry<T>(key, value);
        }

        public BlackboardKey GetOrRegisterKey(string keyName)
        {
            if(!keyRegistry.TryGetValue(keyName, out BlackboardKey key))
            {
                key = new BlackboardKey(keyName);
                keyRegistry[keyName] = key;
            }

            return key;
        }

        public bool ContainKey(BlackboardKey key) => entries.ContainsKey(key);

        public void Remove(BlackboardKey key) => entries.Remove(key);
    }
}

public static class StringExtensions
{
    public static int ComputeFNV1Hash(this string str)
    {
        uint hash = 2166136261;
        foreach (char c in str)
        {
            hash = (hash ^ c)  * 16777619;
        }
        return unchecked((int)hash);
    }
}
