using UnityEngine;

namespace GamEventNode
{
    // 이벤트 등록하는 오브젝트
    public class EventHandler : MonoBehaviour
    {
        public GameNode container;

        private void Awake()
        {
            RegisterNode();
        }

        public void RegisterNode()
        {
            GameEventBoard.Register(container);
        }
    }

}