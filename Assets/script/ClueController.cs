using Inference;
using ServiceLocator;
using UnityEngine;

public class ClueController : MonoBehaviour
{
    [SerializeField] string clueID;
    [SerializeField] int clueLevel = 0;
    public void AddClue()
    {
        Locator.Get<InferenceManager>(this).AddClue(clueID, clueLevel);
    }
}
