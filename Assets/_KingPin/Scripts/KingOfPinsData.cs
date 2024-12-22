using MoreMountains.Tools;
using UnityEngine;

public class KingOfPinsData : MMPersistentSingleton<KingOfPinsData>
{
    private int money;
    private int lastLevelUnlocked;
    
    public int Money
    {
        get => money;
        set => money = value;
    }

    public int LastLevelUnlocked
    {
        get => lastLevelUnlocked;
        set => lastLevelUnlocked = value;
    }
}
