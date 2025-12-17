using System;
using UnityEngine;
using UnityEngine.Events;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "ActionStategy")]
    public class ActionStrategy : ScriptableObject, IStrategy
    {
        public Action action;

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
