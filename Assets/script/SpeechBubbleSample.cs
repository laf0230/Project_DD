using System.Collections;
using UnityEngine;

public class SpeechBubbleSample : MonoBehaviour
{
    public Coroutine activeRoutine;

    public void StartRoutine()
    {
        gameObject.SetActive(true);
        activeRoutine = StartCoroutine(ActiveRoutine());
    }

    public IEnumerator ActiveRoutine()
    {
        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
