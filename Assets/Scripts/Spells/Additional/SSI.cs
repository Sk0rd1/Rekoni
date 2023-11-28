using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSI : MonoBehaviour
{
    private float timeCast = 0;
    private float forceAttraction = 0;
    private int damage = 0;
    private float scaleGame = 1f;

    public bool IsCastBH { private get; set; }

    public void SetValues(float timeCast, int damage, float forceAttraction)
    {
        this.timeCast = timeCast;
        this.damage = damage;
        this.forceAttraction = forceAttraction;
    }

    protected virtual void ChangeScaleGame(float scaleGame)
    {
        this.scaleGame = scaleGame;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemysHealth enemyHealth = other.GetComponent<EnemysHealth>();
            EnemysMovement enemysMovement = other.GetComponent<EnemysMovement>();
            StartCoroutine(PeriodDamage(enemyHealth, enemysMovement));
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

                enemy.position += new Vector3(10 * resultVector.x * Time.deltaTime * scaleGame, 0, 10 * resultVector.z * Time.deltaTime * scaleGame);
                enemy.Rotate(0, 90f * Time.deltaTime * scaleGame, 0);
            }
            catch { }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PeriodDamage(EnemysHealth enemyHealth, EnemysMovement enemyMovement)
    {
        while (IsCastBH)
        {
            try
            {
                enemyHealth.Damage(damage, TypeDamage.Force);
                enemyMovement.IsStunned(0.55f);
            }
            catch { }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
