using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private bool turnFinished;

    public bool TurnFinished
    {
        get => turnFinished;
        set => turnFinished = value;
    }

    public virtual void TakeTurn()
    {

    }

    public virtual void MoveToTarget()
    {
        
    }

    public virtual void DetermineDistanceToTarget()
    {
        
    }

    public virtual void Attack()
    {
        
    }

    public virtual void OnDeath()
    {
        
    }
}