using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldUpgrade", menuName = "Scriptable Objects/Upgrade Events/Shield Upgrade")]
public class ShieldUpgrade : UpgradeEvent
{
    public float ShieldPoints;

    public override void Trigger()
    {
        Shield.Trigger(ShieldPoints);
    }

    public static void Trigger(float shieldPoints)
    {
        var e = new Shield { ShieldPoints = shieldPoints };
        MMEventManager.TriggerEvent(e);
    }
}
