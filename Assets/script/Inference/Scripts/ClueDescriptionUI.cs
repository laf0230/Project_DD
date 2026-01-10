using TMPro;
using UnityEngine;

public class ClueDescriptionUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public bool isActive { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

    public void SetText(string desc)
    {
        text.text = desc;
    }
}
