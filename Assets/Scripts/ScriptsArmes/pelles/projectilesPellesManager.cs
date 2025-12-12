using UnityEngine;

public class projectilesPellesManager : MonoBehaviour
{
    
    [HideInInspector]
    public int degats;
    public float distanceAvantDestruction = 8f;
    private Vector3 positionDepart;

    void Start()
    {
        positionDepart = transform.position;
    }

    void Update()
    {
        float distanceParcourue = Vector3.Distance(positionDepart, transform.position);
        if (distanceParcourue >= distanceAvantDestruction)
        {
            Destroy(gameObject);
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
                //Debug.Log("L'ennemie prend " + degats + " degats");
            }
            Destroy(gameObject);
        }
    }

}
