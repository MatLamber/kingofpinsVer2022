using System;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class MaximumHealthUpgradeManager : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float healthPercentage = 50;


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
            UpgradeMaximumHealth();
    }

    public void UpgradeMaximumHealth()
    {
        if(health is not null)
            health.IncreaseMaximumHealthByPercentage(healthPercentage);
    }
}
