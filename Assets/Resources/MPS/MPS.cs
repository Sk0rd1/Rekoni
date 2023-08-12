using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MPS : MonoBehaviour
{
    private float slow = 0;
    private float periodOfHeal = 1000;

    private int enemyCount = 0;
    private List<Transform> enemys = new List<Transform>();

    public void SetValues(float slow, float periodOfHeal)
    {
        this.slow = slow;
        this.periodOfHeal = periodOfHeal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemys.Add(other.transform);
            EnemysMovement enMov = other.GetComponent<EnemysMovement>();
            enMov.Slow(slow);
            enemyCount++;
            if (enemyCount == 1)
            {
                StartCoroutine(Heal());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemysMovement enMov = other.GetComponent<EnemysMovement>();
            enMov.Slow(slow);
            enemyCount--;
        }
    }

    IEnumerator Heal()
    {
        Health characterHealth = GameObject.Find("CharacterGirl").GetComponent<Health>();
        GameObject character = GameObject.Find("CharacterGirl");

        while (enemyCount != 0)
        {
            int currentEnemyCount = 0;
            foreach(Transform enemy in enemys)
            {   
                if(enemy != null)
                {
                    Vector3 distance3 = enemy.position - character.transform.position;
                    float distance = Mathf.Sqrt(Mathf.Pow(distance3.x, 2f) + Mathf.Pow(distance3.z, 2f));

                    if(distance > 6.75)
                    {
                        currentEnemyCount++;
                    }
                }
            }
            characterHealth.Heal(currentEnemyCount);
            yield return new WaitForSeconds(periodOfHeal);
        }
    }
}
