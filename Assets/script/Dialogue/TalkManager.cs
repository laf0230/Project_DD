using UnityEngine;
using UnityEngine.InputSystem;
using Talk;
using ServiceLocator;
using StarterAssets;
using Assets.script.Talk_System;

public class TalkManager : MonoBehaviour
{
    public static TalkManager Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private DialogueDataSO dialogueData;
    [SerializeField] private CharacterInfoSO characterSpriteData;

    [Header("Handlers")]
    [SerializeField] private DialogueUIHandler uiHandler;

    private PlayerController playerController;
    private ITalkableObject currentTalkable;
    private DialogueNode currentNode;

    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private bool isSelectionMode = false;

    private void Awake()
    {
        Instance = this;
        Locator.Subscribe(this);
    }

    private void Start()
    {
        uiHandler.Initialize();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    public void ActiveDialogue(ITalkableObject talkable, string dialogueID)
    {
        currentTalkable = talkable;
        currentNode = dialogueData.GetDialogue(dialogueID);

        if (currentNode == null)
        {
            Debug.LogError($"Dialogue ID {dialogueID}를 찾을 수 없습니다.");
            return;
        }

        currentIndex = 0;
        isDialogueActive = true;
        isSelectionMode = false;

        playerController.UpdateState(PlayerState.Conversation);
        uiHandler.SetActiveDialogueWindow(true);

        DisplayCurrentLine();
    }

    public void OnNext(InputAction.CallbackContext callback)
    {
        if (!isDialogueActive || isSelectionMode) return;

        if (callback.phase == InputActionPhase.Started)
        {
            NextDialogue();
        }
    }

    // TalkManager의 NextDialogue 핵심 로직 재확인
    public void NextDialogue()
    {
        if (!isDialogueActive || isSelectionMode) return;

        currentIndex++;

        if (currentIndex < currentNode.talkLineList.Count)
        {
            DisplayCurrentLine();
        }
        else
        {
            CheckForSelections();
        }
    }
    private void DisplayCurrentLine()
    {
        var currentLine = currentNode.talkLineList[currentIndex];

        // UI 텍스트 갱신
        uiHandler.UpdateDialogue(currentLine.speakerData.name, currentLine.text);

        // 캐릭터 이미지 갱신
        Sprite left = characterSpriteData.GetCharacterSprite(currentLine.speakerData.id);
        Sprite right = characterSpriteData.GetCharacterSprite(currentLine.otherSpeakerData.id);
        uiHandler.UpdateCharacterImages(left, right);
    }

    private void CheckForSelections()
    {
        if (currentNode.talkSelectionList != null && currentNode.talkSelectionList.Count > 0)
        {
            isSelectionMode = true;
            uiHandler.ShowSelections(currentNode.talkSelectionList, (index) => {
                // 선택지 클릭 시 로직 (여기서는 대화 종료로 설정되어 있음)
                EndDialogue();
            });
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        isSelectionMode = false;
        currentIndex = 0;

        uiHandler.ClearSelections();
        uiHandler.SetActiveDialogueWindow(false);

        playerController.UpdateState(PlayerState.Investigation);
        if (currentTalkable != null) currentTalkable.EndDialogue();
    }
}
