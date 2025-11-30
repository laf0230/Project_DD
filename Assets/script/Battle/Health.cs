using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    public UnityEvent<float> OnDamaged = new();
    public UnityEvent OnDead = new();

    private void Start()
    {
        currentHealth = maxHealth;

        OnDamaged?.AddListener(GetDamage);
    }

    public void GetDamage(float damageValue)
    {
        currentHealth -= damageValue;

        if(currentHealth  <= 0)
        {
            OnDead?.Invoke();
        }
    }
}
