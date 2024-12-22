using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerMoneyCollectorManager : MonoBehaviour
{
    [SerializeField] private MMF_Player LootCollectedFeedbacks;
    private string lootTag = "Money";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(lootTag))
            PlayCollectedLootFeeedbacks();
    }

    public void PlayCollectedLootFeeedbacks()
    {
        LootCollectedFeedbacks.PlayFeedbacks();
    }
}
