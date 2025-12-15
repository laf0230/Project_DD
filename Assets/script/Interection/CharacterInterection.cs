using UnityEngine;
using UnityEngine.Events;

public class CharacterInterection : MonoBehaviour
{
    public UnityEvent OnInterected;

    public UnityEvent GetOnInterected() => OnInterected;

    public void Interect()
    {
        OnInterected?.Invoke();
        Debug.Log("characterinterection: interct");
    }
}
