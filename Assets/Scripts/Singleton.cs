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
    [SerializeField] private Animator animdeath;
    private XPBarScript xpBar;
    public float timertimeMax = 60;
    public bool hasLoadedScene = false;

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Victoire")
        {
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
        wm = FindFirstObjectByType<weaponsManager>();
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
        animdeath = GameObject.Find("Joueur")?.GetComponent<Animator>();
        xpBar = DontDestroyUI.instance.XPBar;
        
        if (scene.name == "MainMenu" || scene.name == "Victoire")
            Destroy(GameObject.Find("Joueur"));
        if(timer != null)
            timer.text= "Temps : " + timertime.ToString();
        if (pvText != null)
            pvText.text = "Vies : " + playerHealth.ToString();

        if (animdeath != null)
            animdeath.SetBool("isNotAlive", false);

        xpBar.UpdateXPBar(playerXP, expToNextLevel);
    }

    
    public void TakeDamage(int damage)
    {
        if (!isAlive || isInvincible)
            return;

        playerHealth -= damage;
        StartCoroutine(ICD());
        if (playerHealth < 0) playerHealth = 0;

        if (pvText != null)
            pvText.text = "Vies : " + playerHealth.ToString();

        if (playerHealth <= 0 && isAlive)
        {
            isAlive = false;
            DontDestroyUI.instance.healthUI.gameObject.SetActive(false);
            DontDestroyUI.instance.timer.gameObject.SetActive(false);
            DontDestroyUI.instance.XPBar.gameObject.SetActive(false);
            if (animdeath != null)
                animdeath.SetBool("isNotAlive", true);

            ManageScenes.instance.gameOver();
        }
    }


    // ajout une seconde d'invincibilité après s'être fait touché par un ennemi
    private IEnumerator ICD()
    {
        isInvincible = true;
        Debug.Log("Joueur invincible pendant 1 seconde");
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    public void AddXP(int nbExp)
    {
        playerXP += nbExp;
        xpBar.UpdateXPBar(playerXP, expToNextLevel);

        if (playerXP >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        playerLevel++;
        playerXP -= expToNextLevel;
        expToNextLevel += 10; 
        wm.ApplyRandomUpgrade();
    }
}
