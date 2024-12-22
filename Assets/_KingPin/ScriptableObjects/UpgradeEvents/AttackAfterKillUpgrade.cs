using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAfterKillUpgrade",
    menuName = "Scriptable Objects/Upgrade Events/Attack After Kill Upgrade")]
public class AttackAfterKillUpgrade : UpgradeEvent
{
    public bool ShouldAttack; // Considera usar un int para el número de ataques

    public override void Trigger()
    {
        AttackAfterKill.Trigger(ShouldAttack);
    }

    public static void Trigger(bool shouldAttack)
    {
        var e = new AttackAfterKill { ShouldAttack = shouldAttack };
        MMEventManager.TriggerEvent(e);
    }
}
