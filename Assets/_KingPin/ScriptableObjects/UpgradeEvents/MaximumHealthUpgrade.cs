using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "MaximumHealthUpgrade",
    menuName = "Scriptable Objects/Upgrade Events/Maximum Health Upgrade")]
public class MaximumHealthUpgrade : UpgradeEvent
{
    public float Percentage;

    public override void Trigger()
    {
        MaximumHealth.Trigger(Percentage);
    }

    public static void Trigger(float percentage)
    {
        var e = new MaximumHealth { Percentage = percentage };
        MMEventManager.TriggerEvent(e);
    }
}
