using MoreMountains.TopDownEngine;
using UnityEngine;

public class LifeStealUpgradeManager : MonoBehaviour
{
    
    [Tooltip("Factor multiplicador del LifeSteal")]
    [SerializeField]
    private float LifeStealPercentage = 0f;

    public float lifeStealPercentage
    {
        get => LifeStealPercentage;
        set => LifeStealPercentage = value;
    }

    /// <summary>
    /// Aplica el porcentaje de mejora de daño a un objeto DamageOnTouch.
    /// Este método se utiliza para este personaje específico.
    /// </summary>
    /// <param name="damageOnTouch">El componente DamageOnTouch del objeto asociado</param>
    public void ApplyLifeStealUpgrade(DamageOnTouch damageOnTouch)
    {
        if (damageOnTouch == null)
        {
            Debug.LogWarning("DamageOnTouch no asignado.");
            return;
        }
        damageOnTouch.LifeStealEnabled = true;
        damageOnTouch.LifeStealPercentage = LifeStealPercentage;
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
    public void SetLifeStealUpgradePercentage(float newPercentage)
    {
        LifeStealPercentage = newPercentage;
    }
}
