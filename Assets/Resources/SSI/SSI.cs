using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSI : MonoBehaviour
{
    private float timeCast = 0;
    private float forceAttraction = 0;
    private int damage = 0;

    public bool IsCastBH { private get; set; }

    public void SetValues(float timeCast, int damage, float forceAttraction)
    {
        this.timeCast = timeCast;
        this.damage = damage;
        this.forceAttraction = forceAttraction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemysHealth enemyHealth = other.GetComponent<EnemysHealth>();
            StartCoroutine(PeriodDamage(enemyHealth));
            StartCoroutine(EnemyToCenter(other.transform));
        }
    }

    IEnumerator EnemyToCenter(Transform enemy)
    {
        while (IsCastBH)
        {
            try
            {
                Vector3 toCenterVector = transform.position - enemy.position;
                toCenterVector.y = 0;
                Vector3 tangentVector = new Vector3(toCenterVector.z, 0, -toCenterVector.x);
                Vector3 resultVector = 1.5f * tangentVector.normalized + forceAttraction * toCenterVector.normalized;

                resultVector.y = transform.position.y;

                enemy.position += new Vector3(10 * resultVector.x * Time.deltaTime, 0, 10 * resultVector.z * Time.deltaTime);
            }
            catch { }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PeriodDamage(EnemysHealth enemyHealth)
    {
        while (IsCastBH)
        {
            try
            {
                enemyHealth.Damage(damage);
            }
            catch { }

            yield return new WaitForSeconds(1f);
        }
    }
}
