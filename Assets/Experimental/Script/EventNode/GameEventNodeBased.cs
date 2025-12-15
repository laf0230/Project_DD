using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GamEventNode
{
    public static class GameEventBoard
    {
        public static Dictionary<string, List<GameNode>> nodeDictionary = new Dictionary<string, List<GameNode>>();

        public static void Register(GameNode node)
        {
            if (!nodeDictionary.TryGetValue(node._EventName, out List<GameNode> nodeList))
            {
                nodeDictionary.Add(node._EventName, nodeList = new List<GameNode>());
            }
            nodeList.Add(node);
        }

        public static void Invoke(string eventName)
        {
            if (!nodeDictionary.TryGetValue(eventName, out List<GameNode> node))
            {
                Debug.Log($"GameEventBord: {eventName}");
            }
            else
            {
                for (global::System.Int32 i = 0; i < node.Count; i++)
                {
                    node[i]._Event?.Invoke();
                }
            }
        }
    }
}