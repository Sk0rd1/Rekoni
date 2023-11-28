using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PSM : MonoBehaviour
{
    private float slow = 0;
    private float periodOfHeal = 1000;

    private List<Transform> enemys = new List<Transform>();
    private List<Transform> slowedEnemys = new List<Transform>();

    public void SetValues(float slow, float periodOfHeal)
    {
        this.slow = slow;
        this.periodOfHeal = periodOfHeal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            bool isAdded = false;
            foreach(Transform en in enemys)
            {
                if (en.transform == other.transform) isAdded = true;
            }
            if (!isAdded)
                enemys.Add(other.transform);
            if (enemys.Count == 1)
            {
                StartCoroutine(Heal());
            }
        }
    }

    IEnumerator Heal()
    {
        Health characterHealth = GameObject.Find("CharacterGirl").GetComponent<Health>();
        GameObject character = GameObject.Find("CharacterGirl");

        slowedEnemys.Add(null);

        while (enemys.Count != 0)
        {
            int currentEnemyCount = 0;
            foreach(Transform enemy in enemys)
            {   
                if(enemy != null)
                {
                    Vector3 distance3 = enemy.position - character.transform.position;
                    float distance = Mathf.Sqrt(Mathf.Pow(distance3.x, 2) + Mathf.Pow(distance3.z, 2));

                    if (distance < 12f && distance > 6f)
                    {
                        if (!slowedEnemys.Contains(enemy))
                        {
                            EnemysMovement enMov = enemy.GetComponent<EnemysMovement>();
                            enMov.Slow(slow);
                            slowedEnemys.Add(enemy);
                        }
                        currentEnemyCount++;
                    }
                    else
                    {
                        if (slowedEnemys.Contains(enemy))   
                        {
                            EnemysMovement enMov = enemy.GetComponent<EnemysMovement>();
                            enMov.Slow(-slow);
                            slowedEnemys.Remove(enemy);
                        }
                    }
                }
            }
            characterHealth.Heal(currentEnemyCount);
            yield return new WaitForSeconds(periodOfHeal);
        }
    }
}
