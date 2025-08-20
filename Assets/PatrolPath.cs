using System.Linq;
using UnityEngine;

[System.Serializable]
public class PathPair
{
    public string pathName;
    public Transform[] pathpoints;
}

[System.Serializable]
public class PathArr
{
    public PathPair[] pathpairs;

    public Transform[] this[string pathName]
    {
        get => pathpairs.FirstOrDefault(p => p.pathName == pathName)?.pathpoints;
    }
}

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private PathArr waypoints;
    [SerializeField] private int currentIndex = 0;

    public Transform GetWaypoint(string pathName, int index)
    {
        var path = waypoints[pathName];
        if (path != null && index >= 0 && index < path.Length)
        {
            return path[index];
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.pathpairs == null) return;

        foreach (var pathPair in waypoints.pathpairs)
        {
            if (pathPair.pathpoints == null || pathPair.pathpoints.Length < 2)
                continue;

            for (int i = 0; i < pathPair.pathpoints.Length - 1; i++)
            {
                if (pathPair.pathpoints[i] != null && pathPair.pathpoints[i + 1] != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(pathPair.pathpoints[i].position, pathPair.pathpoints[i + 1].position);
                }
            }
        }
    }
}

