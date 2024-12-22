using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public Health health;
    [SerializeField] private EnemyShootingManager shootingManager;
    [SerializeField] private Animator animator;
    [SerializeField] private string shootBoeParameter = "ShotBow";
    public override void TakeTurn()
    {
        base.TakeTurn();
        TurnFinished = false;
        StartCoroutine(ManagaAttackTiming());
    }

    public override void Attack()
    {
        base.Attack();
        if (shootingManager == null) return;
        shootingManager.Shoot();
  

    }


    IEnumerator ManagaAttackTiming()
    {
        animator.SetTrigger(shootBoeParameter);
        yield return new WaitForSeconds(0.2f);
        Attack();
        yield return new WaitForSeconds(0.2f);
        TurnFinished = true;
    }
    
    public override void OnDeath()
    {
        base.OnDeath();
        EnemyDeath.Trigger(this);

    }
}
