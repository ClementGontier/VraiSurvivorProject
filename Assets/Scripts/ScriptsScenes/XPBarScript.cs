using UnityEngine;
using UnityEngine.UI;

public class XPBarScript : MonoBehaviour
{
    public Slider slider;

    private void Awake()
    {
    }

    public void UpdateXPBar(int currentXP, int xpToNextLevel)
    {
        slider.maxValue = xpToNextLevel;
        slider.value = currentXP;
    }
}
