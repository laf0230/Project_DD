using ChainAction;
using Talk;
using UnityEngine;

namespace ChainAction
{
    [CreateAssetMenu(fileName = "[ChangeDialogueNode]", menuName = "ActionNode/ChangeDialogueNode")]
    public class ChangeDialogueNode: Node
    {
        public string id;
        public override Status Process(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out CharacterTalk component))
            {
                component.ChangeDialogueID(id);
                return Status.Success;
            }
            else
            {
                Debug.LogError($"ChangeDialogueNode: Check this object {gameObject}. couldn't find CharacterTalk Component.");
                return Status.Failure;
            }
        }
    }
}
