using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageReduceUpgrade",
    menuName = "Scriptable Objects/Upgrade Events/Damage Reduce Upgrade")]
public class DamageReduceUpgrade : UpgradeEvent
{
    public float Percentage;

    public override void Trigger()
    {
        DamageReduce.Trigger(Percentage);
    }

    public static void Trigger(float percentage)
    {
        var e = new DamageReduce { Percentage = percentage };
        MMEventManager.TriggerEvent(e);
    }
}