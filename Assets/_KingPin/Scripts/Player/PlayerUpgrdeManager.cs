using System;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerUpgrdeManager : MonoBehaviour, MMEventListener<DamagePercentageUpgrade>, 
    MMEventListener<CritDamageUpgrade>, 
    MMEventListener<CritChanceUpgrade>, 
    MMEventListener<BonusDamageOnLowHp>, 
    MMEventListener<BonusDamageOnWave>, 
    MMEventListener<MaximumHealth>, 
    MMEventListener<HealPercentage>, 
    MMEventListener<HealToPercentage>, 
    MMEventListener<Shield>, 
    MMEventListener<DamageReduce>, 
    MMEventListener<ExtraTurn>, 
    MMEventListener<AttackAfterKill>, 
    MMEventListener<LifeSteal>, 
    MMEventListener<ExtraBullet>,
    MMEventListener<EnemyFormationDefetead>
{
    
    [SerializeField] private PlayerUpgradeStats playerUpgradeStats;
    
    [SerializeField] private Health health;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerShootingManager playerShootingManager;
    [SerializeField] private DamageUpgradeManager damageUpgradeManager;
    [SerializeField] private MaximumHealthUpgradeManager maximumHealthUpgradeManager;
    [SerializeField] private CriticalUpgradeManager criticalUpgradeManager;
    [SerializeField] private DamageMultiplierOnLowHealth damageMultiplierOnLowHealth;
    [SerializeField] private DamageUpgradeONWave damageUpgradeOnWave;
    [SerializeField] private LifeStealUpgradeManager lifeStealUpgradeManager;

    
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        health.MaximumShield = playerUpgradeStats.shieldHealth;
        health.CurrentShield= playerUpgradeStats.shieldHealth;
        health.UpdateHealthBar(true);
        health.DamageReductionPercentage = playerUpgradeStats.damageReductionPercentage;
        playerController.NumberOfTurnsIncreasePercentage = playerUpgradeStats.numberOfTurnsIncreasePercentage;
        playerController.ExtraDamageThisTurn = playerUpgradeStats.extraDamageThisTurn;
        playerController.AttackIfKilledEnemy = playerUpgradeStats.attackAfterKill;
        playerShootingManager.BulletsPerShot = playerUpgradeStats.bulletsPerShot;

        damageUpgradeManager.damageUpgradePercentage = playerUpgradeStats.damageUpgradePercentage;
        criticalUpgradeManager.criticalUpgradeMultiplierPercentage = playerUpgradeStats.criticalDamage;
        criticalUpgradeManager.criticalChanceUpgradePercentage = playerUpgradeStats.criticalDamageChance;
        damageMultiplierOnLowHealth.upgradeActive = playerUpgradeStats.bonusDamageOnLowHp;
        lifeStealUpgradeManager.lifeStealPercentage = playerUpgradeStats.lifeStealPercentage;



    }

    private void OnEnable()
    {
        this.MMEventStartListening<DamagePercentageUpgrade>();
        this.MMEventStartListening<CritDamageUpgrade>();
        this.MMEventStartListening<CritChanceUpgrade>();
        this.MMEventStartListening<BonusDamageOnLowHp>();
        this.MMEventStartListening<BonusDamageOnWave>();
        this.MMEventStartListening<MaximumHealth>();
        this.MMEventStartListening<HealPercentage>();
        this.MMEventStartListening<HealToPercentage>();
        this.MMEventStartListening<Shield>();
        this.MMEventStartListening<DamageReduce>();
        this.MMEventStartListening<ExtraTurn>();
        this.MMEventStartListening<AttackAfterKill>();
        this.MMEventStartListening<LifeSteal>();
        this.MMEventStartListening<ExtraBullet>();
        this.MMEventStartListening<EnemyFormationDefetead>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<DamagePercentageUpgrade>();
        this.MMEventStopListening<CritDamageUpgrade>();
        this.MMEventStopListening<CritChanceUpgrade>();
        this.MMEventStopListening<BonusDamageOnLowHp>();
        this.MMEventStopListening<BonusDamageOnWave>();
        this.MMEventStopListening<MaximumHealth>();
        this.MMEventStopListening<HealPercentage>();
        this.MMEventStopListening<HealToPercentage>();
        this.MMEventStopListening<Shield>();
        this.MMEventStopListening<DamageReduce>();
        this.MMEventStopListening<ExtraTurn>();
        this.MMEventStopListening<AttackAfterKill>();
        this.MMEventStopListening<LifeSteal>();
        this.MMEventStopListening<ExtraBullet>();
        this.MMEventStopListening<EnemyFormationDefetead>();
    }

    public void OnMMEvent(DamagePercentageUpgrade eventType)
    {
        damageUpgradeManager.damageUpgradePercentage += eventType.Percentage;
        Debug.Log("Damage Percentage Upgrade: " + eventType.Percentage + ", New Value: " +
                  damageUpgradeManager.damageUpgradePercentage);
    }

    public void OnMMEvent(CritDamageUpgrade eventType)
    {
        criticalUpgradeManager.criticalUpgradeMultiplierPercentage += eventType.Percentage;
        Debug.Log("Crit Damage Upgrade: " + eventType.Percentage + ", New Value: " +
                  criticalUpgradeManager.criticalUpgradeMultiplierPercentage);
    }

    public void OnMMEvent(CritChanceUpgrade eventType)
    {
        criticalUpgradeManager.criticalChanceUpgradePercentage = eventType.CritChance;
        Debug.Log("Crit Chance Upgrade: " + eventType.CritChance + ", New Value: " +
                  criticalUpgradeManager.criticalChanceUpgradePercentage);
    }

    public void OnMMEvent(BonusDamageOnLowHp eventType)
    {
        damageMultiplierOnLowHealth.upgradeActive = true;
        Debug.Log("Bonus Damage on Low HP: " + eventType.UpgradeActive);
    }

    public void OnMMEvent(BonusDamageOnWave eventType)
    {
        damageUpgradeOnWave.UpgradeActive= true;
        Debug.Log("Bonus Damage on Wave activated.");
    }

    public void OnMMEvent(MaximumHealth eventType)
    {
        health.IncreaseMaximumHealthByPercentage(eventType.Percentage);
        Debug.Log("Maximum Health Increased by: " + eventType.Percentage + ", New Max Health: " +
                  health.MaximumHealth); // Show new max health
    }

    public void OnMMEvent(HealPercentage eventType)
    {
        health.HealPercentage(eventType.Percentage);
        Debug.Log("Healed by Percentage: " + eventType.Percentage + ", Current Health: " +
                  health.CurrentHealth); // Show current health
    }

    public void OnMMEvent(HealToPercentage eventType)
    {
        health.HealToPercentage(eventType.Percentage);
        Debug.Log("Healed to Percentage: " + eventType.Percentage + ", Current Health: " +
                  health.CurrentHealth); // Show current health
    }

    public void OnMMEvent(Shield eventType)
    {
        if (health.CurrentShield <= 0)
        {
            health.MaximumShield = eventType.ShieldPoints;
            health.CurrentShield = eventType.ShieldPoints;
        }
        else
        {
            health.MaximumShield += eventType.ShieldPoints;
            health.CurrentShield += eventType.ShieldPoints;
        }

        health.UpdateHealthBar(true);
        Debug.Log("Shield Updated: " + eventType.ShieldPoints);
    }

    public void OnMMEvent(DamageReduce eventType)
    {
        health.DamageReductionPercentage += eventType.Percentage;
        Debug.Log("Damage Reduction set to: " + eventType.Percentage);
    }

    public void OnMMEvent(ExtraTurn eventType)
    {
        playerController.NumberOfTurnsIncreasePercentage += eventType.Percentage;
        Debug.Log("Extra Turn Percentage set to: " + eventType.Percentage);
    }

    public void OnMMEvent(AttackAfterKill eventType)
    {
        playerController.AttackIfKilledEnemy = true;
        Debug.Log("Attack After Kill activated.");
    }

    public void OnMMEvent(LifeSteal eventType)
    {
        lifeStealUpgradeManager.lifeStealPercentage += eventType.Percentage;
        Debug.Log("Life Steal Percentage set to: " + eventType.Percentage);
    }

    public void OnMMEvent(ExtraBullet eventType)
    {
        playerShootingManager.BulletsPerShot++;
        Debug.Log("Extra Bullet added. Bullets per shot: " + playerShootingManager.BulletsPerShot);
    }

    public void OnMMEvent(EnemyFormationDefetead eventType)
    {
        damageUpgradeOnWave.UpgradeActive = false;
    }
}
