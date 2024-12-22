using UnityEngine;
using MoreMountains.TopDownEngine;

public class CriticalUpgradeManager : MonoBehaviour
{
    [Tooltip("Incremento adicional al multiplicador de daño crítico (en porcentaje).")]
    [SerializeField]
    private float CriticalUpgradeMultiplierPercentage = 0f;

    [Tooltip("Incremento adicional a la probabilidad de daño crítico (en porcentaje).")]
    [SerializeField]
    private float CriticalChanceUpgradePercentage = 0f;


    public float criticalUpgradeMultiplierPercentage
    {
        get => CriticalUpgradeMultiplierPercentage;
        set => CriticalUpgradeMultiplierPercentage = value;
    }

    public float criticalChanceUpgradePercentage
    {
        get => CriticalChanceUpgradePercentage;
        set => CriticalChanceUpgradePercentage = value;
    }

    /// <summary>
    /// Aplica las mejoras críticas (multiplicador y chance) dinámicamente a un objeto DamageOnTouch.
    /// Este método se utiliza para configurar los parámetros de crítico de forma personalizada.
    /// </summary>
    /// <param name="damageOnTouch">El componente DamageOnTouch asociado al objeto</param>
    public void ApplyCriticalUpgrade(DamageOnTouch damageOnTouch)
    {
        if (damageOnTouch == null)
        {
            Debug.LogWarning("DamageOnTouch no asignado.");
            return;
        }

        // Incrementamos el multiplicador crítico
        float additionalMultiplier = damageOnTouch.CriticalDamageBase * (CriticalUpgradeMultiplierPercentage / 100f);
        damageOnTouch.CriticalDamageBase = (damageOnTouch.CriticalDamageBase + additionalMultiplier); 

        // Incrementamos la probabilidad crítica base
        damageOnTouch.CriticalChancePercentage = CriticalChanceUpgradePercentage;

        Debug.Log($"Aplicadas mejoras críticas - Nuevo multiplicador base: {damageOnTouch.CriticalDamageBase}, " +
                  $"Nueva probabilidad crítica: {damageOnTouch.CriticalChancePercentage}%");
    }

    /// <summary>
    /// Restaura las estadísticas originales críticas de DamageOnTouch.
    /// Esto elimina cualquier mejora crítica aplicada.
    /// </summary>
    /// <param name="damageOnTouch">El componente DamageOnTouch asociado</param>
    /// <param name="originalCriticalMultiplier">El multiplicador crítico original</param>
    /// <param name="originalCriticalChance">La probabilidad de crítico original</param>
    public void ResetCriticalStats(DamageOnTouch damageOnTouch, float originalCriticalMultiplier, float originalCriticalChance)
    {
        if (damageOnTouch == null)
        {
            Debug.LogWarning("DamageOnTouch no asignado.");
            return;
        }

        damageOnTouch.CriticalDamageBase = originalCriticalMultiplier;
        damageOnTouch.CriticalChancePercentage = originalCriticalChance;

        Debug.Log($"Restaurados valores críticos originales - Multiplicador base: {originalCriticalMultiplier}, " +
                  $"Probabilidad crítica: {originalCriticalChance}%");
    }

    /// <summary>
    /// Permite modificar dinámicamente el porcentaje de mejora del multiplicador crítico.
    /// </summary>
    /// <param name="newMultiplierPercentage">El nuevo porcentaje multiplicador crítico</param>
    public void SetCriticalMultiplierUpgradePercentage(float newMultiplierPercentage)
    {
        CriticalUpgradeMultiplierPercentage = newMultiplierPercentage;
        Debug.Log($"Nuevo porcentaje de mejora del multiplicador crítico: {CriticalUpgradeMultiplierPercentage}%");
    }

    /// <summary>
    /// Permite modificar dinámicamente el porcentaje de mejora para la probabilidad crítica.
    /// </summary>
    /// <param name="newChancePercentage">El nuevo porcentaje de probabilidad crítica</param>
    public void SetCriticalChanceUpgradePercentage(float newChancePercentage)
    {
        CriticalChanceUpgradePercentage = newChancePercentage;
        Debug.Log($"Nuevo porcentaje de mejora de la probabilidad crítica: {CriticalChanceUpgradePercentage}%");
    }
}