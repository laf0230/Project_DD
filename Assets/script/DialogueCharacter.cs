using ServiceLocator;
using UnityEngine;
using UnityEngine.Events;

namespace DD.DialogueSystem
{
    public class DialogueCharacter : MonoBehaviour
    {
        public UnityEvent OnDialogueStart = new();
        public UnityEvent OnDialogueEnd = new();

        TalkManager talkManager;

        private void Start()
        {
            talkManager = Locator.Get<TalkManager>(this);
            //talkManager.
        }

        public void StartDialogue()
        {
            OnDialogueStart?.Invoke();
        }
    }
}
