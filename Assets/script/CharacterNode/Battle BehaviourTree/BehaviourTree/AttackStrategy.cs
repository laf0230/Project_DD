using BlackboardSystem;
using ServiceLocator;
using UnityEngine;

namespace BehaviourTrees
{
    public class MoveStrategy : Strategy
    {
        // 이동과 달리기를 따로 함
        // 조건을 사용해서 분기를 만들면 될듯
        [SerializeField] string animID;
        [SerializeField] float moveSpeed;
        [SerializeField] Vector3 targetPosition;

        BossAnimatorWrapper anim;
        Blackboard blackboard;

        private void OnEnable()
        {
            blackboard = Locator.Get<BlackboardController>(this).GetBlackboard();
        }

        public override Node.State Process()
        {
            if (anim == null)
                anim = Locator.Get<BossAnimatorWrapper>(this);

            //if(blackboard.TrygetValue)
            //targetPosition = blackboard.

            anim.PlayAction(animID, 0f);



            return Node.State.Running;
        }
    }

    [CreateAssetMenu(menuName = "BehaviourTree/Strategy/new Attack Strategy")]
    public class AttackStrategy : Strategy
    {
        [SerializeField] int actionID;
        private BossAnimatorWrapper animWrapper;

        bool isFirstTime = true;

        private void OnEnable()
        {
            isFirstTime = true;
            animWrapper = null;
        }

        public override Node.State Process()
        {
            if (animWrapper == null)
                animWrapper = Locator.Get<BossAnimatorWrapper>(this);

            if(isFirstTime)
            {
                animWrapper.PlayAction("DoAction", actionID);

                isFirstTime = false;

                return Node.State.Running;
            }

            if(animWrapper.isActionFinished)
            {
                ResetStrategy();
                return Node.State.Sucess;
            }

            return Node.State.Running;
        }

        public override void ResetStrategy()
        {
            isFirstTime = true;
        }
    }
}
