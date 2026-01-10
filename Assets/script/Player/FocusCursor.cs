using ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

public class FocusCursor: MonoBehaviour
{
    [SerializeField] Image cursorImage;

    private void Awake()
    {
        Locator.Subscribe(this);
    }

    public void DisplayFocusCursor(bool isVisible) => cursorImage.gameObject.SetActive(isVisible);
}
