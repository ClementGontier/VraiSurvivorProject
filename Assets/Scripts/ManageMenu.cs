using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageMenu : MonoBehaviour
{
    public AudioSource background;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Niveau1()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
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
