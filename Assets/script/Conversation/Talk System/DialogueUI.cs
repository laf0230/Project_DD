using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private UnityEvent onPointerUp;

    public TextMeshProUGUI NameText { get => nameText; }
    public TextMeshProUGUI DescriptionText { get => descriptionText; }
    public UnityEvent OnPointerUp => onPointerUp;

    public string Name
    {
        get
        {
            if (nameText == null)
            {
                Debug.LogWarning("nameText is not exist");
                return null;
            }
            return nameText.text;
        }
        set
        {
            if (nameText == null)
            {
                Debug.LogWarning("nameText is not exist");
            }
            else
            {
                nameText.text = value;
            }
        }
    }

    public string Description
    {
        get
        {
            if (descriptionText == null)
            {
                Debug.LogWarning("getter: DescriptionText is null");
                return null;
            }
            return descriptionText.text;
        }
        set
        {
            if (descriptionText == null)
            {
                Debug.LogWarning("setter: DescriptionText is null");
            }
            else
            {
                descriptionText.text = value;
            }
        }
    }
}