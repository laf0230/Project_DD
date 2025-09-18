using Inference;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InferenceUI : MonoBehaviour
{
    [SerializeField] InferenceManager manager;
    [SerializeField] ClueUI clueUiPrefab;
    [SerializeField] Transform clueUiContainer; // 분류된 단서 UI 부모
    [SerializeField] Dictionary<string, ClueUI> clueUis = new();

    [Header("Inference Note")]
    [SerializeField] ClueUI inferenceNoteUI;
    [SerializeField] Transform inferenceNoteClueUiContainer;
    [SerializeField] Dictionary<string, Clue> selectedClue;

    private void Start()
    {
        //UpdateUI("all");
    }

    // if Push the button
    // Complete 카테고리 UI 제작 후 버튼을 통한 호출로 목록 업데이트
    public void ClassificationClueUI(string categoryTag)
    {
        var clues = manager.GetClassifiedClueDataByTag(categoryTag);
        HashSet<string> activeIds = new HashSet<string>();

        // Create and Init
        foreach(var clue in clues)
        {
            if (!clueUis.ContainsKey(clue.id))
            {
                var clueUIObject = Instantiate(clueUiPrefab, clueUiContainer);
                clueUis[clue.id] = clueUIObject;
            }

            var clueUI = clueUis[clue.id];
            clueUI.isActive = true;
            clueUI.Clue = clue;

            activeIds.Add(clue.id);
        }

        // Disable: has to disabled uis
        foreach(var kvp in clueUis)
        {
            if(!activeIds.Contains(kvp.Key))
                kvp.Value.isActive = false;
        }
    }

    public void ClassificationInferenceNoteUI(string categoryTag)
    {
        var clues = manager.GetClassifiedClueDataByTag(categoryTag);
        HashSet<string> activeIds = new HashSet<string>();

        // Create and Init
        foreach(var clue in clues)
        {
            if (!clueUis.ContainsKey(clue.id))
            {
                var clueUIObject = Instantiate(clueUiPrefab, inferenceNoteClueUiContainer);
                clueUis[clue.id] = clueUIObject;
            }

            var clueUI = clueUis[clue.id];
            clueUI.isActive = true;
            clueUI.Clue = clue;

            activeIds.Add(clue.id);
        }

        // Disable: has to disabled uis
        foreach(var kvp in clueUis)
        {
            if(!activeIds.Contains(kvp.Key))
                kvp.Value.isActive = false;
        }
    }
}
