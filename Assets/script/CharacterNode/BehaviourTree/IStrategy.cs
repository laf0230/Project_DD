using System;

namespace Node
{
    public interface IStrategy
    {
        Node.State Process();

        void ResetStrategy() { }
    }
}
