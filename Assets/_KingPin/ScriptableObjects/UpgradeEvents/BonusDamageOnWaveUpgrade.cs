using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusDamageOnWaveUpgrade",
    menuName = "Scriptable Objects/Upgrade Events/Bonus Damage On Wave Upgrade")]
public class BonusDamageOnWaveUpgrade : UpgradeEvent
{
    public bool UpgradeActive;
    
    public override void Trigger()
    {
        BonusDamageOnWaveUpgrade.Trigger(UpgradeActive);
    }

    public static void Trigger(bool upgradeActive)
    {
        var e = new BonusDamageOnWave { UpgradeActive = upgradeActive };
        MMEventManager.TriggerEvent(e);
    }
}
