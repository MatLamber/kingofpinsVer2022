using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "CritDamageUpgrade", menuName = "Scriptable Objects/Upgrade Events/Crit Damage Upgrade")]
public class CritDamageUpgradeEvent : UpgradeEvent
{
    public float Percentage;

    public override void Trigger()
    {
        CritDamageUpgrade.Trigger(Percentage);
    }

    public static void Trigger(float percentage)
    {
        var e = new CritDamageUpgrade { Percentage = percentage };
        MMEventManager.TriggerEvent(e);
    }
}
