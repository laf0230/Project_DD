using Assets.script.Talk_System;
using NUnit.Framework.Constraints;
using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    [Header("대화 데이터/캐릭터 데이터")]
    [SerializeField] private TalkCollectionSO talkData;
    [SerializeField] private CharacterInfoSO characterSpriteListData;

    [Header("UI 데이터")]
    [SerializeField] private Transform dialogueContainer;
    [SerializeField] private DialogueUI dialogueUIPrefab;
    [SerializeField] private DialogueUI dialogueUI;

    [Header("선택지 데이터")]
    [SerializeField] private Transform dialogueSelectionContainer;
    [SerializeField] private TalkSelectionUI selectionUIPrefab;
    [SerializeField] private TalkSelectionUI[] selectionUIs = new TalkSelectionUI[5];
    private int activatedSelectionCount = 0;

    [Tooltip("캐릭터의 이미지가 보여지는 부분에 대한 데이터")]
    [Header("캐릭터 이미지 보관 데이터")]
    [SerializeField] private Transform characterImageContainer;
    [SerializeField] private TalkCharacterUI characterImageUIPrefab;
    [SerializeField] private TalkCharacterUI[] characterImageList;

    private TalkNode currentNode;
    private TalkLine currentLine;
    private PlayerController playerController;
    private int currentIndex = 0;
    private bool isSelectionActivated = false;
    private bool isDialogue = false;

    private static TalkManager instance;

    public static TalkManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        Debug.Log(instance);
    }

    private void Start()
    {
        if(dialogueContainer == null)
        {
            Debug.LogWarning("dialogue Container is empty");
        }

        dialogueUI = Instantiate(dialogueUIPrefab, dialogueContainer, false);
        dialogueUI.gameObject.SetActive(false);
        //dialogueUI.OnPointerUp.AddListener(NextDialogue);

        for (int i = 0; i < selectionUIs.Length; i++)
        {
            selectionUIs[i] = Instantiate(selectionUIPrefab, dialogueSelectionContainer, false);
            selectionUIs[i].gameObject.SetActive(false);
            selectionUIs[i].OnSelected.AddListener(EndDialogue);
        }

        for(var i = 0; i < characterImageList.Length; i++)
        {
            var obj = Instantiate(characterImageUIPrefab, characterImageContainer, false);

            obj.gameObject.SetActive(false);
            characterImageList[i] = obj;
        }

        //ActiveDialogue("Sample");
    }

    public void ActiveDialogue(string dialogueID)
    {
        playerController = FindAnyObjectByType<PlayerController>();
        playerController.UpdateState(PlayerState.Conversation);
        currentIndex = 0;
        isSelectionActivated = false;
        isDialogue = true;

        var nodes = talkData.TalkNodes;

        foreach (var node in nodes)
        {
            if (node.ID == dialogueID)
            {
                dialogueUI.gameObject.SetActive(true);
                currentNode = node;
                currentLine = currentNode.talkLineList[currentIndex];
                dialogueUI.Name = currentLine.speakerData.name;
                dialogueUI.Description = currentLine.talkText;
                VisualizeCharacter();
                return;
            }
        }

        Debug.LogError("Couldn't find id matched dialogue");
    }

    private void VisualizeCharacter()
    {
        InitCharacterImage();

        SetCharacterImage(currentLine.speakerData);

        foreach(var other in currentLine.otherDataList)
        {
            SetCharacterImage(other);
        }
    }

    private void SetCharacterImage(EmotionData data)
    {
        int characterIndex = characterSpriteListData.characterSpriteList.FindIndex(c => c.name == data.name);

        if (characterIndex == -1 || characterIndex >= characterImageList.Length) return;

        var spriteData = characterSpriteListData.characterSpriteList[characterIndex];

        int emotionIndex = spriteData.spriteList.FindIndex(s => s.name == data.emotionName);
        if(emotionIndex == -1)
        {
            Debug.LogError($"Cant find character emotion: {data.name}");
            return;
        }
        Sprite emotionSprite = spriteData.spriteList.Find(s => s.name == data.emotionName);
        if(emotionSprite == null) return;

        characterImageList[characterIndex].Sprite = emotionSprite;
        characterImageList[characterIndex].gameObject.SetActive(true);
    }

    private void InitCharacterImage()
    {
        foreach (var c in characterImageList)
        {
            c.gameObject.SetActive(false);
        }
    }

    private void VisibleSelection()
    {
        isSelectionActivated = true;

        var selectionCount = currentNode.talkSelectionList.Count;
        if(activatedSelectionCount > selectionCount)
        {
            // 요구되는 선택지가 활성화된 선택지보다 많을 경우 추가 활성화
            for(int i = selectionCount; i < activatedSelectionCount; i++)
            {
                selectionUIs[i].gameObject.SetActive(false);
            }
        } else if(activatedSelectionCount < selectionCount)
        {
            // 활성화된 선택지가 요구되는 선택지보다 많을 경우 비활성화
            for(int i = activatedSelectionCount; i < selectionCount; i++)
            {
                selectionUIs[i].gameObject.SetActive(true);
            }
        }

        // 선택지 데이터 업데이트
        for (int i = 0; i < selectionCount; i++)
        {
            selectionUIs[i].SelectionText = currentNode.talkSelectionList[i].selectionText;
        };

        // 활성화된 선택지 수 갱신
        activatedSelectionCount = selectionCount;
    }

    public void OnNext(InputAction.CallbackContext callback)
    {
        if (!isDialogue) return;
        if(callback.phase == InputActionPhase.Started)
        {
            NextDialogue();
        }
    }

    public void NextDialogue()
    {
        // 선택지가 이미 활성화되어 있을 경우 예외처리
        currentIndex++;

        if (isSelectionActivated)
            return;

        if(currentNode.talkLineList.Count-1 < currentIndex)
        {
            // 다음 대화가 없을 경우 선택지 출력
            if(currentNode.talkSelectionList.Count-1 > 0)
            {
                // 선택지 출력
                VisibleSelection();
            }
            else
            {
                // 선택지가 없을 경우
                EndDialogue();
            }
        }
        else
        {
            VisualizeCharacter();

            // 다음 대사 출력
            currentLine = currentNode.talkLineList[currentIndex];

            // Todo - 조건에 맞게 TalkLine을 추가/제거를 해야함

            dialogueUI.Name = currentLine.speakerData.name;
            dialogueUI.Description = currentLine.talkText;
        }
    }

    public void EndDialogue()
    {
        playerController.UpdateState(PlayerState.Investigation);
        dialogueContainer.gameObject.SetActive(false);
        isDialogue = false;

        // Todo 대사 종료 후 트리거 업데이트
    }
}
