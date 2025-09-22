using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Inference
{

    [CreateAssetMenu(menuName = "ClueData")]
    public class ClueDataStroage : ScriptableObject
    {
        public TextAsset data;
        public Clue[] clues;

        public TextAsset recipieData;
        public ClueRecipe[] recipes;

        public Dictionary<string, IClue> clueCollection = new();

        private void OnEnable()
        {
            LoadData();
        }

        [ContextMenu("데이터 새로고침")]
        public void LoadData()
        {
            foreach (var clue in clues)
            {
                clueCollection.Add(clue.Id, clue);
                Debug.Log($"ClueData: {clue.Id} Included");
            }

            foreach(var recipe in recipes)
            {
                clueCollection.Add(recipe.Id, recipe);
                Debug.Log($"ClueData: {recipe.Id} Included\n Name: {recipe.Name}\n Tags: {string.Join(",", recipe.Tags)}\n Description: {recipe.Description}");
            }
        }

        [ContextMenu("파일로부터 데이터 변환")]
        public void Convert()
        {
            clues = JsonUtility.FromJson<DTClues>(data.text).clues;
        }

        public IClue GetClue(string id)
        {
            for (int i = 0; i < clueCollection.Count; i++)
            {
                if (clueCollection.TryGetValue(id, out IClue clue))
                    return clue;
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
