using MoreMountains.TopDownEngine;
using UnityEngine;

public class DamageMultiplierOnLowHealth : MonoBehaviour
{
    [Tooltip("Booleano que determian si aplica no el buff de ataque.")]
    [SerializeField]
    private bool UpgradeActive;

    public bool upgradeActive
    {
        get => UpgradeActive;
        set => UpgradeActive = value;
    }

    [SerializeField] private Health health;
    
    public void ApplyDamageUpgrade(DamageOnTouch damageOnTouch)
    {
        if (damageOnTouch == null)
        {
            Debug.LogWarning("DamageOnTouch no asignado.");
            return;
        }
        
        if(!upgradeActive) return;
        if(!health.CheckLowHealth()) return;
        // Sumamos el porcentaje al daño base
        damageOnTouch.MinDamageCaused =(damageOnTouch.MinDamageCaused * 2);
        damageOnTouch.MaxDamageCaused = (damageOnTouch.MaxDamageCaused * 2);
    }

    /// <summary>
    /// Restaura el daño original en un objeto DamageOnTouch (sin el porcentaje adicional).
    /// Esto es útil cuando quieres desactivar mejoras o upgrades temporales para este personaje.
    /// </summary>
    /// <param name="damageOnTouch">El componente DamageOnTouch del objeto</param>
    /// <param name="originalMinDamage">El daño mínimo original</param>
    /// <param name="originalMaxDamage">El daño máximo original</param>
    public void ResetDamage(DamageOnTouch damageOnTouch, float originalMinDamage, float originalMaxDamage)
    {
        if (damageOnTouch == null)
        {
            Debug.LogWarning("DamageOnTouch no asignado.");
            return;
        }

        damageOnTouch.MinDamageCaused = originalMinDamage;
        damageOnTouch.MaxDamageCaused = originalMaxDamage;
    }


}
