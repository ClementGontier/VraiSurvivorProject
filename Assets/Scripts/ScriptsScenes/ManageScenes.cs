using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScenes : MonoBehaviour
{
    public static ManageScenes instance;

    [Header("Menus")]
    public GameObject gameOverMenu;


    private void Awake()
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

    }

    private void Start()
    {
        gameOverMenu = DontDestroyUI.instance.GetGameOverMenu();
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
       if (scene.name != "MainMenu" && scene.name != "Victoire") DontDestroyUI.instance.gameObject.SetActive(true);
       if(scene.name == "MainMenu" || scene.name == "Victoire")
        {
            DontDestroyUI.instance.healthUI.gameObject.SetActive(false);
            DontDestroyUI.instance.timer.gameObject.SetActive(false);
            DontDestroyUI.instance.XPBar.gameObject.SetActive(false);
        }  
       else {DontDestroyUI.instance.healthUI.gameObject.SetActive(true);
       DontDestroyUI.instance.timer.gameObject.SetActive(true);
       DontDestroyUI.instance.XPBar.gameObject.SetActive(true);}
        // Désactive automatiquement le GameOverMenu sur nouvelle scène
        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);

        Time.timeScale = 1f; // reprend le temps si c'était en pause
    }

    public void gameOver()
    {
        if (gameOverMenu != null)
            gameOverMenu.SetActive(true);
           // GameObject.FindGameObjectWithTag("HealthUI").SetActive(false);

        Time.timeScale = 0f; // Met le temps en pause
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        if(SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Victoire")
            Singleton.Instance.hasLoadedScene=false;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }


}