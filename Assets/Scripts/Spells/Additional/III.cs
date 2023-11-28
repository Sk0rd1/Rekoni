using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class III : MonoBehaviour
{
    private int damage = 0;
    private float timeStunned = 0f;

    public void SetValues(int damage, float timeStunned)
    {
        this.timeStunned = timeStunned;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            try
            {
                other.GetComponent<EnemysHealth>().Damage(damage, TypeDamage.Cold);
                other.GetComponent<EnemysMovement>().IsStunned(timeStunned);
                this.gameObject.SetActive(false);
            }
            catch { }

        }
    }
}
