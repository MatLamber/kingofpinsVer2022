using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools; // Necesario para MMEventManager
using System.Linq;
using DG.Tweening;

public class UpgradeCardUIManager : MonoBehaviour, MMEventListener<UpgradePhaseStarted>, MMEventListener<UpgradePhaseEnded>
{
    [Header("Card Settings")] [SerializeField]
    private UpgradeCardUI[] cardUIs; // Referencias a las tarjetas existentes en el Canvas

    [SerializeField] private UpgradeCard[] allUpgradeCards; // Todos los ScriptableObjects disponibles
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject rerollButton;

    [Header("Rarity Probabilities")] [SerializeField, Range(0f, 100f)]
    private float commonProbability = 90f;

    [SerializeField, Range(0f, 100f)] private float rareProbability = 7f;
    [SerializeField, Range(0f, 100f)] private float legendaryProbability = 3f;
    
    

    private Dictionary<UpgradeCard, int> _upgradeUses = new Dictionary<UpgradeCard, int>();
    
    private Vector3 _originalCardScale;

    
    private void Start()
    {
        ResetTimesSelected();
                _originalCardScale = cardUIs[0].transform.localScale; // Store the original scale
    }

    private void ResetTimesSelected()
    {
        foreach (var card in allUpgradeCards)
        {
            card.timesSelected = 0;
        }
    }
    private void OnEnable()
    {
        // Suscripción al evento UpgradePhaseStarted
        this.MMEventStartListening<UpgradePhaseStarted>();
        this.MMEventStartListening<UpgradePhaseEnded>();
    }

    private void OnDisable()
    {
        // Desuscribirse del evento UpgradePhaseStarted
        this.MMEventStopListening<UpgradePhaseStarted>();
        this.MMEventStopListening<UpgradePhaseEnded>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetUpCards();
        }
    }

    // Método que se ejecuta cuando se dispara el evento UpgradePhaseStarted
    public void OnMMEvent(UpgradePhaseStarted phaseEvent)
    {
        SetUpCards(); // Reconfigura las tarjetas
        skipButton.SetActive(false);
        rerollButton.SetActive(false);
        skipButton.transform.localScale = Vector3.zero;
        rerollButton.transform.localScale = Vector3.zero;
        skipButton.SetActive(true);
        rerollButton.SetActive(true);
        skipButton.transform.DOScale(Vector3.one, 0.5f).SetDelay(0.6f).SetEase(Ease.OutBack);
        rerollButton.transform.DOScale(Vector3.one, 0.5f).SetDelay(0.5f).SetEase(Ease.OutBack);
    }
    
    public void OnMMEvent(UpgradePhaseEnded eventType)
    {
        skipButton.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
        rerollButton.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
    }

    public void OnRerollButtonPressed()
    {
        SetUpCards();
        rerollButton.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
    }

    // Método para configurar las tarjetas
    public void SetUpCards()
    {
        if (allUpgradeCards.Length < 3)
        {
            Debug.LogError("No hay suficientes ScriptableObjects.");
            return;
        }

        // Lista para almacenar las cartas seleccionadas (para evitar repeticiones)
        List<UpgradeCard> selectedCards = new List<UpgradeCard>();
        // Copia de todas las cartas disponibles para ir eliminando las seleccionadas
        List<UpgradeCard> availableCards = allUpgradeCards.Where(card =>
            card.numberOfUses == 0 || (card.numberOfUses > 0 && card.timesSelected < card.numberOfUses)).ToList();
        for (int i = 0; i < cardUIs.Length; i++)
        {
            cardUIs[i].transform.localScale = Vector3.zero;
            cardUIs[i].transform.DOScale(_originalCardScale, 0.5f).SetDelay(i * 0.2f).SetEase(ease: Ease.OutBack);
        }

        for (int i = 0;
             i < Mathf.Min(cardUIs.Length, 3) && availableCards.Count > 0;
             i++) // Obtener como maximo 3 cartas
        {
            UpgradeCard selectedCard = GetRandomCardByRarity(availableCards);

            if (selectedCard != null)
            {
                selectedCards.Add(selectedCard);
                if (selectedCard.numberOfUses > 0)
                {
                    if (!_upgradeUses.ContainsKey(selectedCard))
                    {
                        _upgradeUses[selectedCard] = 0;
                    }

                    _upgradeUses[selectedCard]++;
                }

                availableCards.Remove(selectedCard); // Eliminar la carta seleccionada de las disponibles
            }
            else
            {
                //Si ya no hay cartas disponibles para una rareza, deja de intentar generar más cartas
                break;
            }
        }

        for (int i = 0; i < cardUIs.Length && i < selectedCards.Count; i++)
        {
            cardUIs[i].SetCard(selectedCards[i]);
        }

        if (cardUIs.Length > selectedCards.Count)
        {
            Debug.LogWarning("Algunas tarjetas en el Canvas quedaron sin datos.");
        }
    }

    private UpgradeCard GetRandomCardByRarity(List<UpgradeCard> availableCards)
    {
        float randomValue = Random.Range(0f, 100f);
        Rarity targetRarity;

        if (randomValue < commonProbability)
        {
            targetRarity = Rarity.Common;
        }
        else if (randomValue < commonProbability + rareProbability)
        {
            targetRarity = Rarity.Rare;
        }
        else
        {
            targetRarity = Rarity.Legendary;
        }

        return GetRandomCardOfRarity(targetRarity, availableCards);
    }


    private UpgradeCard GetRandomCardOfRarity(Rarity rarity, List<UpgradeCard> availableCards)
    {
        var cardsOfRarity = availableCards.Where(card => card.rarity == rarity).ToArray();

        if (cardsOfRarity.Length == 0)
        {
            Debug.LogWarning($"No hay cartas disponibles de rareza {rarity}.");
            return null;
        }

        int randomIndex = Random.Range(0, cardsOfRarity.Length);
        return cardsOfRarity[randomIndex];
    }


}