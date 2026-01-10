using Inference;
using ServiceLocator;
using UnityEngine;

[AddComponentMenu("단서 시스템/단서 획득 기능")]
public class ClueController : MonoBehaviour
{
    [Header("도움이 필요하면 마우스를 올려주세요.")]
    [Header("획득할 단서의 ID")]
    [SerializeField] string clueID;
    [Tooltip("획득할 단서의 설명")]
    [SerializeField] int clueLevel = 0;
    public void AddClue()
    {
        Locator.Get<InferenceManager>(this).AddClue(clueID, clueLevel);
    }
}
