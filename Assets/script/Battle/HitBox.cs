using UnityEngine;
using UnityEngine.Events;

public class HitBox : MonoBehaviour
{
    public UnityEvent<HertBox> OnHit;
    public string hitTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out HertBox hertBox) && other.CompareTag(hitTag))
        {
            OnHit.Invoke(hertBox);
            hertBox?.OnHert.Invoke(this);
        }
    }
}
