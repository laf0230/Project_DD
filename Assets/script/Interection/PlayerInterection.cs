using UnityEngine;

public class PlayerInterection : MonoBehaviour
{
    public bool isInterecctable = true;
    public LayerMask interectionLayer;

    public void Interection()
    {
        if (!isInterecctable) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Interectable")))
        {
            if(hit.transform.TryGetComponent<IInterectable>(out IInterectable interectable))
            {
                Debug.Log($"Hit{hit.transform}");
                interectable.Interect();
            }
        }
    }
}
