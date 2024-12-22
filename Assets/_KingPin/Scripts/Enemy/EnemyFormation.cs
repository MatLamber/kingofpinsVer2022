using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private List<Enemy> enemies;
    
    public List<Enemy> Enemies => enemies;

    private void OnEnable()
    {
        MoveToTarget();
    }


    public void MoveToTarget()
    {
        transform.SetParent(target);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }
    
}
