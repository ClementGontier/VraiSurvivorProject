using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarScript : MonoBehaviour
{
    public Slider slider;
    public TMP_Text levelText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        levelText = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateXPBar(int currentXP, int xpToNextLevel)
    {
        slider.maxValue = xpToNextLevel;
        slider.value = currentXP;
    }

    public void UpdateXPBarTitle(int playerLevel)
    {
        levelText.text = "Level " + playerLevel.ToString();
    }
}
