using System;
using System.Collections.Generic;
using UnityEngine;

namespace Node
{
    public class Node : ScriptableObject
    {
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
        public virtual void ResetNode()
        {
            currentChild = 0;
            foreach (Node child in children)
            {
                child.ResetNode();
            }
        }
    }

    public class BehaviourTree : Node
    {
        public BehaviourTree(string name, int priority = 0) : base(name, priority)
        {
        }

        public override State Process()
        {
            while(currentChild < children.Count)
            {
                var state = children[children.Count - 1].Process();
                if(state != State.Sucess)
                {
                    return state;
                }
                currentChild++;
            }
            return State.Sucess;
        }
    }

    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority)
        {
        }

        public override State Process()
        {
            if(currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case State.Failure:
                        return State.Failure;
                    case State.Running:
                        return State.Running;
                    default:
                        // 자식 노드가 sucess일 때 다음 노드로 이동
                        // 모든 노드 순회가 끝날 경우 sucess
                        // 순회가 종료되지 않았을 경우 running
                        currentChild++;
                        return currentChild == children.Count ? State.Sucess : State.Running;
                }
            }
            ResetNode();
            return State.Running;
        }
    }
}
