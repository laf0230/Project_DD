using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inference
{
    public class InferenceManager : MonoBehaviour
    {
        public ClueDataStroage clueData;
        public Dictionary<string, IClue> ownClueContainer = new();
        public Dictionary<string, Clue> selectedClueList;

        // 단서 해금 기능 목록
        // Complate 단서 설명 레벨에 따라서 단서 텍스트 해금
        // Complate 단서 자체 해금
        // Todo 단서 반환
        // Todo 기획자가 설정하기 쉽게 이벤트 추가

        // 단서 사용 기능 목록
        // Complate Todo 태그별로 정렬
        // Complete 태그에 맞는 단서목록 호출 (ID값, ClueData)

        // 단서 분류
        // Todo 기획자가 원하는 태그를 기준으로 카테고리를 나눌 수 있도록 작성 - 이후에 추가적인 확장성을 고려함(ex. 접는 기능 등)
        // Todo 기획자가 UI에서 태그를 입력하면 해당 태그를 기준으로 정렬되는 방식 채용

        private void Awake()
        {
            clueData.LoadData();
            foreach (var clue in clueData.clueCollection)
            {
                for (int i = 0; i < clue.Value.DescriptionLength; i++)
                {
                    AddClue(clue.Key, i);
                }
            }
        }

        public void AddClue(string id, int descriptionLevel = 0)
        {
            var unIdentifiedClue = clueData.GetClue(id);

            if (!ownClueContainer.ContainsKey(id))
            {
                // Add Empty Clue(except description)
                if (unIdentifiedClue is Clue clue)
                {
                    var newClue = Clue.Instantiate(clue.Id, clue.Name, clue.Tags, clue.linkedClueId, clue.DescriptionLength);
                    ownClueContainer[id] = newClue;
                }
                else if (unIdentifiedClue is ClueRecipe recipe)
                {
                    var newClue = ClueRecipe.CreateRecipe(recipe.Id, recipe.Name, recipe.Tags, recipe.clueIds, recipe.DescriptionLength);
                    ownClueContainer[id] = newClue;
                }

                Debug.Log("InferenceManager: " + ownClueContainer[id].Name + " Clue Added");
            }

            // Add Description
            ownClueContainer[id].Description[descriptionLevel] = unIdentifiedClue.Description[descriptionLevel];
            Debug.Log("InferenceManager: " + ownClueContainer[id].Description[descriptionLevel] + " Description Added");

        }

        public IClue GetClue(string id) => ownClueContainer[id];

        // 카테고리 UI 버튼을 눌렀을 때 해당하는 목록만 활성화하는 방식을 사용
        // 특정 태그로 요청하면 해당 태그에 맞는 데이터 반환
        public List<IClue> GetClassifiedClueDataByTag(string tag)
        {
            var result = new List<IClue>();
            foreach (var clue in ownClueContainer.Values)
            {
                Debug.Log("GetClue: " + clue);
                Debug.Log($"GetClue: TagCount: {clue.Tags.Length} Tags: {string.Join(", ", clue.Tags)}");
                for (global::System.Int32 i = 0; i < clue.Tags.Length; i++)
                {
                    if (clue.Tags[i] == tag)
                    {
                        result.Add(clue);
                        Debug.Log($"InferenceManager: Searched ID: {tag}, ID: {clue.Name} Tag: {clue.Tags[i]} added");
                    }
                }
            }

            if (result.Count <= 0)
            {
                Debug.LogWarning($"ClueClassification: Check the Tag: {tag}, this tag is not yet exsist");
            }

            return result;
        }

        public List<string> GetClassifiedClueIDByTag(string tag)
        {
            var result = new List<string>();
            foreach (var clue in ownClueContainer.Values)
            {
                for (global::System.Int32 i = 0; i < clue.Tags.Length; i++)
                {
                    if (clue.Tags[i] == tag)
                        result.Add(clue.Id);
                }
            }

            if (result.Count <= 0)
            {
                Debug.LogWarning($"ClueClassification: Check the Tag: {tag}, this tag is not yet exsist");
            }

            return result;
        }

        public void AddCombinedClue(string name, List<string> ids)
        {
            ownClueContainer.Add(name, ClueRecipe.CreateRecipe(UniqueIDGenerator.GenerateUniqueId(name), name, new string[] { "Sentence" }, ids, 0));
        }

        [ContextMenu("보유한 단서 출력")]
        private void PrintOwnClues()
        {
            Debug.Log(string.Join(", ", ownClueContainer.Keys));
        }

        /*

        public void SelectClue(string tag, Clue selectedClue)
        {
            //var newRecipe = new ClueRecipe();
            //newRecipe.clueIds.Add(tag);
        }

        // CombineClues to Sentence
        public void CombineClues()
        {
            // clue를 리스트로 갖고 있어야함, 이 리스트를 하나로 관리하는 리스트가 필요함
            createdClueList.Add(createdClueList.Count+1, selectedClueList);
            selectedClueList.Clear();
        }

        public Dictionary<string, Clue> GetCombinedClue(int index)
        {
            return createdClueList[index];
        }

        // Modify Clues
        public void ModifyCombinedClue(int index, string tag, Clue newCombinedClue)
        {
            createdClueList[index][tag] = newCombinedClue;
        }

        public void RemoveCombinedClue(int index)
        {
            createdClueList.Remove(index);
        }
        */
    }
}
