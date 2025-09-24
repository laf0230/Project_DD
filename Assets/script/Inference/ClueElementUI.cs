using Inference;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClueElementUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI clueName;

    public Transform DescriptionContainer { get; set; }
    private IClue clue;
    public IClue Clue { get => clue; set => SetData(value); }
    public bool isActive { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    public UnityEvent OnClick => button?.onClick;

    void SetData(IClue clue)
    {
        if(this.clue != clue)
            this.clue = clue;

        // Clue Name
        clueName.text = clue.Name;
    }
}
