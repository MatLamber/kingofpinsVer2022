using System;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardUI : MonoBehaviour, MMEventListener<PlayerMoneyUpdated>
{
    [Header("UI Components")] [SerializeField]
    private Image backgroundImage;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI costText;
    private UpgradeCard currentUpgradeCard;
    private Button cardButton;

    private void Awake()
    {
        cardButton = GetComponent<Button>();
        cardButton.onClick.AddListener(HandleCardClick);
    }

    public void SetCard(UpgradeCard upgradeCard)
    {
        backgroundImage.sprite = upgradeCard.background;
        iconImage.sprite = upgradeCard.icon;
        descriptionText.text = upgradeCard.description;
        rarityText.text = upgradeCard.rarity.ToString().ToUpper();
        costText.text = upgradeCard.cost.ToString();
        currentUpgradeCard = upgradeCard;
        CheckIfPlayerCanBuyUpgrade();
    }

    public void CheckIfPlayerCanBuyUpgrade()
    {
        if (currentUpgradeCard == null) return;
        if (GameManager.Instance.PlayerMoney < currentUpgradeCard.cost)
        {
            cardButton.interactable = false;
            costText.color = Color.red;
        }
        else
        {
            cardButton.interactable = true;
            costText.color = Color.white;
        }

    }

    private void HandleCardClick()
    {
        if (currentUpgradeCard == null || currentUpgradeCard.upgradeEvent == null)
        {
            Debug.LogWarning("No se ha configurado un evento o la carta no está asignada.");
            return;
        }

        currentUpgradeCard.timesSelected++;
        currentUpgradeCard.upgradeEvent.Trigger(); // Dispara el evento configurado en la carta
        PlayerGainedMoney.Trigger(-currentUpgradeCard.cost);
    }

    public void OnMMEvent(PlayerMoneyUpdated eventType)
    {
        //CheckIfPlayerCanBuyUpgrade();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<PlayerMoneyUpdated>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<PlayerMoneyUpdated>();
    }
}