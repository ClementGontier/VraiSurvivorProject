using System.Collections.Generic;
using UnityEngine;

public class aura : MonoBehaviour, IWeapon
{
    public float vitesseAttaque = 1f;
    public int degats = 10;
    public float taille = 5;
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

    public void Upgrade()
    {
        vitesseAttaque += 0.5f;
        degats += 2;
        taille += 3;
    }

    public void updateWeapon()
    {
        
    }
    
    public void essaieAttaque(GameObject ennemie)
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ennemie") EnnemiesDansZone.Add(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ennemie") EnnemiesDansZone.Remove(collision.gameObject);
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
                // Debug.Log("L'ennemie prend " + degats + " degats");
            }
        }
    }
}
