using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WaypointSO")]
public class WaypointSO : ScriptableObject
{
    public List<Vector3> positions = new();
    public int Count => positions.Count;

    public void AddWaypoint(Transform transform, int position)
    {
        // 리스트 크기 자동 확장
        while (positions.Count <= position)
        {
            positions.Add(Vector3.zero);
        }

        // 중복 체크
        if (positions[position] != Vector3.zero)
        {
            Debug.LogWarning($"{transform.name} : {position} already has {positions[position]}.");
        }

        positions[position] = transform.position;
    }

    public Vector3 GetWaypoint(int position)
    {
        if (position < 0 || position >= positions.Count)
        {
            Debug.LogWarning($"Waypoint index {position} out of range.");
            return Vector3.zero;
        }
        return positions[position];
    }
}

