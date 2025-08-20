using UnityEngine;
using UnityEngine.Pool;

public class BulletPool: MonoBehaviour
{
    public ObjectPool<Weapon> bulletPool;
    public Weapon bulletPrefab;

    private void Start()
    {
    }
}
