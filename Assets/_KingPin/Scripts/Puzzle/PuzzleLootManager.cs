using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class PuzzleLootManager : MonoBehaviour,MMEventListener<LootInteraction>
{
    private List<Transform> lootList = new List<Transform>();


    private void Start()
    {
        InitializeLootList();
    }

    public void InitializeLootList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            lootList.Add(transform.GetChild(i));
        }
    }
    
    public void RemoveLoot(Transform loot)
    {
        if (lootList.Count > 0 && lootList.Contains(loot))
            lootList.Remove(loot);
        
        
        if(lootList.Count == 0)
            LootPoolEmptied.Trigger();
    }

    public void OnLootInteraction(Transform loot)
    {
        RemoveLoot(loot);
    }

    public void OnMMEvent(LootInteraction eventType)
    {
        OnLootInteraction(eventType.LootTransform);
    }

    private void OnEnable()
    {
        this.MMEventStartListening<LootInteraction>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<LootInteraction>();
    }
}
