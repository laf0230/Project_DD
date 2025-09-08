using Assets.script.Talk_System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TalkSelectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI selectionText;
    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private Button btn;

    public UnityEvent OnSelected => onSelected;

    public string SelectionText
    {
        get
        {
            if (selectionText == null)
            {
                Debug.LogWarning("getter:SelectionText Text is null");
                return null;
            }
            return selectionText.text;
        }
        set
        {
            if (selectionText == null)
            {
                Debug.LogWarning("setter:SelectionText Text is null");
            }
            else
            {
                selectionText.text = value;
            }
        }
    }

    private void Start()
    {
        btn.onClick.AddListener(() => OnSelected.Invoke());
    }

    public void SetData(TalkSelection data)
    {
        SelectionText = data.selectionText;
        onSelected = data.action;
    }
}
