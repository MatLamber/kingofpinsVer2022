using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LootController : MonoBehaviour
{
    private string moneyCollectorTag = "MoneyCollector";
    private Vector3 originalScale;
    private Rigidbody rigidbody;
    private bool wentToPlayer;
    private void Start()
    {
        originalScale = transform.localScale;
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(EnableRigidBody());
    }


    IEnumerator EnableRigidBody()
    {
        yield return new WaitForSeconds(0.5f);
        rigidbody.isKinematic = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(moneyCollectorTag))
        {
            if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
                wentToPlayer = true;
            rigidbody.isKinematic = true;
            GoToTarget(other.transform.position);
            Shrink();
            
        }
  
    }

    public void Shrink ()
    {
        transform.DOScale(Vector3.zero, 0.2f).OnComplete((() =>
        {
            if(wentToPlayer)
                PlayerGainedMoney.Trigger(20);
            gameObject.SetActive(false);
        })).SetDelay(0.1f);
    }

    public void GoToTarget(Vector3 targetPosion)
    {
        transform.DOMove(targetPosion, 0.3f);
    }

    private void OnDisable()
    {
        LootInteraction.Trigger(this.transform);
        transform.localScale = originalScale;
        rigidbody.isKinematic = false;

    }
}
