using UnityEngine;
using UnityEngine.Events;

namespace Talk
{
    [AddComponentMenu("Talk/CharacterTalk")]
    public class CharacterTalk : MonoBehaviour
    {
        public TalkManager manager;
        [SerializeField] string dialogueId = "Sample";
        public bool isTalkable = true;

        public void StartDialogue()
        {
            manager.ActiveDialogue(dialogueId);
        }

        public void ChangeDialogueID(string id) => dialogueId = id;

        public void SetTalkable(bool isTalkable) => this.isTalkable = isTalkable;
    }
}
