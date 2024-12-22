using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour, MMEventListener<EnemyMoving>, MMEventListener<EnemyAttacking>
{
    [SerializeField] private Animator animator;
    private string stateParameter = "State";
    private string slashParatemeter = "Slash2H";
    private string hitParatemeter = "Hit";
    private string actionParameter = "Action";
    private int stateValue = 1;
    
    IEnumerator SpawnMovementAnimationCylce()
    {
        PlayMovementStateAnimation(2);
        yield return new WaitForSeconds(0.5f);
        PlayMovementStateAnimation(1);
    }

    public void OnEnemyMoving()
    {
        StartCoroutine(SpawnMovementAnimationCylce());
    }

    public void OnAttacking()
    {
        PlaySlashAnimation();
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

    public void OnHit()
    {
        PlayHitAnimation();
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

    public void PlaySlashAnimation()
    {
        if (animator is not null)
        {
            animator.SetBool(actionParameter,true);
            animator.SetTrigger(slashParatemeter);
            StartCoroutine(ResetActionParameter());
        }
        else
        {
            Debug.LogError($"No se seteo un animator en: {gameObject.name}");
        }
    }

    IEnumerator ResetActionParameter(float delay = 0.6f)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(actionParameter,false);
    }

    public void OnMMEvent(EnemyMoving eventType)
    {
        if(this.transform == eventType.CurrentTransform)
            OnEnemyMoving();
    }
    
    
    public void OnMMEvent(EnemyAttacking eventType)
    {
        if(this.transform == eventType.CurrentTransform)
            OnAttacking();
    }
    
    
    private void OnEnable()
    {
        StartCoroutine(SpawnMovementAnimationCylce());
        this.MMEventStartListening<EnemyMoving>();
        this.MMEventStartListening<EnemyAttacking>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<EnemyMoving>();
        this.MMEventStopListening<EnemyAttacking>();
    }



}