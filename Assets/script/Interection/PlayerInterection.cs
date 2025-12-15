using UnityEngine;

public class PlayerInterection : MonoBehaviour
{
    public bool isInterectable = true;
    public LayerMask interectionLayer;
    PlayerController playerController;

    private void Start()
    {
        if(!TryGetComponent(out playerController))
        {
            Debug.Log("PlayerInterection: This Component required PlayerController Class");
        }
    }

    public void Interection()
    {
        if (!isInterectable) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Interectable")))
        {
            if(hit.transform.TryGetComponent(out IInterectable interectable))
            {
                Debug.Log($"Hit{hit.transform}");

                switch (interectable.Interect())
                {
                    case InterectionType.Conversation:
                        playerController.isMovable = false;
                        playerController.UpdateState(PlayerState.Conversation);
                        break;
                    default:
                        playerController.isMovable = true;
                        playerController.UpdateState(PlayerState.Investigation);
                        break;
                }
            }
        }
    }
}
