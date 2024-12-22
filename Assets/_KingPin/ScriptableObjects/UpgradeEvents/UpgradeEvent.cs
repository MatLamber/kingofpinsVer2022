using UnityEngine;
using MoreMountains.Tools;

public abstract class UpgradeEvent : ScriptableObject
{
    public UpgradeType upgradeType;
    public abstract void Trigger();
}
