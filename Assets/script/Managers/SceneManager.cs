using DD;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
            Destroy(instance);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
