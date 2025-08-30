using System;
using UnityEngine;

namespace ChainAction
{
    public class Node : ScriptableObject
    {
        public enum Status { Success, Failure,  Running }
        public virtual Status Process(GameObject gameObject) { return Status.Success; }
        public virtual void ResetNode() { }
    }
}
