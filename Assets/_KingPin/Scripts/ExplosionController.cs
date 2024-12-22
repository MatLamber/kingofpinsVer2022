using System;
using System.Collections;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private string lootTagName = "Money";
    private string blockingTag = "Pin";

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the specified tag for deactivation
        if (other.CompareTag(lootTagName))
        {
            // Calculate direction from this object to the target
            Vector3 direction = other.transform.position - transform.position;
            // Cast a ray to see if there's any object with the blocking tag in the way
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, direction.magnitude))
            {
                Debug.Log($"Hit {hit.collider.name}");
                // If a hit is found and it's not the target itself, check if it's a blocker
                if (hit.collider.CompareTag(blockingTag))
                {
                    return; // Do not deactivate, a blocker is in the way
                }
            }

            // No blockers found, deactivate the object
            other.gameObject.SetActive(false);
        }
    }
}
