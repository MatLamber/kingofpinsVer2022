using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "CritChanceUpgrade", menuName = "Scriptable Objects/Upgrade Events/Crit Chance Upgrade")]
public class CritChanceUpgradeEvent : UpgradeEvent
{
    public float CritChance;

    public override void Trigger()
    {
        CritChanceUpgrade.Trigger(CritChance);
    }

    public static void Trigger(float critChance)
    {
        var e = new CritChanceUpgrade { CritChance = critChance };
        MMEventManager.TriggerEvent(e);
    }
}
