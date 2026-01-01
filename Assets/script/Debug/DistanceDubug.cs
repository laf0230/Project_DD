using UnityEngine;

public class DistanceDubug : MonoBehaviour
{
    public Color color = Color.white;
    public float distance = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
