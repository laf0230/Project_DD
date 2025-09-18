using Inference;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueUI : MonoBehaviour
{
    [SerializeField] InferenceManager manager;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI clueName;
    [SerializeField] private Transform descriptionContainer;
    [SerializeField] private ClueDescriptionUI descriptionPrefab;
    [SerializeField] private List<ClueDescriptionUI> descriptionUis;

    [SerializeField] private Clue clue;
    public Clue Clue { get => clue; set => SetData(value); }
    public bool isActive { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

    void SetData(Clue clue)
    {
        if(this.clue != clue)
            this.clue = clue;

        // Clue Name
        clueName.text = clue.name;

        // Replace Description Data
        for (int i = 0; i < clue.descriptionLength; i++)
        {
            if (descriptionUis.Count <= i)
            {
                var descriptionObject = Instantiate(descriptionPrefab, descriptionContainer);
                descriptionUis.Add(descriptionObject);
            }
            descriptionUis[i].isActive = true;

            descriptionUis[i].SetText(clue.description[i]);
        }

        for(int i = clue.descriptionLength; i < descriptionUis.Count; i++)
        {
            descriptionUis[i].isActive = false;
        }
    }
}
