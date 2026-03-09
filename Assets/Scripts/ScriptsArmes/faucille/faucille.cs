using System.Collections.Generic;
using UnityEngine;

public class faucille : MonoBehaviour, IWeapon
{
    public float vitesseAttaque = 1f;
    protected float tempsAvantProchaineAttaque = 0f;
    public float vitesseProjectile = 10f;
    public int degats = 1;
    public float distanceAvantDemi = 5f;
    public float taille = 1;
    public Sprite icone;
    protected GameObject projectilePrefab;
    public GameObject departTire;
    private weaponsManager wm;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Prefab/Armes/projFaucille");
        wm = GetComponentInParent<weaponsManager>();
    }



    // Update is called once per frame
    public void updateWeapon()
    {
        // tant que le temps avant la prochaine attaque est superieure a 0 on le decremente
        if (tempsAvantProchaineAttaque > 0)
        {
            tempsAvantProchaineAttaque -= Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        weaponsManager weaponsManager = GetComponentInParent<weaponsManager>();
        weaponsManager.ajoutArme(this);
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
                Debug.Log("vitesse attaque faucille augmenté");
                break;
            case 2:
                taille += 2;
                Debug.Log("taille faucille augmenté");
                break;
            case 3:
                degats += 2;
                Debug.Log("attaque faucille augmenté");
                break;
        }
    }

    public List<UpgradeOption> GetUpgradeOptions()
    {
        return new List<UpgradeOption>
        {
            new UpgradeOption
            {
                titre = "Level up Faucille",
                sousTitre = "Cadence de tir",
                description = "Augmente la cadence de tir de +0.5",
                icone = icone,
                appliquer = () => { vitesseAttaque += 0.5f; }
            },
            new UpgradeOption
            {
                titre = "Level up Faucille",
                sousTitre = "Taille",
                description = "Augmente la taille du projectile de +2",
                icone = icone,
                appliquer = () => { taille += 2; }
            },
            new UpgradeOption
            {
                titre = "Level up Faucille",
                sousTitre = "Dégâts",
                description = "Augmente les dégâts de +2",
                icone = icone,
                appliquer = () => { degats += 2; }
            }
        };
    }

    public void Reinit()
    {
        vitesseAttaque = 1f;
        degats = 1;
        taille = 1;
    }

    public void essaieAttaque(GameObject ennemie)
    {
        if (tempsAvantProchaineAttaque <= 0)
        {
            GameObject ennemiCible = wm.ChoisirEnnemi();
            attaque(ennemiCible);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.faucilleson);
            tempsAvantProchaineAttaque = 1f / vitesseAttaque;
        }
    }

    public void attaque(GameObject ennemie)
    {
        // on calcule la direction du projectile
        Vector3 emplacementEnnemie = ennemie.GetComponent<Collider2D>().bounds.center;
        Vector3 direction = (emplacementEnnemie - departTire.transform.position).normalized;

        // on cree le projectile
        GameObject projectile = Instantiate(projectilePrefab, departTire.transform.position, Quaternion.identity);

        // on assigne les paramètres au projectile
        projectileFaucilleManager paramsProj = projectile.GetComponent<projectileFaucilleManager>();
        if (paramsProj != null)
        {
            paramsProj.degats = degats;
            paramsProj.distanceAvantDemi = distanceAvantDemi;
            paramsProj.directionInitiale = direction;
            paramsProj.vitesseProjectile = vitesseProjectile;
            paramsProj.taille = taille;
        }

        // on annule la gravite et on applique une velocite au projectile
        Rigidbody2D rbP = projectile.GetComponent<Rigidbody2D>();
        if (rbP != null)
        {
            rbP.gravityScale = 0;
            rbP.linearVelocity = direction.normalized * vitesseProjectile;
        }
    }
}
