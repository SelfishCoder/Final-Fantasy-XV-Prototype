using UnityEngine;
using SelfishCoder.Core;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class TurretManager: Singleton<TurretManager>
{
    public List<GameObject> turrets = new List<GameObject>();

    public void BuildTower(BuildArea location,int turretIndex)
    {
        if (!IsBuildable(location,turrets[turretIndex].GetComponent<Turret>())) return;
        Turret turret = turrets[turretIndex].GetComponent<Turret>();
        GameObject tower = Instantiate(turrets[turretIndex]);
        tower.transform.position = location.transform.position;
        location.IsEmpty = false;
        GoldManager.Instance.SpendGold(turret.gold);
    }

    public void UpgradeTower(Turret turret)
    {
        turret.power += 10;
        turret.level++;
    }

    private bool IsBuildable(BuildArea buildArea, Turret turret)
    {
        return buildArea.IsEmpty && GoldManager.Instance.HasEnoughGold(turret.gold);
    }
    
}