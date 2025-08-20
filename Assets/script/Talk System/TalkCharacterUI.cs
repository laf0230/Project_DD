using UnityEngine;
using UnityEngine.UI;

public class TalkCharacterUI : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    public Sprite Sprite
    {
        get
        {
            if(characterImage == null)
            {
                Debug.LogWarning("CharacterImage is Empty.");
                return null;
            }
            return characterImage.sprite;
        }

        set
        {
            if(characterImage == null)
            {
                Debug.LogWarning("CharacterImage is Empty.");
            }
            characterImage.sprite = value;
        }
    }
}
