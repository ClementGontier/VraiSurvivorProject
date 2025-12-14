using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }

    [Header("Stats Joueur")]
    public int playerMaxHealth = 10;
    public int playerHealth = 10;
    public bool isAlive = true;
    public bool isInvincible = false;
    public int nbEnnemiesKilled = 0;

    [Header("XP / Leveling")]
    public int playerXP = 0;
    public int playerLevel = 1;
    public int expToNextLevel = 10;
    public weaponsManager wm;
    public float timertime = 60;

    [Header("Références Scène")]
    public TMP_Text pvText;
    public TMP_Text timer;
    public XPBarScript xpBar;
    public float timertimeMax = 60;
    public bool hasLoadedScene = false;


    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Victoire")
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musiqueniveau);
            timertime -= 1 * Time.deltaTime;
            timer.text = timertime.ToString("0");

            if (timertime <= 0 && !hasLoadedScene)
            {
                hasLoadedScene = true;
                timertime = 0;
                ManageScenes.instance.NextLevel();
                timertime = timertimeMax;
            }
        }
        else
        {
            AudioManager.Instance.musicSource.Stop();
        }
    }

    private void Awake()
    {
        // Gestion du singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pvText = DontDestroyUI.instance.GetHealthUI();
        timer = DontDestroyUI.instance.GetTimer();
        xpBar = DontDestroyUI.instance.XPBar;

        playerHealth = playerMaxHealth;

        if (scene.name == "MainMenu" || scene.name == "Victoire")
        {
            Destroy(GameObject.Find("Joueur"));
        }
        else
        {
            wm = FindWeaponsManager();
        }

        if (timer != null)
            timer.text = "Temps : " + timertime.ToString();
        if (pvText != null)
            pvText.text = "Vies : " + playerHealth.ToString() + "/" + playerMaxHealth.ToString();

        xpBar.UpdateXPBar(playerXP, expToNextLevel);
    }

    private weaponsManager FindWeaponsManager()
    {
        weaponsManager manager = FindFirstObjectByType<weaponsManager>(FindObjectsInactive.Include);
        if (manager == null) Debug.LogWarning("weaponsManager introuvable dans la scène");
        return manager;
    }


    public void TakeDamage(int damage)
    {
        if (!isAlive || isInvincible)
            return;

        playerHealth -= damage;
        StartCoroutine(ICD());
        if (playerHealth < 0) playerHealth = 0;

        if (pvText != null)
            pvText.text = "Vies : " + playerHealth.ToString() + "/" + playerMaxHealth.ToString();

        if (playerHealth <= 0 && isAlive)
        {
            isAlive = false;
            DontDestroyUI.instance.healthUI.gameObject.SetActive(false);
            DontDestroyUI.instance.timer.gameObject.SetActive(false);
            DontDestroyUI.instance.XPBar.gameObject.SetActive(false);
            ManageScenes.instance.gameOver();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameoverson);

        }
    }

    // ajout une seconde d'invincibilité après s'être fait touché par un ennemi
    private IEnumerator ICD()
    {
        isInvincible = true;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.degat);
        Debug.Log("Joueur invincible pendant 1 seconde");
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    public void AddXP(int nbExp)
    {
        playerXP += nbExp;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.xpson);   
        while (playerXP >= expToNextLevel)
        {
            LevelUp();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.levelupson);
        }
        xpBar.UpdateXPBar(playerXP, expToNextLevel);
    }

    private void LevelUp()
    {
        playerLevel++;
        playerXP -= expToNextLevel;
        expToNextLevel += 10;
        playerMaxHealth += 2;
        playerHealth += 2;
        xpBar.UpdateXPBar(playerXP, expToNextLevel);
        if (wm == null) wm = FindWeaponsManager();
        if (wm != null)
            wm.ApplyRandomUpgrade();
        xpBar.UpdateXPBarTitle(playerLevel);
    }
}
