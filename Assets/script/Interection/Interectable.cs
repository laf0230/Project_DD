using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("인터랙션/인터랙션 반응 이벤트")]
public class Interectable : MonoBehaviour, IInterectable
{
    [Header("도움이 필요하면 이름에 마우스를 올려보세요")]
    [Tooltip("상호작용 시 발생하는 이벤트, 오브젝트를 끌어와서 적용하기")]
    [SerializeField] UnityEvent OnInterected;
    [Tooltip("플레이어의 행동을 막기 위한 타입 \n Conversation: 움직일 수 없음 \n Nothing: 움직일 수 있음")]
    [SerializeField] InterectionType interectionType;

    public InterectionType Interect()
    {
        OnInterected?.Invoke();

        return interectionType;
    }
}
