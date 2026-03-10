using System.Collections.Generic;
using UnityEngine;

public class fleche : MonoBehaviour, IWeapon
{
    public int nombreProjectiles = 3;
    public float vitesseAttaque = 1f;
    protected float tempsAvantProchaineAttaque = 0f;
    public float vitesseProjectile = 10f;
    public int degats = 5;
    public float distanceAvantDestruction = 8f;
    public float angleEcart = 25f;
    public Sprite icone;
    protected GameObject projectilePrefab;
    public GameObject departTire;
    private weaponsManager wm;

    void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Prefab/Armes/projFleche");
        wm = GetComponentInParent<weaponsManager>();
    }

    public void updateWeapon()
    {
        if (tempsAvantProchaineAttaque > 0)
        {
            tempsAvantProchaineAttaque -= Time.deltaTime;
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
        nombreProjectiles++;
        angleEcart += 5f;
    }

    public List<UpgradeOption> GetUpgradeOptions()
    {
        return new List<UpgradeOption>
        {
            new UpgradeOption
            {
                titre = "Level up Flèches",
                sousTitre = "Volée",
                description = "+1 flèche et rayon +5°",
                icone = icone,
                appliquer = () => { nombreProjectiles++; angleEcart += 5f; }
            },
            new UpgradeOption
            {
                titre = "Level up Flèches",
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
        degats = 5;
        angleEcart = 25f;
    }

    public void essaieAttaque(GameObject ennemie)
    {
        if (tempsAvantProchaineAttaque <= 0)
        {
            GameObject cible = wm.ChoisirEnnemi();
            if (cible == null) return;
            attaque(cible);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pelleson);
            tempsAvantProchaineAttaque = 1f / vitesseAttaque;
        }
    }

    public void attaque(GameObject ennemie)
    {
        // direction de base vers l'ennemi
        Vector3 emplacementEnnemie = ennemie.GetComponent<Collider2D>().bounds.center;
        Vector3 directionBase = (emplacementEnnemie - departTire.transform.position).normalized;
        float angleBase = Mathf.Atan2(directionBase.y, directionBase.x) * Mathf.Rad2Deg;

        // espacement entre les flèches dans le cône
        float angleStep = nombreProjectiles > 1 ? angleEcart / (nombreProjectiles - 1) : 0f;
        float angleDepart = angleBase - angleEcart / 2f;

        for (int i = 0; i < nombreProjectiles; i++)
        {
            float angle = angleDepart + i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);

            GameObject projectile = Instantiate(projectilePrefab, departTire.transform.position, Quaternion.identity);
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle + 270);

            projectilesPellesManager paramProj = projectile.GetComponent<projectilesPellesManager>();
            if (paramProj != null)
            {
                paramProj.degats = degats;
                paramProj.distanceAvantDestruction = distanceAvantDestruction;
            }

            Rigidbody2D rbP = projectile.GetComponent<Rigidbody2D>();
            if (rbP != null)
            {
                rbP.gravityScale = 0;
                rbP.linearVelocity = direction * vitesseProjectile;
            }
        }
    }
}
