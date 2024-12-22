using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, MMEventListener<TravelingPhaseStarted>
{

    [SerializeField] private List<GameObject> enemiesFormation;
    private int currentEnemysFormation;

    private void Start()
    {
        if(GameManager.Instance is not null)
            GameManager.Instance.InitializeEnemies(enemiesFormation);
    }

    IEnumerator SendNextEnemieFormation()
{
    yield return new WaitForSeconds(1.7f);

    for(int i = 0; i < enemiesFormation.Count; i++)
    {
        enemiesFormation[i].SetActive(i == currentEnemysFormation);
    }

    currentEnemysFormation++;
}

    public void OnTravelingPhaseStarted()
    {
        StartCoroutine(SendNextEnemieFormation());
    }

    public void OnMMEvent(TravelingPhaseStarted eventType)
    {
        OnTravelingPhaseStarted();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<TravelingPhaseStarted>();
    }
    
    private void OnDisable()
    {
        this.MMEventStopListening<TravelingPhaseStarted>();
    }
}
