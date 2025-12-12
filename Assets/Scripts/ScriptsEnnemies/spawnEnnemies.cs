using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spawnEnnemies : MonoBehaviour
{
    private GameObject enemyPrefab;
    public GameObject player;
    public float spawnRadius = 5f;
    public float spawnInterval = 3f;
    public int nbEnemies = 3;
    public float increaseInterval = 5f;
    public int increaseAmount = 3;
    public int palier1, palier2;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        nbEnemies = 3;
        // On récupère le joueur dès que la scène est chargée
        player = GameObject.FindWithTag("Joueur");

        if (scene.name != "MainMenu")
        {
            // Redémarre les coroutines si on change de niveau
            StartCoroutine(SpawnEnemiesRoutine());
            StartCoroutine(increaseEnemiesRoutine());
        }
    }


    public void SpawnEnemy(String enemyName)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            // on récupère le prefab en fonction du nom
            enemyPrefab = Resources.Load<GameObject>("Prefab/Monstres/" + enemyName);

            // angle aléatoire en radians
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

            // calcul de la position sur le cercle
            Vector2 spawnPosition = new Vector2(
                player.transform.position.x + Mathf.Cos(angle) * spawnRadius,
                player.transform.position.y + Mathf.Sin(angle) * spawnRadius
            );

            // instanciation de l'ennemi
            GameObject ennemie = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            // on assigne le joueur à l'ennemi ici parce qu'on peut pas le faire dans l'inspector
            ennemie.GetComponent<bougeEnnemies>().joueur = player;
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            // on attends deux secondes avant la première vague de spawn
            yield return new WaitForSeconds(2);

            while (true)
            {
                if (nbEnemies > palier1 && nbEnemies < palier2)
                {
                    int nbEnemieA = UnityEngine.Random.Range(1, nbEnemies);
                    int nbEnemieB = nbEnemies - nbEnemieA;
                    for (int i = 0; i < nbEnemieA; i++) SpawnEnemy("enemyPrefab");
                    for (int i = 0; i < nbEnemieB; i++) SpawnEnemy("enemyPrefab1");
                }
                else if (nbEnemies > palier2)
                {
                    int nbEnemieA = UnityEngine.Random.Range(1, nbEnemies);
                    int nbEnemieB = nbEnemies - nbEnemieA;
                    for (int i = 0; i < nbEnemieA; i++) SpawnEnemy("enemyPrefab1");
                    for (int i = 0; i < nbEnemieB; i++) SpawnEnemy("enemyPrefab2");
                }
                else
                {
                    for (int i = 0; i < nbEnemies; i++) SpawnEnemy("enemyPrefab");
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    IEnumerator increaseEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(increaseInterval);
            nbEnemies += increaseAmount;
        }
    }

}