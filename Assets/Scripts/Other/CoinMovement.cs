using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

public class CoinMovement : MonoBehaviour
{
    private bool isSee = false;
    private float moveSpeed = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSee = true;
            StartCoroutine(Move(other.transform));
        }
    }

    private IEnumerator Move(Transform characterGirl)
    {
        while(true)
        {
            Vector3 direction = characterGirl.position - transform.position;
            direction.y += 1f;
            direction.Normalize();
            transform.position += moveSpeed * direction * Time.deltaTime;
            if (Vector3.Distance(characterGirl.position, transform.position) < 1f) break;
            yield return new WaitForEndOfFrame();
        }
        characterGirl.GetComponent<ExpendableResources>().PlusCoin();
        Destroy(this.gameObject);
    }
}
