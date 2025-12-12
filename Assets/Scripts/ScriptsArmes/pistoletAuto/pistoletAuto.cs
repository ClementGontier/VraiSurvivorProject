using UnityEngine;

public class pistoletAuto : MonoBehaviour, IWeapon
{
    public float vitesseAttaque = 1f;
    protected float tempsAvantProchaineAttaque = 0f;
    public float vitesseProjectile = 10f;
    public int degats = 10;
    public float distanceAvantDestruction = 15f;
    protected GameObject projectilePrefab;
    public GameObject departTire;


    
    void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Prefab/Armes/projectile");
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

     public void Upgrade()
    {
        int x = Random.Range(1, 3);
        switch (x)
        {
            case 1:
                vitesseAttaque ++;
                Debug.Log("vitesse attaque boule de feu augmenté");
                break;
            case 2:
                degats ++;
                Debug.Log("attaque boule de feu augmenté");
                break;
        }
    }


    public void updateWeapon()
    {
        // tant que le temps avant la prochaine attaque est supérieur à 0 on le décrémente
        if (tempsAvantProchaineAttaque > 0)
        {
            tempsAvantProchaineAttaque -= Time.deltaTime;
        }
    }
    
    public void essaieAttaque(GameObject ennemie)
    {
        if(tempsAvantProchaineAttaque<=0)
        {
            attaque(ennemie);
            tempsAvantProchaineAttaque = 1f / vitesseAttaque;
        }
    }

    public void attaque(GameObject ennemie)
    {
        // on calcule la direction du projectile
        Vector3 emplacementEnnemie = ennemie.GetComponent<Collider2D>().bounds.center;
        Vector3 direction = (emplacementEnnemie - departTire.transform.position).normalized;

        // on calcule l'orientation du projectile
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // on crée le projectile
        GameObject projectile = Instantiate(projectilePrefab, departTire.transform.position, Quaternion.identity);

        // on oriente le projectile (j'ai mis +270 à l'angle pour que le sprite soit dans le bon sens, sinon il était perpendiculaire à la direction et j'ai la flemme de savoir pourquoi)
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+270));

        // on assigne les dégâts au projectile et la distance avant destruction 
        projectilesPistoletAutoManager paramsProj = projectile.GetComponent<projectilesPistoletAutoManager>();
        if (paramsProj != null)
        {
            paramsProj.degats = degats;
            paramsProj.distanceAvantDestruction = distanceAvantDestruction;
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
