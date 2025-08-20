using DD.Combat;
using UnityEngine;
public enum CombatType
{
    melee,
    Range
}

public class Combat : MonoBehaviour
{
    public StatSO stat;
    public Weapon weapon;

    public IMovable controller;
    public bool isAttackable = true;

    public virtual void Init(IMovable pc)
    {
        controller = pc;
        stat.stat.currentHealth = stat.stat.maxHealth;
    }

    public virtual void Attack(CombatType combatType)
    {
        if (!isAttackable)
            return;

        switch (combatType)
        {
            case CombatType.melee:
                weapon.Use(this);
                break;
            case CombatType.Range:
                break;
            default:
                break;
        }
    }

    public virtual void Damage(float damage)
    {
        stat.stat.currentHealth -= damage;
        if (stat.stat.currentHealth < 0)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {

    }
}

[System.Serializable]
public class Stat
{
    public float maxHealth;
    public float currentHealth;
}
