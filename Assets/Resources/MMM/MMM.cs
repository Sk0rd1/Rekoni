using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMM : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemysHealth enemysHealth = other.GetComponent<EnemysHealth>();

            if( enemysHealth != null)
            {
                enemysHealth.PoisonDamage(10, 1, 4);
            }

        }
    }
}
