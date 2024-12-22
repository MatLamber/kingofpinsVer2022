using System;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour, MMEventListener<PlayerShootEvent>,
    MMEventListener<TravelingPhaseStarted>, MMEventListener<TravelingPhaseEnded>
{
    [SerializeField] private Animator animator;
    private string stateParameter = "State";
    private string fireParatemeter = "Fire";
    private string hitParatemeter = "Hit";
    private int stateValue = 1;

    public void PlayReadyAnimation()
    {
        if (animator is not null)
        {
            PlayMovementStateAnimation(1);
        }
        else
        {
            Debug.LogError($"No se seteo un animator en: {gameObject.name}");
        }
    }


    public void PlayMovementStateAnimation(int state)
    {
        if (animator is not null)
        {
            stateValue = state;
            animator.SetInteger(stateParameter, stateValue);
        }
        else
        {
            Debug.LogError($"No se seteo un animator en: {gameObject.name}");
        }
    }

    public void PlayFireAnimation()
    {
        if (animator is not null)
        {
            animator.SetTrigger(fireParatemeter);
        }
        else
        {
            Debug.LogError($"No se seteo un animator en: {gameObject.name}");
        }
    }

    public void PlayRunAnimation()
    {
        
        Debug.Log($"Run");
        if (animator is not null)
        {
            PlayMovementStateAnimation(2);
        }
        else
        {
            Debug.LogError($"No se seteo un animator en: {gameObject.name}");
        }
    }

    public void PlayHitAnimation()
    {
        if (animator is not null)
        {
            animator.SetTrigger(hitParatemeter);
        }
        else
        {
            Debug.LogError($"No se seteo un animator en: {gameObject.name}");
        }
    }


    public void OnMMEvent(PlayerShootEvent eventType)
    {
        PlayFireAnimation();
    }
    
    public void OnMMEvent(TravelingPhaseStarted eventType)
    {
        PlayRunAnimation();
    }

    public void OnMMEvent(TravelingPhaseEnded eventType)
    {
        PlayReadyAnimation();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<PlayerShootEvent>();
        this.MMEventStartListening<TravelingPhaseStarted>();
        this.MMEventStartListening<TravelingPhaseEnded>();
        
    }

    private void OnDisable()
    {
        this.MMEventStopListening<PlayerShootEvent>();
        this.MMEventStopListening<TravelingPhaseStarted>();
        this.MMEventStopListening<TravelingPhaseEnded>();
    }


}