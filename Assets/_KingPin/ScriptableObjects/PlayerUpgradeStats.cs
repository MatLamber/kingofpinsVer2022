using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerUpgradeStats", menuName = "Scriptable Objects/PlayerUpgradeStats")]
public class PlayerUpgradeStats : ScriptableObject
{
    public float shieldHealth;
    public float damageReductionPercentage;
    public float damageUpgradePercentage;
    public float criticalDamage;
    public float criticalDamageChance;
    public float chanceForDoubleTurns;
    public float lifeStealPercentage;
    public int bulletsPerShot;
    public float numberOfTurnsIncreasePercentage;
    public bool attackAfterKill;
    public bool bonusDamageOnLowHp;
    public bool extraDamageThisTurn;

}
