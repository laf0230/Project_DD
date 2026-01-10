using Inference;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClueUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI clueName;
    [SerializeField] private Transform descriptionContainer;
    [SerializeField] private ClueDescriptionUI descriptionPrefab;
    [SerializeField] private List<ClueDescriptionUI> descriptionUis;

    [SerializeField] private IClue clue;
    public UnityEvent<ClueUI> onSelected;

    public IClue Clue { get => clue; set => SetData(value); }
    public bool isActive { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

    void SetData(IClue clue)
    {
        if(this.clue != clue)
            this.clue = clue;

        // Clue Name
        clueName.text = clue.Name;

        // Replace Description Data
        for (int i = 0; i < clue.DescriptionLength; i++)
        {
            if (descriptionUis.Count <= i)
            {
                var descriptionObject = Instantiate(descriptionPrefab, descriptionContainer);
                descriptionUis.Add(descriptionObject);
            }
            descriptionUis[i].isActive = true;

            descriptionUis[i].SetText(clue.Description[i].description);
        }

        for(int i = clue.DescriptionLength; i < descriptionUis.Count; i++)
        {
            descriptionUis[i].isActive = false;
        }
    }

    public void OnSelected() => onSelected?.Invoke(this);
}
