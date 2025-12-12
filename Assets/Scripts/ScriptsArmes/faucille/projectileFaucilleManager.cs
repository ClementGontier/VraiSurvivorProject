using UnityEngine;

public class projectileFaucilleManager : MonoBehaviour
{
    [HideInInspector]
    public int degats;
    public float distanceAvantDemi = 8f;
    public Vector3 directionInitiale;
    public float vitesseProjectile = 10f;
    public float vitesseRotation = 720f;
    public float taille;
    private Vector3 positionDepart;
    private Vector3 pointDemiTour;
    private float distanceRetourObjectif;
    private bool enRetour = false;
    private Rigidbody2D rb;

    void Start()
    {
        positionDepart = transform.position;
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(taille, taille, 0);
    }

    void Update()
    {
        // rotation visuelle de la faucille
        transform.Rotate(0f, 0f, vitesseRotation * Time.deltaTime);

        if (!enRetour)
        {
            float distanceParcourue = Vector3.Distance(positionDepart, transform.position);
            if (distanceParcourue >= distanceAvantDemi)
            {
                enRetour = true;
                pointDemiTour = transform.position;
                distanceRetourObjectif = distanceParcourue * 2f;

                Vector2 directionRetour = -rb.linearVelocity.normalized;
                rb.linearVelocity = directionRetour * vitesseProjectile;
            }
        }
        else
        {
            float distanceRetour = Vector3.Distance(pointDemiTour, transform.position);
            if (distanceRetour >= distanceRetourObjectif)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ennemie")
        {
            vieEnnemies vieEnnemi = collision.gameObject.GetComponent<vieEnnemies>();
            if (vieEnnemi != null)
            {
                vieEnnemi.prendsDegats(degats);
                // Debug.Log("L'ennemie prend " + degats + " degats");
            }
        }
    }
}
