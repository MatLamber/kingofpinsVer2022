using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Legendary
}

[CreateAssetMenu(fileName = "UpgradeCard", menuName = "Scriptable Objects/UpgradeCard")]
public class UpgradeCard : ScriptableObject
{
    public Sprite background;
    public Sprite icon;
    public string description;
    public UpgradeType upgradeType;
    public Rarity rarity;
    public int cost;
    public int numberOfUses;
    public int timesSelected;
    public UpgradeEvent upgradeEvent;
}
