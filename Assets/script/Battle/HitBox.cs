using UnityEngine;
using UnityEngine.Events;

public class HitBox : MonoBehaviour
{
    public UnityEvent<HertBox> OnHit;
    public LayerMask hitMask;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out HertBox hertBox))
        {
            OnHit.Invoke(hertBox);
            hertBox?.OnHert.Invoke(this);
        }
    }
}
