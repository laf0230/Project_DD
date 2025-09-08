using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public class Node
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
        public virtual void Reset()
        {
            currentChild = 0;
            foreach (Node child in children)
            {
                child.Reset();
            }
        }
    }

    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name)
        {
        }

        public override State Process()
        {
            while(currentChild < children.Count)
            {
                var state = children[currentChild].Process();

                if(state != State.Sucess)
                {
                    return state;
                }
                currentChild++;
            }
            return State.Sucess;
        }
    }

    public class Selector : Node // OR failure일 시 다음 자식 실행 자식이 하나라도 Success이면 Success하고 종료
    {
        public Selector(string name, int priority = 0) : base(name, priority)
        {
        }

        public override State Process()
        {
            // 하나를 선택해서 실행함
            if(currentChild < children.Count)
            {
                switch(children[currentChild].Process())
                {
                    case State.Sucess:
                        return State.Sucess;
                    case State.Running:
                        return State.Running;
                    default:
                        currentChild++;
                        return State.Running;
                }
            }
            Reset();
            return State.Sucess;
        }
    }

    public class Sequence : Node // AND 하나라도 failure이면 모두 failure 모든 자식이 Success이면 Success
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
                        Reset();
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
            Reset();
            return State.Running;
        }
    }

    public class Leaf : Node
    {
        [SerializeField]
        readonly IStrategy strategy;

        public Leaf(string name, int priority) : base(name, priority) { }

        public override State Process() => strategy.Process();

        public override void Reset() => strategy.ResetStrategy();
    }
}
