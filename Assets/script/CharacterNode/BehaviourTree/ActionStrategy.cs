using System;
using UnityEngine;
using UnityEngine.Events;

namespace Node
{
    [CreateAssetMenu(menuName = "Node/ActionNode")]
    public class ActionStrategy : ScriptableObject, IStrategy
    {
        [SerializeField] Action action;

        public ActionStrategy(Action action)
        {
            this.action = action;
        }

        public Node.State Process()
        {
            action?.Invoke();
            return Node.State.Sucess;
        }

        public void ResetStrategy() { }
    }
}
