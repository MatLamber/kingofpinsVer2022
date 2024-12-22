using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStealUpgrade", menuName = "Scriptable Objects/Upgrade Events/Life Steal Upgrade")]
public class LifeStealUpgrade : UpgradeEvent
{
    public float Percentage;

    public override void Trigger()
    {
        LifeSteal.Trigger(Percentage);
    }

    public static void Trigger(float percentage)
    {
        var e = new LifeSteal { Percentage = percentage };
        MMEventManager.TriggerEvent(e);
    }
}
