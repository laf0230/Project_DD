using UnityEngine;

namespace Event2
{
    public abstract class Condtion
    {
        public enum ComparisonOperator { greater, Equal, NotEqual, Less, }

        public abstract void CheckData();
    }

    public abstract class Trigger
    {
        public abstract void Execute();
    }
}
