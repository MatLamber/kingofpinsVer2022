using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusDamageOnLowHpUpgrade",
    menuName = "Scriptable Objects/Upgrade Events/Bonus Damage On Low HP Upgrade")]
public class BonusDamageOnLowHpUpgradeEvent : UpgradeEvent
{
    public bool UpgradeActive;
    

    public override void Trigger()
    {
        BonusDamageOnLowHp.Trigger(UpgradeActive); // Pasa ambos valores al struct
    }

    public static void Trigger(bool upgradeActive)
    {
        var e = new BonusDamageOnLowHp { UpgradeActive = upgradeActive };
        MMEventManager.TriggerEvent(e);
    }
}
