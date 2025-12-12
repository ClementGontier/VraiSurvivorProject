using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroyUI : MonoBehaviour
{
    public static DontDestroyUI instance; 
    public GameObject gameOverMenu; 
    public TMP_Text healthUI;
    public TMP_Text timer;
    public XPBarScript XPBar;
    void Awake()
    {

        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
        }
        gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
    }


    public GameObject GetGameOverMenu()
    {
        return gameOverMenu;
    }

    public TMP_Text GetHealthUI()
    {
        return healthUI;
    }

     public TMP_Text GetTimer()
    {
        return timer;
    }

    public XPBarScript GetXPBar()
    {
        return XPBar;
    }

    public void LoadMainMenu()
    {
        instance.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}