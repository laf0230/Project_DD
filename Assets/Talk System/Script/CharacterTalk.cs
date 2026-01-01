using UnityEngine;
using UnityEngine.Events;

namespace Talk
{
    [AddComponentMenu("대화/대화 오브젝트")]
    public class CharacterTalk : MonoBehaviour
    {
        [Header("활성화 시킬 대화 이름")]
        [Tooltip("이벤트를 통해서 대화를 교체하고 싶다면 \"인터랙션  반응 이벤트\"를 이용해주세요!")]
        [SerializeField] string dialogueId = "Sample";
        public bool isTalkable = true;

        public void StartDialogue()
        {
            TalkManager.Instance.ActiveDialogue(dialogueId);
        }

        public void ChangeDialogueID(string id) => dialogueId = id;

        public void SetTalkable(bool isTalkable) => this.isTalkable = isTalkable;
    }
}
