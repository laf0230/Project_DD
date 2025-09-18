using UnityEditor;
using UnityEngine;

namespace Inference
{

    [CreateAssetMenu(menuName = "ClueData")]
    public class ClueDataStroage : ScriptableObject
    {
        public TextAsset data;
        public Clue[] clues;

        [ContextMenu("")]
        public void Convert()
        {
            clues = JsonUtility.FromJson<DTClues>(data.text).clues;
        }

        public Clue GetClue(string id)
        {
            for (int i = 0; i < clues.Length; i++)
            {
                if (clues[i].id == id)
                    return clues[i];
            }

            Debug.LogError($"{id} clue is not exist. Check the Clue Data");
            return null;
        }
    }

    public class DTClues
    {
        public Clue[] clues;
    }
}
