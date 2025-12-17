using ServiceLocator;
using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "BehaviourTree/Strategy/new Attack Strategy")]
    public class AttackStrategy : Strategy
    {
        [SerializeField] int actionID;
        private BossAnimatorController animWrapper;

        bool isFirstTime = true;

        private void OnEnable()
        {
            isFirstTime = true;
            animWrapper = null;
        }

        public override Node.State Process()
        {
            if (animWrapper == null)
                animWrapper = Locator.Get<BossAnimatorController>();

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
