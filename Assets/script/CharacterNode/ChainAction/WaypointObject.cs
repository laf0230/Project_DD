using UnityEngine;

public class WaypointObject : MonoBehaviour
{
    [SerializeField] WaypointSO waypointData;
    [SerializeField] int index = 0;

    private void Start()
    {
        waypointData.AddWaypoint(transform, index);
    }
}
