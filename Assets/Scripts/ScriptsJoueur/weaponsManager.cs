using System.Collections.Generic;
using UnityEngine;

public class weaponsManager : MonoBehaviour
{
    public List<IWeapon> lockedWeapons = new List<IWeapon>();
    public List<IWeapon> activeWeapons = new List<IWeapon>();
    private List<GameObject> EnnemiesDansZone = new();

    void Start()
    {
    IWeapon[] allWeapons = GetComponentsInChildren<IWeapon>(true);

    foreach (IWeapon weapon in allWeapons)
    {
        GameObject go = weapon.GetGameObject();

        if (go.activeSelf)
        {
            activeWeapons.Add(weapon);
        }
        else
        {
            lockedWeapons.Add(weapon);
        }
    }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(IWeapon weapon in activeWeapons)
        {
            weapon.updateWeapon();
        }
    }

    public void ajoutArme(IWeapon weapon)
    {
        lockedWeapons.Remove(weapon);
        activeWeapons.Add(weapon);
        weapon.GetGameObject().SetActive(true);
    }

    public void eneleverArme(IWeapon weapon)
    {
        lockedWeapons.Add(weapon);
        activeWeapons.Remove(weapon);
        weapon.GetGameObject().SetActive(false);
    }


    void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "ennemie")
        {
            foreach(IWeapon weapon in activeWeapons)
            {
                weapon.essaieAttaque(trigger.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ennemie") EnnemiesDansZone.Add(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ennemie") EnnemiesDansZone.Remove(collision.gameObject);
    }

    public GameObject ChoisirEnnemi()
    {
        if (EnnemiesDansZone == null || EnnemiesDansZone.Count == 0)
            return null;

        // Ennemi aléatoire !
        int index = Random.Range(0, EnnemiesDansZone.Count);
        return EnnemiesDansZone[index];
    }

    public List<UpgradeOption> GenererOptionsUpgrade(int nb)
    {
        List<UpgradeOption> toutesOptions = new List<UpgradeOption>();

        // Options d'unlock pour les armes verrouillées
        foreach (IWeapon weapon in lockedWeapons)
        {
            IWeapon w = weapon;
            toutesOptions.Add(new UpgradeOption
            {
                titre = "Unlock " + w.GetName(),
                sousTitre = "Nouvelle arme",
                description = "Débloque l'arme " + w.GetName(),
                icone = w.GetIcone(),
                estUnlock = true,
                appliquer = () => ajoutArme(w)
            });
        }

        // Options d'upgrade pour les armes actives
        foreach (IWeapon weapon in activeWeapons)
        {
            toutesOptions.AddRange(weapon.GetUpgradeOptions());
        }

        // Mélange Fisher-Yates
        for (int i = toutesOptions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (toutesOptions[i], toutesOptions[j]) = (toutesOptions[j], toutesOptions[i]);
        }

        return toutesOptions.GetRange(0, Mathf.Min(nb, toutesOptions.Count));
    }

    public void ApplyRandomUpgrade()
    {
        bool canUnlock = lockedWeapons.Count > 0;
        if (canUnlock && Random.value > 0.5f)
        {
            UnlockRandomWeapon();
        }
        else
        {
            UpgradeRandomWeapon();
        }
    }

    private void UnlockRandomWeapon()
    {
        IWeapon weapon = lockedWeapons[Random.Range(0, lockedWeapons.Count)];
        lockedWeapons.Remove(weapon);
        activeWeapons.Add(weapon);
        weapon.GetGameObject().SetActive(true);
        Debug.Log(weapon.GetGameObject().name + " ajouté.");
    }

    private void UpgradeRandomWeapon()
    {
        IWeapon weapon = activeWeapons[Random.Range(0, activeWeapons.Count)];
        weapon.Upgrade();
        Debug.Log(weapon.GetGameObject().name + " améliorée.");
    }

}
