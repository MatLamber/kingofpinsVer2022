using System;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class EnemyShootingManager : MonoBehaviour
{

    [SerializeField] private Transform weaponHolder;
    private Weapon weapon;
    private WeaponAutoAim2D autoAim2D;
    private int bulletsPerShot = 1;

    public int BulletsPerShot
    {
        get => bulletsPerShot;
        set => bulletsPerShot = value;
    }

    void Start()
    {
        if (weaponHolder)
        {
            StartCoroutine(SetWeapon());
        }
        else
        {
            Debug.LogError($"No se seteo un Weapon Holder en: {gameObject.name}");
        }
    }

    IEnumerator SetWeapon()
    {
        yield return new WaitUntil(() => weaponHolder.transform.childCount > 0);
        if (weaponHolder.transform.GetChild(0).GetComponent<Weapon>())
        {
            weapon = weaponHolder.transform.GetChild(0).GetComponent<Weapon>();
        }
        else
        {
            Debug.LogError("El primer hijo del Weapon Holder no tiene un componenete Weapon.");
        }
        
        if (weaponHolder.transform.GetChild(0).GetComponent<WeaponAutoAim2D>())
        {
            autoAim2D = weaponHolder.transform.GetChild(0).GetComponent<WeaponAutoAim2D>();
        }
        else
        {
            Debug.LogError("El primer hijo del Weapon Holder no tiene un componenete AutoAim2D.");
        }
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            ShootWithOutTarget();
    }


    public void Shoot()
    {
        Debug.Log("Shoot");
        weapon.BurstLength = bulletsPerShot;
        if(autoAim2D.Target is null) return;
        Invoke(nameof(ShootRequest),0f);
    }
    
    public void ShootWithOutTarget()
    {
        weapon.BurstLength = bulletsPerShot;
        //if(autoAim2D.Target is null) return;
        Invoke(nameof(ShootRequest),0f);
    }

    public void EnableAiming()
    {
        weapon.GetComponent<WeaponAim2D>().enabled = true;
    }

    public void DisableAiming()
    {
        weapon.GetComponent<WeaponAim2D>().enabled = false;
    }

    public void ShootRequest()
    {
        if (weapon)
        {
            weapon.WeaponInputStart(); 
        }
        else
        {
            Debug.LogError($"No se seteo ningun arma");
        }
    }

    private void OnTurnStarted()
    {
        EnableAiming();
    }

    private void OnTurnEnded()
    {
        DisableAiming();
    }
    
    
}
