using UnityEngine;
using MoreMountains.TopDownEngine;

public class DamageUpgradeManager : MonoBehaviour
{
    [Tooltip("Factor multiplicador del daño. Úsalo para aumentar o reducir el daño de este personaje específico.")]
    [SerializeField]
    private float DamageUpgradePercentage = 0f;

    public float damageUpgradePercentage
    {
        get => DamageUpgradePercentage;
        set => DamageUpgradePercentage = value;
    }

    /// <summary>
    /// Aplica el porcentaje de mejora de daño a un objeto DamageOnTouch.
    /// Este método se utiliza para este personaje específico.
    /// </summary>
    /// <param name="damageOnTouch">El componente DamageOnTouch del objeto asociado</param>
    public void ApplyDamageUpgrade(DamageOnTouch damageOnTouch)
    {
        if (damageOnTouch == null)
        {
            Debug.LogWarning("DamageOnTouch no asignado.");
            return;
        }

        // Calculamos el porcentaje adicional del daño
        float additionalMinDamage = damageOnTouch.MinDamageCaused * (DamageUpgradePercentage / 100f);
        float additionalMaxDamage = damageOnTouch.MaxDamageCaused * (DamageUpgradePercentage / 100f);

        // Sumamos el porcentaje al daño base
        damageOnTouch.MinDamageCaused =  (damageOnTouch.MinDamageCaused + additionalMinDamage);
        damageOnTouch.MaxDamageCaused = (damageOnTouch.MaxDamageCaused + additionalMaxDamage);
        

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

    /// <summary>
    /// Permite modificar el porcentaje de mejora dinámicamente para este personaje.
    /// </summary>
    /// <param name="newPercentage">El nuevo porcentaje multiplicador de daño</param>
    public void SetDamageUpgradePercentage(float newPercentage)
    {
        DamageUpgradePercentage = newPercentage;
    }
}