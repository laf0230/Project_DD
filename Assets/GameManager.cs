using UnityEngine;
using UnityEngine.InputSystem;
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

    public void SetMouseEnable(bool enabled)
    {
        Cursor.visible = enabled;

        if(enabled) 
            Cursor.lockState = CursorLockMode.None;
        else 
            Cursor.lockState = CursorLockMode.Locked;
    }
}