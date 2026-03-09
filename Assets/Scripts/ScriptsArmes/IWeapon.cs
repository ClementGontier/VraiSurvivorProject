using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void updateWeapon();
    void essaieAttaque(GameObject cible);
    void Upgrade();
    GameObject GetGameObject();
    string GetName();
    void Reinit();
    List<UpgradeOption> GetUpgradeOptions();
    Sprite GetIcone();
}
