using UnityEngine;
using WaypointSystem;

public class GameManager : MonoBehaviour
{
    public WaypointManager wayPointManager;

    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}