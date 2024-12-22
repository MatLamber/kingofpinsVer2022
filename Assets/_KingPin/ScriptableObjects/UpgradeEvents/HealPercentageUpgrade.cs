using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "HealPercentageUpgrade",
    menuName = "Scriptable Objects/Upgrade Events/Heal Percentage Upgrade")]
public class HealPercentageUpgrade : UpgradeEvent
{
    public float Percentage;

    public override void Trigger()
    {
        HealPercentage.Trigger(Percentage);
    }

    public static void Trigger(float percentage)
    {
        var e = new HealPercentage { Percentage = percentage };
        MMEventManager.TriggerEvent(e);
    }
}
