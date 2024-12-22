using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraBulletUpgrade", menuName = "Scriptable Objects/Upgrade Events/Extra Bullet Upgrade")]
public class ExtraBulletUpgrade : UpgradeEvent
{
    public bool UpgradeActive;
    public override void Trigger()
    {
        ExtraBullet.Trigger(UpgradeActive);
    }

    public static void Trigger(bool upgradeActive)
    {
        var e = new ExtraBullet { UpdateActive = upgradeActive };
        MMEventManager.TriggerEvent(new ExtraBullet());
    }
}
