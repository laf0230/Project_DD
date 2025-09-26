using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inference
{

    [CreateAssetMenu(menuName = "ClueData")]
    public class ClueDataStroage : ScriptableObject
    {
        public string savePath = "Data/Clue";
        public string fileName;

        public TextAsset data;
        public Clue[] clues;

        public ClueRecipe[] recipes;

        [Header("빌드 전용")]
        public bool isForBuild = false;

        public Dictionary<string, IClue> clueCollection = new();

        private void Awake()
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

            foreach (var recipe in recipes)
            {
                clueCollection.Add(recipe.Id, recipe);
                Debug.Log($"ClueData: {recipe.Id} Included\n Name: {recipe.Name}\n Tags: {string.Join(",", recipe.Tags)}\n Description: {recipe.Description}");
            }
        }

        [ContextMenu("Json으로부터 데이터 변환")]
        public void Convert()
        {
            string root = !isForBuild
                ? Application.persistentDataPath
                : Application.dataPath;

            string directory = Path.Combine(root, savePath);
            string path = Path.Combine(directory, fileName + ".json");

                if (!File.Exists(path))
                {
                    Debug.Log("해당 파일은 존재하지 않습니다.");
                    return;
                }
            var json = File.ReadAllText(path);
            var clueData = JsonUtility.FromJson<DTClues>(json);

            if(EditorUtility.DisplayDialog("Warining", "현재 등록된 데이터가 지워지고\n새로운 데이터가 등록됩니다.\n정말 하시겠습니까?", "예", "아니오"))
            {
                clues = clueData.clues;
                recipes = clueData.recipes;
                LoadData();
            }

            Debug.Log($"Saved ClueData Path: {path}");
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


        [ContextMenu("Json으로 변환")]
        public void ToJsonFile()
        {
            string root = !isForBuild
                ? Application.persistentDataPath
                : Application.dataPath;

            string directory = Path.Combine(root, savePath);

            Directory.CreateDirectory(directory);

            DTClues dtClue = new DTClues(clues, recipes);
            string content = JsonUtility.ToJson(dtClue, true);

            string path = Path.Combine(directory, fileName + ".json");

            if (File.Exists(path))
            {
                Debug.LogError($"Clue Data: Already Exist Same name file {{{fileName}}}");
                return;
            }
            File.WriteAllText(path, content);

            Debug.Log($"Saved ClueData Path: {path}");
        }
    }

        [System.Serializable]
    public class DTClues
    {
        public Clue[] clues;
        public ClueRecipe[] recipes;

        public DTClues(Clue[] clues, ClueRecipe[] recipes)
        {
            this.clues = clues;
            this.recipes = recipes;
        }
    }
}
