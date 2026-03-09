using System.Collections.Generic;
using UnityEngine;

public class pelles : MonoBehaviour, IWeapon
{
    public int nombreProjectiles = 3;
    public float vitesseAttaque = 1f;
    protected float tempsAvantProchaineAttaque = 0f;
    public float vitesseProjectile = 10f;
    public int degats = 10;
    public float distanceAvantDestruction = 8f;
    public Sprite icone;
    protected GameObject projectilePrefab;
    public GameObject departTire;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Prefab/Armes/projPelles");
    }

    // Update is called once per frame
    public void updateWeapon()
    {
        // tant que le temps avant la prochaine attaque est supérieur à 0 on le décrémente
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
                Debug.Log("vitesse attaque pelle augmenté");
                break;
            case 2:
                nombreProjectiles ++;
                Debug.Log("nb projetile pelle augmenté");
                break;
            case 3:
                degats += 2;
                Debug.Log("attaque pelle augmenté");
                break;
        }
    }

    public List<UpgradeOption> GetUpgradeOptions()
    {
        return new List<UpgradeOption>
        {
            new UpgradeOption
            {
                titre = "Level up Pelles",
                sousTitre = "Cadence de tir",
                description = "Augmente la cadence de tir de +0.5",
                icone = icone,
                appliquer = () => { vitesseAttaque += 0.5f; }
            },
            new UpgradeOption
            {
                titre = "Level up Pelles",
                sousTitre = "Projectiles",
                description = "Ajoute +1 projectile au tir",
                icone = icone,
                appliquer = () => { nombreProjectiles++; }
            },
            new UpgradeOption
            {
                titre = "Level up Pelles",
                sousTitre = "Dégâts",
                description = "Augmente les dégâts de +2",
                icone = icone,
                appliquer = () => { degats += 2; }
            }
        };
    }

    public void Reinit()
    {
        nombreProjectiles = 3;
        vitesseAttaque = 1f;
        degats = 10;
    }

    public void essaieAttaque(GameObject ennemie)
    {
        if(tempsAvantProchaineAttaque<=0)
        {
            attaque(ennemie);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pelleson);
            tempsAvantProchaineAttaque = 1f / vitesseAttaque;
        }
    }

    public void attaque(GameObject ennemie)
    {
        // on calcul les angles entre chaque projectile
        float angleEntreProjectiles = 360f / nombreProjectiles;

        for (int i = 0; i < nombreProjectiles; i++)
        {
            // on détermine l'angle de tir du projectile
            float angle = i * angleEntreProjectiles + 90f;

            // convertir l'angle en direction (vecteur)
            float angleRadians = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians), 0);

            // on crée le projectile
            GameObject projectile = Instantiate(projectilePrefab, departTire.transform.position, Quaternion.identity);

            // on oriente le projectile (j'ai mis +270 à l'angle pour que le sprite soit dans le bon sens, sinon il était perpendiculaire à la direction et j'ai la flemme de savoir pourquoi)
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+270));

            // on assigne les dégâts au projectile et la distance avant destruction
            projectilesPellesManager paramProj = projectile.GetComponent<projectilesPellesManager>();
            if (paramProj != null)
            {
                paramProj.degats = degats;
                paramProj.distanceAvantDestruction = distanceAvantDestruction;
            }

            // on annule la gravité et on applique une vélocité au projectile
            Rigidbody2D rbP = projectile.GetComponent<Rigidbody2D>();
            if (rbP != null)
            {
                rbP.gravityScale = 0;
                rbP.linearVelocity = direction.normalized * vitesseProjectile;
            }
        }
    }
}
