using Inference;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InferenceNotePannelUI : MonoBehaviour
{
    [SerializeField] InferenceManager manager;
    [SerializeField] Transform clueContainer;
    [SerializeField] ClueElementUI elementPrefab;
    [SerializeField] List<ClueElementUI> clueElementUIs = new List<ClueElementUI>();
    [SerializeField] Transform descriptionContainer;
    [SerializeField] ClueDescriptionUI descriptionUIPrefab;
    [SerializeField] List<ClueDescriptionUI> descriptionUis = new List<ClueDescriptionUI>();
    [SerializeField] TextMeshProUGUI previewTMP;

    private string sentenceName = "";
    private IClue[] selectedClues = new IClue[4];
    private IClue selectedClue;
    private int order;

    public void Open()
    {
        gameObject.SetActive(true);
        ResetUI();
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClassifyButtonClicked(string tag)
    {
        var result = manager.GetClassifiedClueDataByTag(tag);

        // 부족하면 생성
        while (clueElementUIs.Count < result.Count)
        {
            var newElement = Instantiate(elementPrefab, clueContainer);
            clueElementUIs.Add(newElement);
        }

        // 결과 개수만큼 활성화 및 데이터 반영
        for (int i = 0; i < result.Count; i++)
        {
            clueElementUIs[i].gameObject.SetActive(true);
            clueElementUIs[i].Clue = result[i];
            clueElementUIs[i].OnClick.RemoveAllListeners();

            ClearAllDescriptionUI();

            var descriptions = result[i].Description;
            clueElementUIs[i].OnClick.AddListener(() => SetDescription(descriptions));
        }

        // 남는 UI 비활성화
        for (int i = result.Count; i < clueElementUIs.Count; i++)
        {
            clueElementUIs[i].gameObject.SetActive(false);
        }
    }

    void SetDescription(ClueDescription[] descriptions)
    {
        Debug.Log("CluePannelUI::Result Length: " + descriptions.Length);

        while (descriptionUis.Count < descriptions.Length)
        {
            var description = Instantiate(descriptionUIPrefab, descriptionContainer);

            descriptionUis.Add(description);
        }

        for (int i = 0; i < descriptions.Length; i++)
        {
            var ui = descriptionUis[i];
            ui.isActive = true;

            Debug.Log($"CluePannelUI::SetDescription:[{i}] = {descriptions[i].description}");
            ui.SetText(descriptions[i].description);
        }

        for (int i = descriptions.Length; i < descriptionUis.Count; i++)
        {
            descriptionUis[i].isActive = false;
        }
    }

    void ClearAllDescriptionUI()
    {
        for (int i = 0; i < descriptionUis.Count; i++)
        {
            descriptionUis[i].gameObject.SetActive(false);
        }
    }

    // Used By Sentence Name Field
    public void SetSentenceName(string name) => sentenceName = name;

    // Used By Slot Button
    public void SetSentenceOrder(int order) => this.order = order;

    // Used By Clue Element UI
    public void SetSelectedClue(IClue clue)
    {
        selectedClue = clue;
        selectedClues[order] = clue;
    }

    // Used By Slot Button
    public void OnSlotButtonClicked(string tag)
    {
        var result = manager.GetClassifiedClueDataByTag(tag);

        // 부족하면 생성
        while (clueElementUIs.Count < result.Count)
        {
            var newElement = Instantiate(elementPrefab, clueContainer);
            clueElementUIs.Add(newElement);
        }

        // 결과 개수만큼 활성화 및 데이터 반영
        for (int i = 0; i < result.Count; i++)
        {
            clueElementUIs[i].gameObject.SetActive(true);
            clueElementUIs[i].Clue = result[i];
            clueElementUIs[i].OnClick.RemoveAllListeners();

            ClearAllDescriptionUI();

            var clue = result[i];
            clueElementUIs[i].OnClick.AddListener(() => SetDescription(clue.Description));
            clueElementUIs[i].OnClick.AddListener(() => SetSelectedClue(clue));
            clueElementUIs[i].OnClick.AddListener(SetPreviewText);
        }

        // 남는 UI 비활성화
        for (int i = result.Count; i < clueElementUIs.Count; i++)
        {
            clueElementUIs[i].gameObject.SetActive(false);
        }
    }

    // Used By Clue Element UI
    public void SetPreviewText()
    {
        previewTMP.text = $"{selectedClues[0]?.Name}가 {selectedClues[1]?.Name}에게 {selectedClues[2]?.Name}에서 {selectedClues[3]?.Name}했다.";
    }

    public void OnCombineButtonClicked()
    {
        for (int i = 0; i < selectedClues.Length; i++)
        {
            if (selectedClues[i] == null)
            {
                Debug.LogError("InferenceNotePannelUI::OnCombineButtonClicked: 비어있는 단서가 있습니다.");
                return;
            }
        }

        string[] ids = new string[selectedClues.Length];

        for (int i = 0; i < selectedClues.Length; i++) 
        {
            ids[i] = selectedClues[i].Id;
        }

        manager.AddCombinedClue(sentenceName, ids.ToList());

        // Reset Combine UIs
        ResetUI();
    }

    private void ResetUI()
    {
        Array.Clear(selectedClues, 0, selectedClues.Length);
        selectedClue = null;
        SetPreviewText();
    }
}
