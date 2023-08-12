using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMM : MonoBehaviour
{
    private int startDamage = 0;
    private int periodDamage = 0;
    private float timeCast = 0;

    public void SetValues(int startDamage, int periodDamage, float timeCast)
    {
        this.timeCast = timeCast;
        this.startDamage = startDamage;
        this.periodDamage = periodDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemysHealth enemysHealth = other.GetComponent<EnemysHealth>();

            if( enemysHealth != null)
            {
                enemysHealth.PoisonDamage(startDamage, periodDamage, timeCast);
            }

        }
    }
}
