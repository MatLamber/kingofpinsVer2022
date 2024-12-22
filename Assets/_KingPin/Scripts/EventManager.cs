using MoreMountains.Tools;
using UnityEngine;


public enum UpgradeType
{
    DamagePercentage,
    CriticalDamagePercetange,
    CriticalChance,
    BonusDamageOnLowHp,
    BonusDamageOnNextWave,
    Shield,
    DamageReduction,
    MaxHealthIncrease,
    HealingPercentage,
    HealingToPercentage,
    BonusAttackChance,
    BonusAttackAfterKill,
    LifeSteal,
    ExtraProjectile
    
}

public struct PlayerGainedMoney
{
    public static PlayerGainedMoney e;
    public int amount;
    public static void Trigger(int amount)
    {
        e.amount = amount;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct PlayerMoneyUpdated
{
    public static PlayerMoneyUpdated e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct TravelingPhaseStarted
{
    public static TravelingPhaseStarted e;

    public static void Trigger()
    {
          MMEventManager.TriggerEvent(e);              
    }
}

public struct TravelingPhaseEnded
{
    public static TravelingPhaseEnded e;

    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct PuzzlePhaseStarted
{
    public static PuzzlePhaseStarted e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}


public struct PuzzlePhaseEnded
{
    public static PuzzlePhaseEnded e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct UpgradePhaseStarted
{
    public static UpgradePhaseStarted e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct UpgradePhaseEnded
{
    public static UpgradePhaseEnded e;

    public  static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct PinMovement
{
    public static PinMovement e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct PuzzleSolved
{
    public static PuzzleSolved e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct UpgradeOptionSelected
{
    public static UpgradeOptionSelected e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}


public struct EnemyMoving
{
    public static EnemyMoving e;
    public Transform CurrentTransform;

    public static void Trigger(Transform currentTransfrom)
    {
        e.CurrentTransform = currentTransfrom;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct EnemyAttacking
{
    public static EnemyAttacking e;
    public Transform CurrentTransform;

    public static void Trigger(Transform currentTransfrom)
    {
        e.CurrentTransform = currentTransfrom;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct PlayerTurnStarted
{
    public static PlayerTurnStarted e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct PlayerTurnEndend
{
    public static PlayerTurnEndend e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct EnemyDeath
{
    public static EnemyDeath e;
    public Enemy EnemyScript;
    public static void Trigger(Enemy enemyScript)
    {
        e.EnemyScript = enemyScript;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct PlayerDeath
{
    public static PlayerDeath e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct LootInteraction
{
    public static LootInteraction e;
    public Transform LootTransform;
    public static void Trigger(Transform lootTransform)
    {
        e.LootTransform = lootTransform;
        MMEventManager.TriggerEvent(e);              
    }
}


public struct LootPoolEmptied
{
    public static LootPoolEmptied e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct EnemyFormationDefetead
{
    public static EnemyFormationDefetead e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);              
    }
}

public struct DamagePercentageUpgrade
{
    public static DamagePercentageUpgrade e;
    public float Percentage;
    public static void Trigger(float percentage)
    {
        e.Percentage = percentage;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct CritDamageUpgrade
{
    public static CritDamageUpgrade e;
    public float Percentage;
    public static void Trigger(float percentage)
    {
        e.Percentage = percentage;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct CritChanceUpgrade
{
    public static CritChanceUpgrade e;
    public float CritChance;
    public static void Trigger(float critChance)
    {
        e.CritChance = critChance;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct BonusDamageOnLowHp
{
    public static BonusDamageOnLowHp e;
    public bool UpgradeActive;
    public static void Trigger(bool upgradeActive)
    {
        e.UpgradeActive = upgradeActive;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct BonusDamageOnWave
{
    public static BonusDamageOnWave e;
    public bool UpgradeActive;
    public static void Trigger(bool upgradeActive)
    {
        e.UpgradeActive = upgradeActive;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct MaximumHealth
{
    public static MaximumHealth e;
    public float Percentage;
    public static void Trigger(float percentage)
    {
        e.Percentage = percentage;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct HealPercentage
{
    public static HealPercentage e;
    public float Percentage;
    public static void Trigger(float percentage)
    {
        e.Percentage = percentage;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct HealToPercentage
{
    public static HealToPercentage e;
    public float Percentage;
    public static void Trigger(float percentage)
    {
        e.Percentage = percentage;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct Shield
{
    public static Shield e;
    public float ShieldPoints;
    public static void Trigger(float shieldPoints)
    {
        e.ShieldPoints = shieldPoints;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct DamageReduce
{
    public static DamageReduce e;
    public float Percentage;
    public static void Trigger(float percentage)
    {
        e.Percentage = percentage;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct ExtraTurn
{
    public static ExtraTurn e;
    public float Percentage;
    public static void Trigger(float percentage)
    {
        e.Percentage = percentage;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct AttackAfterKill
{
    public static AttackAfterKill e;
    public bool ShouldAttack;

    public static void Trigger(bool shouldAttack)
    {
        e.ShouldAttack = shouldAttack;
        MMEventManager.TriggerEvent(e);
    }
}

public struct LifeSteal
{
    public static LifeSteal e;
    public float Percentage;
    public static void Trigger(float percentage)
    {
        e.Percentage = percentage;
        MMEventManager.TriggerEvent(e);              
    }
}

public struct ExtraBullet
{
    public static ExtraBullet e;
    public bool UpdateActive;
    public static void Trigger(bool updateActive)
    {
        e.UpdateActive = updateActive;
        MMEventManager.TriggerEvent(e);
    }
}
public class EventManager
{
        
}
