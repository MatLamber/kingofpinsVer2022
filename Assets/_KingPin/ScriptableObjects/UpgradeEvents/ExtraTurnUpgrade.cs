using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraTurnUpgrade", menuName = "Scriptable Objects/Upgrade Events/Extra Turn Upgrade")]
public class ExtraTurnUpgrade : UpgradeEvent
{
    public float Percentage; // O podrías usar un int para el número de turnos extra

    public override void Trigger()
    {
        ExtraTurn.Trigger(Percentage);
    }

    public static void Trigger(float percentage)
    {
        var e = new ExtraTurn { Percentage = percentage };
        MMEventManager.TriggerEvent(e);
    }
}
