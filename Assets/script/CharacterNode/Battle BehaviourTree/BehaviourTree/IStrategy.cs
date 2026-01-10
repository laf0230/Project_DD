using System;
using System.Collections;
using UnityEngine;

namespace BehaviourTrees
{
    public interface IStrategy
    {
        Node.State Process();

        void ResetStrategy() { }
    }

    public class Strategy : ScriptableObject, IStrategy
    {
        public virtual Node.State Process()
        {
            return Node.State.Failure;
        }

        public virtual void ResetStrategy() { }
    }
}
