using UnityEngine;
using MoreMountains.Tools;


[CreateAssetMenu(fileName = "DamagePercentageUpgrade",
    menuName = "Scriptable Objects/Upgrade Events/Damage Percentage Upgrade")]
public class DamagePercentageUpgradeEvent : UpgradeEvent
{
    public float Percentage;

    public override void Trigger()
    {
        DamagePercentageUpgrade.Trigger(Percentage);
    }

    public static void Trigger(float percentage)
    {
        var e = new DamagePercentageUpgrade { Percentage = percentage };
        MMEventManager.TriggerEvent(e);
    }
}