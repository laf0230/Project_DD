using Assets.script.Talk_System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUIHandler : MonoBehaviour
{
    [Header("UI Containers")]
    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private Transform selectionContainer;

    // 좌/우 컨테이너를 각각 지정할 수 있도록 변수 분리
    [SerializeField] private Transform leftCharacterContainer;
    [SerializeField] private Transform rightCharacterContainer;

    [Header("Prefabs")]
    [SerializeField] private DialogueUI dialogueUIPrefab;
    [SerializeField] private TalkSelectionUI selectionUIPrefab;
    [SerializeField] private TalkCharacterUI characterImageUIPrefab;

    private DialogueUI dialogueUI;
    private TalkSelectionUI[] selectionUIs = new TalkSelectionUI[5];

    // 배열 대신 명시적인 변수로 관리하거나, 인덱스로 관리
    private TalkCharacterUI leftCharacterUI;
    private TalkCharacterUI rightCharacterUI;
    public void Initialize()
    {
        // 대화 UI 생성
        dialogueUI = Instantiate(dialogueUIPrefab, dialogueContainer.transform, false);
        dialogueUI.gameObject.SetActive(false);

        // 선택지 UI 생성
        for (int i = 0; i < selectionUIs.Length; i++)
        {
            selectionUIs[i] = Instantiate(selectionUIPrefab, selectionContainer, false);
            selectionUIs[i].gameObject.SetActive(false);
        }

        // 캐릭터 이미지 UI를 각각의 컨테이너에 생성
        if (leftCharacterContainer != null)
        {
            leftCharacterUI = Instantiate(characterImageUIPrefab, leftCharacterContainer, false);
            leftCharacterUI.gameObject.SetActive(false);
        }

        if (rightCharacterContainer != null)
        {
            rightCharacterUI = Instantiate(characterImageUIPrefab, rightCharacterContainer, false);
            rightCharacterUI.gameObject.SetActive(false);
        }
    }
    public void SetActiveDialogueWindow(bool isActive)
    {
        dialogueContainer.SetActive(isActive);
        dialogueUI.gameObject.SetActive(isActive);
        if (!isActive) HideAllCharacters();
    }

    public void UpdateDialogue(string name, string description)
    {
        dialogueUI.SetData(name, description);
    }
    public void UpdateCharacterImages(Sprite leftSprite, Sprite rightSprite)
    {
        // 왼쪽 캐릭터 업데이트
        if (leftCharacterUI != null)
        {
            bool hasLeft = leftSprite != null;
            leftCharacterUI.gameObject.SetActive(hasLeft);
            if (hasLeft) leftCharacterUI.Sprite = leftSprite;
        }

        // 오른쪽 캐릭터 업데이트
        if (rightCharacterUI != null)
        {
            bool hasRight = rightSprite != null;
            rightCharacterUI.gameObject.SetActive(hasRight);
            if (hasRight) rightCharacterUI.Sprite = rightSprite;
        }
    }

    public void ShowSelections(List<DialogueSelection> selections, UnityEngine.Events.UnityAction<int> onSelected)
    {
        ClearSelections();
        for (int i = 0; i < selections.Count && i < selectionUIs.Length; i++)
        {
            selectionUIs[i].SelectionText = selections[i].text;
            selectionUIs[i].gameObject.SetActive(true);

            // 기존 리스너 제거 후 새로 등록 (인덱스 전달을 위해)
            selectionUIs[i].OnSelected.RemoveAllListeners();
            int index = i;
            selectionUIs[i].OnSelected.AddListener(() => onSelected.Invoke(index));
        }
    }

    public void ClearSelections()
    {
        foreach (var ui in selectionUIs) ui.gameObject.SetActive(false);
    }

    private void HideAllCharacters()
    {
        if (leftCharacterUI != null) leftCharacterUI.gameObject.SetActive(false);
        if (rightCharacterUI != null) rightCharacterUI.gameObject.SetActive(false);
    }
}