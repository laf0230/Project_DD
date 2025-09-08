using System;

namespace BehaviourTrees
{
    public interface IStrategy
    {
        Node.State Process();

        void ResetStrategy() { }
    }
}
