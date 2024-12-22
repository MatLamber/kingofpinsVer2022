using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class LootBombController : MonoBehaviour,  MMEventListener<PuzzleSolved>
{
    [SerializeField] private MMF_Player igniteFeedBack;
    [SerializeField] private float scaleMultiplier;
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject explosion2D;
    [SerializeField] private MMF_Player explosionFeedback;

    [SerializeField] private float graceTime = 3.0f; // Tiempo de gracia después de registrar los colliders iniciales

    private HashSet<Collider> initialColliders = new HashSet<Collider>();
    private bool isIgniteMode = false;
    private bool checkForNewCollisions = false;
    private float elapsedTime = 0.0f;
    private bool hasExploded;
    private bool puzzleSolved;

    private void Start()
    {
        RegisterInitialCollisions();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<PuzzleSolved>();
    }
    

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= graceTime && !checkForNewCollisions)
        {
            checkForNewCollisions = true; // Activar la verificación de nuevas colisiones después del tiempo de gracia
            Debug.Log("Tiempo de gracia terminado. Listo para verificar nuevas colisiones.");
        }
    }

    private void RegisterInitialCollisions()
    {
        Collider[]
            colliders = Physics.OverlapSphere(transform.position, 0.5f); // Asume un radio pequeño alrededor del objeto
        foreach (Collider collider in colliders)
        {
            initialColliders.Add(collider);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (checkForNewCollisions && !initialColliders.Contains(collision.collider) && !isIgniteMode)
        {
            OnIgnite();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickeablesZone"))
        {
            if(hasExploded) return;
            hasExploded = true;
            GetComponent<Rigidbody>().isKinematic = true;
            DOTween.Kill(transform);
            transform.DOMove(other.transform.position, 0.2f).SetEase(Ease.Linear).OnComplete((() =>
            {
                explosionFeedback.PlayFeedbacks();
                bomb.SetActive(false);
                explosion2D.SetActive(true);
                StartCoroutine(DisableBomb());
            })); 


        }
    }

    IEnumerator DisableBomb()
    {
        yield return new WaitForSeconds(0.1f);
        explosion2D.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        this.MMEventStopListening<PuzzleSolved>();
        //StopCoroutine(DisableBomb());
    }

    private void OnIgnite()
    {
        if(puzzleSolved) return;
        if(hasExploded) return;
        transform.SetParent(null);
        isIgniteMode = true;
        Debug.Log("La bomba estalla.");
        igniteFeedBack?.PlayFeedbacks();
        transform.DOScale(transform.localScale * scaleMultiplier, 2f).OnComplete(() =>
        {
            explosionFeedback.PlayFeedbacks();
            bomb.SetActive(false);

            explosion.SetActive(true);
            hasExploded = true;
            
            StartCoroutine(DisableBomb());
            
        });
        // Opcional: Desactiva el script o realiza otras acciones post-explosión
    }

    public void OnMMEvent(PuzzleSolved eventType)
    {
        puzzleSolved = true;
        if(isIgniteMode)
            StopCoroutine(DisableBomb());
    }
}