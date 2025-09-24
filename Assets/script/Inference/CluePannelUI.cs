using Inference;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CluePannelUI : MonoBehaviour
{
    [SerializeField] InferenceManager manager;
    [SerializeField] Transform clueContainer;
    [SerializeField] ClueElementUI elementPrefab;
    [SerializeField] List<ClueElementUI> clueElementUIs = new List<ClueElementUI>();
    [SerializeField] Transform descriptionContainer;
    [SerializeField] ClueDescriptionUI descriptionUIPrefab;
    [SerializeField] List<ClueDescriptionUI> descriptionUis = new List<ClueDescriptionUI>();

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
}
