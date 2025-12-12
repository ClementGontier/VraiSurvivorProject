using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.WindowsMR.Input;
using System;

public class DontDestroyUI : MonoBehaviour
{
    public static DontDestroyUI instance; 
    public GameObject gameOverMenu; 
    public TMP_Text healthUI;
    public TMP_Text timer;
    public XPBarScript XPBar;
    private Singleton singleton;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "niveau1")
        {
            singleton = FindAnyObjectByType<Singleton>();
            if (singleton != null)
            {
                Debug.Log("SUCE MA BITE");
            }
        }
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
        singleton.playerXP = 0;
        singleton.playerLevel = 1;
        singleton.isAlive = true;
        singleton.playerHealth = singleton.playerMaxHealth;
        singleton.timertime = singleton.timertimeMax;
        reinitialiserListeArmes();
    }

    private void reinitialiserListeArmes()
    {
        foreach(IWeapon weapon in singleton.wm.activeWeapons)
        {
            weapon.Reinit();
            singleton.wm.eneleverArme(weapon);
        }
    }
}