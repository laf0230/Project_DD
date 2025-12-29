using UnityEngine;

public class SimpleWeapon : MonoBehaviour
{
    public float damageValue;

    public void OnDamage(HertBox hit)
    {
        if(hit.TryGetComponent(out Health health))
        {
            health.OnDamaged?.Invoke(damageValue);
        }
    }
}
