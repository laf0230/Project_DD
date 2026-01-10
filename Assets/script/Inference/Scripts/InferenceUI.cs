using Inference;
using ServiceLocator;
using StarterAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InferenceUI : MonoBehaviour
{
    [SerializeField] InferenceManager manager;
    [SerializeField] ClueUI clueUiPrefab;
    [SerializeField] Transform clueUiContainer; // 분류된 단서 UI 부모
    [SerializeField] Dictionary<string, ClueUI> clueUis = new();
    [SerializeField] List<UIContainer> uis = new();
    [SerializeField] InferenceAlertUI alertUi;

    [Header("Inference Note")]
    [SerializeField] Transform inferenceNoteClueUiContainer;
    [SerializeField] Transform inferenceNoteDescriptionContainer;
    [SerializeField] GameObject inferenceNoteSelectablePrefab;
    [SerializeField] int selectedClueLimit = 4;
    [SerializeField] TextMeshProUGUI previewResult;
    Dictionary<int, IClue> selectedClue = new Dictionary<int, IClue>();
    Dictionary<string, ClueUI> inferenceNoteClueUis = new();

    private string combinedClueName;
    private Button selectedButton;
    private string selectedTag;
    private int currentOrder;
    private string lastOpendUIName;

    private void Awake()
    {
        Locator.Subscribe(this);
    }

    private void Start()
    {
        lastOpendUIName = uis[0].name;
        //OpenUI(lastOpendUIName);
    }

    public void OpenUI()
    {
        clueUiContainer.gameObject.SetActive(true);
        OpenUI(lastOpendUIName);
    }

    public void OpenUI(string uiName)
    {
        Locator.Get<PlayerController>(this).UpdateState(PlayerState.Conversation);

        for(int i=0; i < uis.Count; i++)
        {
            if (uis[i].name == uiName)
            {
                uis[i].ui.gameObject.SetActive(true);
                lastOpendUIName = uiName;
            }
            else
                uis[i].ui.gameObject.SetActive(false);
        }
    }

    public void CloseUI()
    {
        Locator.Get<PlayerController>(this).UpdateState(PlayerState.Investigation);
        clueUiContainer.gameObject.SetActive(false);
    }

    public void SetClueName(string clueName) => combinedClueName = clueName;

    // if Push the button
    // Complete 카테고리 UI 제작 후 버튼을 통한 호출로 목록 업데이트
    public void ClassificationClueUI(string categoryTag)
    {
        var clues = manager.GetClassifiedClueDataByTag(categoryTag);
        HashSet<string> activeIds = new HashSet<string>();

        // Create and Init
        foreach(var clue in clues)
        {
            if (!clueUis.ContainsKey(clue.Id))
            {
                var clueUIObject = Instantiate(clueUiPrefab, clueUiContainer);
                clueUis[clue.Id] = clueUIObject;
            }

            var clueUI = clueUis[clue.Id];
            clueUI.isActive = true;
            clueUI.Clue = clue;

            activeIds.Add(clue.Id);
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
        selectedTag = categoryTag;

        var clues = manager.GetClassifiedClueDataByTag(categoryTag);
        HashSet<string> activeIds = new HashSet<string>();

        // Create and Init
        foreach(var clue in clues)
        {
            if (!inferenceNoteClueUis.ContainsKey(clue.Id))
            {
                var clueUIObject = Instantiate(clueUiPrefab, inferenceNoteClueUiContainer);
                inferenceNoteClueUis[clue.Id] = clueUIObject;
                clueUIObject.onSelected.AddListener(OnSelectedClueUI);
            }

            var clueUI = inferenceNoteClueUis[clue.Id];
            clueUI.isActive = true;
            clueUI.Clue = clue;
            clueUI.onSelected = null;

            activeIds.Add(clue.Id);
        }

        // Disable: has to disabled uis
        foreach(var kvp in inferenceNoteClueUis)
        {
            if(!activeIds.Contains(kvp.Key))
                kvp.Value.isActive = false;
        }
    }

    public void SetTagOrder(int order) => currentOrder = order;

    public void OnSelectedClueUI(ClueUI clueUI)
    {
        if(selectedButton == null) return;

        selectedClue[currentOrder] = clueUI.Clue;
    }

    public void CombineClues()
    {
        if(selectedClue.Count < selectedClueLimit)
        {
            Debug.LogWarning("조합에 필요한 단서가 모자릅니다.");
        }
        else
        {
            List<string> result = new();
            foreach (var clue in selectedClue.Values) 
            {
                result.Add(clue.Id);
            }
            manager.AddCombinedClue(combinedClueName, result);
        }
    }

    public void ActiveAlertUI(IClue clue)
    {
        alertUi.ActiveAlert(clue);
    }

    [System.Serializable]
    public class UIContainer
    {
        public string name;
        public Transform ui;
    }
}
