using UnityEngine;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    AIInputCharacter character;

    private void Start()
    {
        character = GetComponent<AIInputCharacter>();

        if(GameManager.Instance.wayPointManager.GetWaypoint(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out Transform[] waypoints))
        {
            Vector3[] positions = new Vector3[waypoints.Length];

            for (int i = 0; i < waypoints.Length; i++)
            {
                positions[i] = waypoints[i].position;
            }

            character.SetDestinations(positions);
        }
    }
}
