using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public class Node: ScriptableObject
    {
        [System.Serializable]
        public enum State { Sucess, Failure, Running }
        public List<Node> children = new();

        readonly string nodeName;
        protected int currentChild;
        readonly int priority;
        
        public Node(string name, int priority = 0)
        {
            nodeName = name;
            this.priority = priority;
        }

        public void AddChild(Node node) => children.Add(node);
        public virtual State Process() => children[currentChild].Process();
        public virtual void Reset()
        {
            currentChild = 0;
            foreach (Node child in children)
            {
                child.Reset();
            }
        }
    }
}
