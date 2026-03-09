using System.Collections.Generic;
using UnityEngine;

public class aura : MonoBehaviour, IWeapon
{
    public float vitesseAttaque = 0.5f;
    public int degats = 1;
    public float taille = 5;
    public Sprite icone;
    private float tempsAvantProchaineAttaque = 0f;
    private List<GameObject> EnnemiesDansZone = new();

    // Update is called once per frame
    public void Update()
    {
        // tant que le temps avant la prochaine attaque est supérieur à 0 on le décrémente
        if (tempsAvantProchaineAttaque > 0)
        {
            tempsAvantProchaineAttaque -= Time.deltaTime;
        }

        transform.localScale = new Vector3(taille, taille, 0);

        if (tempsAvantProchaineAttaque <= 0)
        {
            attaque();
            tempsAvantProchaineAttaque = 1f / vitesseAttaque;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public string GetName()
    {
        return gameObject.name;
    }

    public Sprite GetIcone()
    {
        return icone;
    }

    public void Upgrade()
    {
        int x = Random.Range(1, 4);
        switch (x)
        {
            case 1:
                vitesseAttaque += 0.5f;
                Debug.Log("vitesse attaque aura augmenté");
                break;
            case 2:
                taille += 3;
                Debug.Log("taille aura augmenté");
                break;
            case 3:
                degats += 2;
                Debug.Log("attaque aura augmenté");
                break;
        }
    }

    public List<UpgradeOption> GetUpgradeOptions()
    {
        return new List<UpgradeOption>
        {
            new UpgradeOption
            {
                titre = "Level up Aura",
                sousTitre = "Fréquence",
                description = "Augmente la fréquence des dégâts de +0.5",
                icone = icone,
                appliquer = () => { vitesseAttaque += 0.5f; }
            },
            new UpgradeOption
            {
                titre = "Level up Aura",
                sousTitre = "Portée",
                description = "Augmente la zone d'effet de +3",
                icone = icone,
                appliquer = () => { taille += 3; }
            },
            new UpgradeOption
            {
                titre = "Level up Aura",
                sousTitre = "Dégâts",
                description = "Augmente les dégâts de +2",
                icone = icone,
                appliquer = () => { degats += 2; }
            }
        };
    }

    public void Reinit()
    {
        vitesseAttaque = 0.5f;
        degats = 1;
        taille = 5;
    }

    public void updateWeapon()
    {

    }

    public void essaieAttaque(GameObject ennemie)
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ennemie") EnnemiesDansZone.Add(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ennemie") EnnemiesDansZone.Remove(collision.gameObject);
    }

    public void attaque()
    {
        // boucle for a l'envers pour éviter de modifier la liste dans un foreach ce qui provoque une erreur
        for (int i = EnnemiesDansZone.Count - 1; i >= 0; i--)
        {
            vieEnnemies vieEnnemi = EnnemiesDansZone[i].GetComponent<vieEnnemies>();
            if (vieEnnemi != null)
            {
                vieEnnemi.prendsDegats(degats);
            }
        }
    }
}
