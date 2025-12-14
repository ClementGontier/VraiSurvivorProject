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
    private AudioManager audiom;
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
        gameObject.SetActive(true);
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
            audiom = FindAnyObjectByType<AudioManager>();
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
        singleton.playerXP = 0;
        singleton.playerLevel = 1;
        singleton.expToNextLevel = 10;
        // pour éviter les erreurs null reference quand on restart plein de fois
        if (singleton.xpBar == null) singleton.xpBar = FindFirstObjectByType<XPBarScript>(FindObjectsInactive.Include);
        singleton.xpBar.UpdateXPBarTitle(singleton.playerLevel);
        singleton.isAlive = true;
        singleton.playerMaxHealth = 10;
        singleton.playerHealth = singleton.playerMaxHealth;
        singleton.timertime = singleton.timertimeMax;
        reinitialiserListeArmes();
        Debug.Log("restart fait");
        SceneManager.LoadScene("niveau1");
    }

    private void reinitialiserListeArmes()
    {
        IWeapon pistoletAuto = null;
        // pour éviter les erreurs null reference quand on restart plein de fois
        if (singleton.wm == null) singleton.wm = FindFirstObjectByType<weaponsManager>(FindObjectsInactive.Include);
        for (int i = singleton.wm.activeWeapons.Count - 1; i >= 0; i--)
        {
            IWeapon weapon = singleton.wm.activeWeapons[i];
            if (weapon != null)
            {
                if (weapon.GetGameObject().name == "pistoletAuto")
                {
                    pistoletAuto = weapon;
                }
                weapon.Reinit();
                singleton.wm.eneleverArme(weapon);
                Debug.Log(weapon.GetGameObject().name + " a été réinitialisé et retiré de la liste");
            }

        }
        singleton.wm.ajoutArme(pistoletAuto);
    }
}