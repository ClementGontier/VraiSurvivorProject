using UnityEngine;
using System.Text.RegularExpressions;

public class vieEnnemies : MonoBehaviour
{
    public int vieMax;
    protected int vieActuelle;
    public int degats;
    public GameObject xpPrefab;
    private Singleton singleton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vieActuelle = vieMax;
        singleton = Singleton.Instance;
    }

    public void prendsDegats(int montant)
    {
        vieActuelle -= montant;
        if (vieActuelle <= 0)
        {
            meurt();
        }
    }

    void meurt()
    {
        dropExp();
        Destroy(gameObject);
    }

    void dropExp()
    {
        if (Regex.IsMatch(gameObject.name.ToLower(), "boss"))
        {
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            int nb = Random.Range(1, 3);
            if (nb == 2)
            {
                Instantiate(xpPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
