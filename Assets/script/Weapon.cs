using System.Threading.Tasks;
using UnityEngine;

public class Weapon: MonoBehaviour, IColliderUse
{
    public string weaponName;
    public float damage;
    public float range = 1f;
    [Tooltip("움직임 제한/무기 휘두르는시간")]
    public int msDuration = 500;

    public async void Use(Combat user)
    {
        gameObject.SetActive(true);
        user.controller.SetIsMovable(false);
        user.isAttackable = false;

        Ray ray = new Ray(transform.position, Vector3.up);
        var targets = Physics.SphereCastAll(ray, range);

        foreach (var target in targets)
        {
            if(target.collider.TryGetComponent(out Combat combat))
            {
                if (user == combat) continue;

                combat.Damage(damage);
            }
        }

        await Task.Delay(msDuration);

        user.isAttackable = true;
        user.controller.SetIsMovable(true);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
