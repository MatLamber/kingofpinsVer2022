using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "HealToPercentageUpgrade",
    menuName = "Scriptable Objects/Upgrade Events/Heal To Percentage Upgrade")]
public class HealToPercentageUpgrade : UpgradeEvent
{
    public float Percentage;

    public override void Trigger()
    {
        HealToPercentage.Trigger(Percentage);
    }

    public static void Trigger(float percentage)
    {
        var e = new HealToPercentage { Percentage = percentage };
        MMEventManager.TriggerEvent(e);
    }
}
