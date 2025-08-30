using UnityEngine;
using UnityEngine.AI;

namespace ChainAction
{
    [CreateAssetMenu(menuName ="ActionNode/MoveNode")]
    public class MoveNode : Node
    {
        public WaypointSO waypointSO;
        int waypointIndex = 0;

        public override Status Process(GameObject gameObject)
        {
            var agent = gameObject.GetComponent<NavMeshAgent>();
            if (agent == null) return Status.Failure;

            Vector3 target = waypointSO.GetWaypoint(waypointIndex);

            // 경로가 없거나 목표가 다를 경우 새 경로 설정
            if (!agent.hasPath || agent.destination != target)
            {
                agent.SetDestination(target);
            }

            // 아직 이동 중일 때
            if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                return Status.Running;
            }

            // 목표에 도착했을 때
            waypointIndex++;

            if (waypointIndex >= waypointSO.Count)
            {
                ResetNode();
                return Status.Success;
            }

            return Status.Running;
        }

        public override void ResetNode()
        {
            waypointIndex = 0;
        }
    }
}

