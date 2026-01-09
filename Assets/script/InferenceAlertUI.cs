using Inference;
using ServiceLocator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InferenceAlertUI : MonoBehaviour
{
    public Transform container;
    public Image clueImage;
    public TextMeshProUGUI clueName;
    public TextMeshProUGUI clueDescription;

    public void ActiveAlert(IClue clue)
    {
        Debug.Log("Inference Alert Actived");
        container.gameObject.SetActive(true);
        Locator.Get<PlayerController>(this).UpdateState(PlayerState.Conversation);

        clueName.text = clue.Name;
        var clueDescriptionText = clue.Description;
        string[] finalValue = new string[clue.DescriptionLength];

        for(int i = 0; i < clueDescriptionText.Length; i++)
        {
            finalValue[i] = clueDescriptionText[i].description;
        }

        clueDescription.text = string.Concat("\n", finalValue);
    }

    public void DisableUI()
    {
        Debug.Log("Inference Disabled");
        Locator.Get<PlayerController>(this).UpdateState(PlayerState.Investigation);
        container.gameObject.SetActive(false);
    }
}
