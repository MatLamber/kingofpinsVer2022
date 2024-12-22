using System;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour, MMEventListener<PlayerShootEvent>
{

    [SerializeField] private GameObject shootingMouth;
    [SerializeField] private GameObject hurtMouth;



    public void ShowShootingFace()
    {
        shootingMouth.SetActive(true);
        StartCoroutine(HideShootingFace());
    }

    IEnumerator HideShootingFace()
    {
        yield return new WaitForSeconds(0.5f);
        shootingMouth.SetActive(false);
    }
    
    public void ShowHurtFace()
    {
        hurtMouth.SetActive(true);
        StartCoroutine(HideHurtFace());
    }

    IEnumerator HideHurtFace()
    {
        yield return new WaitForSeconds(0.5f);
        hurtMouth.SetActive(false);
    }

    public void OnShooting()
    {
        ShowShootingFace();
    }

    public void OnHurt()
    {
        ShowHurtFace();
    }

    public void OnMMEvent(PlayerShootEvent eventType)
    {
        OnShooting();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<PlayerShootEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<PlayerShootEvent>();
    }
}
