using UnityEngine;

public interface IWeapon
{
    void updateWeapon();
    void essaieAttaque(GameObject cible);
    void Upgrade();
    public GameObject GetGameObject();
    public string GetName();
}