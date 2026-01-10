using BlackboardSystem;
using ServiceLocator;
using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "Node/Decorator")]
    public class Decorator : Sequence
    {
        enum ComprisonOperator
        {
           NotEqual, Less, LessEqual, Equal, GreaterEqual, Greater
        }

        [Header("관측할 데이터")]
        [SerializeField] BlackboardEntryData conditionData;
        [Header("연산자")]
        [SerializeField] ComprisonOperator comprisonOperator;
        [Header("목표치")]
        [SerializeField] BlackboardEntryData targetValue;

        public Decorator(string name, int priority = 0) : base(name, priority)
        {
        }

        public override State Process()
        {
            var result = CheckAchived() ? State.Sucess : State.Failure;
            Debug.Log($"Condition: Key {conditionData.keyName} Value {conditionData.value}, Compatition To: Key {targetValue.keyName} Value {targetValue.value}");
            return result;
        }

        public bool CheckAchived()
        {
            if (conditionData == null || targetValue == null) return false;

            AnyValue current = conditionData.value;
            AnyValue target = targetValue.value;

            return comprisonOperator switch
            {
                ComprisonOperator.Equal => current.Equals(target),
                ComprisonOperator.NotEqual => !current.Equals(target),
                ComprisonOperator.Less => current.CompareTo(target) < 0,
                ComprisonOperator.LessEqual => current.CompareTo(target) <= 0,
                ComprisonOperator.Greater => current.CompareTo(target) > 0,
                ComprisonOperator.GreaterEqual => current.CompareTo(target) >= 0,
                _ => false
            };
        }
    }
}
