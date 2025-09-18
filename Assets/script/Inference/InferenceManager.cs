using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inference
{
    public class InferenceManager : MonoBehaviour
    {
        public ClueDataStroage clueData;
        public Dictionary<string, Clue> ownedClueContainer = new();
        public Dictionary<string, Clue> selectedClueList;

        public CombinedClueDataStroage combinedClueData;
        public List<ClueRecipe> createdClueList;

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
            foreach (var clue in clueData.clues)
            {
                for (int i = 0; i < 3; i++)
                {
                    AddClue(clue.id);
                }
            }
        }

        public void AddClue(string id, int descriptionLevel = 0)
        {
            var clue = clueData.GetClue(id);

            if (!ownedClueContainer.ContainsKey(id))
            {
                // Add Empty Clue(except description)
                var newClue = Clue.Instantiate(clue.id, clue.name, clue.tags, clue.linkedClueId, clue.descriptionLength);
                ownedClueContainer[id] = newClue;
                Debug.Log("InferenceManager: " + ownedClueContainer[id].name);
            }

            // Add Description
            ownedClueContainer[id].description[descriptionLevel] = clue.description[descriptionLevel];
            Debug.Log("InferenceManager: " + ownedClueContainer[id].description[descriptionLevel] + " Description Added");
        }

        public Clue GetClue(string id) => ownedClueContainer[id];

        // 카테고리 UI 버튼을 눌렀을 때 해당하는 목록만 활성화하는 방식을 사용
        // 특정 태그로 요청하면 해당 태그에 맞는 데이터 반환
        public List<Clue> GetClassifiedClueDataByTag(string tag)
        {
            var result = new List<Clue>();
            foreach (var clue in ownedClueContainer.Values)
            {
                for (global::System.Int32 i = 0; i < clue.tags.Length; i++)
                {
                    if (clue.tags[i] == tag)
                        result.Add(clue);
                }
            }

            if(result.Count <= 0)
            {
                Debug.LogWarning($"ClueClassification: Check the Tag: {tag}, this tag is not yet exsist");
            }

            return result;
        }

        public List<string> GetClassifiedClueIDByTag(string tag)
        {
            var result = new List<string>();
            foreach (var clue in ownedClueContainer.Values)
            {
                for (global::System.Int32 i = 0; i < clue.tags.Length; i++)
                {
                    if (clue.tags[i] == tag)
                        result.Add(clue.id);
                }
            }

            if(result.Count <= 0)
            {
                Debug.LogWarning($"ClueClassification: Check the Tag: {tag}, this tag is not yet exsist");
            }

            return result;
        }

        public void AddCombinedClue(List<string> ids)
        {
            createdClueList.Add(ClueRecipe.Combine(ids));
        }

        public ClueRecipe GetCombinedClue(List<string> ids)
        {
            foreach (var recipe in createdClueList)
            {
                if (recipe.clueIds.Count != ids.Count)
                    continue;

                bool allMatch = true;
                for (int i = 0; i < ids.Count; i++)
                {
                    if (recipe.clueIds[i] != ids[i])
                    {
                        allMatch = false;
                        break;
                    }
                }

                if (allMatch)
                    return recipe;
            }

            Debug.LogErrorFormat($"InferenceManager: Couldn't found Recipe. Ids = {0}", string.Join(", ", ids));
            return null;
        }

        public void ModifyCombinedClue(int index, List<string> ids, string id)
        {
            // Todo Modify CombinedClue ata
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
