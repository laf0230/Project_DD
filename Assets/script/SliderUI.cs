using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    public Slider slider;
    public StatSO sliderObj;

    public void Update()
    {
        slider.maxValue = sliderObj.stat.maxHealth;
        slider.value = sliderObj.stat.currentHealth;
    }
}
