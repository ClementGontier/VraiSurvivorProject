using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageMenu : MonoBehaviour
{
    public AudioSource background;
    private Singleton singleton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake()
    {
        singleton = FindAnyObjectByType<Singleton>();
    }

    public void Niveau1()
    {
        if (singleton != null)
        {
            Time.timeScale = 1f;
            singleton.playerXP = 0;
            singleton.playerLevel = 1;
            singleton.expToNextLevel = 10;
            singleton.xpBar.UpdateXPBarTitle(singleton.playerLevel);
            singleton.isAlive = true;
            singleton.playerMaxHealth = 10;
            singleton.playerHealth = singleton.playerMaxHealth;
            singleton.timertime = singleton.timertimeMax;
            Debug.Log("start fait");
            SceneManager.LoadScene("niveau1");
        }
        else
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        background.Play();
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
