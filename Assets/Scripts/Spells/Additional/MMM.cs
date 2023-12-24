using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMM : MonoBehaviour
{
    private int fireDamage = 0;

    public void SetValues(int fireDamage)
    {
        this.fireDamage = fireDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemysHealth enemysHealth = other.GetComponent<EnemysHealth>();

            if( enemysHealth != null)
            {
                enemysHealth.FireDamage(fireDamage);
            }

        }
    }
}
