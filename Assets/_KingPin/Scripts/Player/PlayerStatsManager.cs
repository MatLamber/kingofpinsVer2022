using System;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour, MMEventListener<PlayerGainedMoney>
{
    private int money;

    private void Start()
    {
        money = KingOfPinsData.Instance.Money;
        GameManager.Instance.PlayerMoney = money;
        UIManager.Instance.UpdateMoneyText(money.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMoney(100);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        GameManager.Instance.PlayerMoney = money;
        UIManager.Instance.UpdateMoneyText(money.ToString());
        PlayerMoneyUpdated.Trigger();
    }
    

    public void OnPlayerGainedMoney(int amount)
    {
        AddMoney(amount);
    }
    public void OnMMEvent(PlayerGainedMoney eventType)
    {
            OnPlayerGainedMoney(eventType.amount);
    }

    private void OnEnable()
    {
        this.MMEventStartListening<PlayerGainedMoney>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<PlayerGainedMoney>();
    }
}
