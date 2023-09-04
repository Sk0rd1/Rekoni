using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class EnemysHealth : MonoBehaviour
{
    /*private int health = 10;
    private bool enemyIsDeath = false;

    private const float POISONINTERVAL = 1f;
    private int increasePoisonDamage = 0;
    private int totalPoisonDamage = 0;
    private float totalPoisonDuration = 0f;
    private bool isPoisonTimerStart = false;

    private const float FIREINTERVAL = 1f;
    private const float TIMETOFIREDAMAGE = 5f;
    private int finalFireDamage = 0;
    private int increaseFireDamage = 0;
    private int numOfFireEffects = 0;
    private bool isFireTimerStart = false;

    private Renderer renderer;
    private Material material;*/

    private void Start()
    {
        /*renderer = GetComponent<Renderer>();
        material = renderer.material;*/
    }

    private void MinusHealth(int damage)
    {
        /*health -= damage;
        //Debug.Log("HP" + health);

        if(health < 1 && !enemyIsDeath)
        {
            enemyIsDeath = true;
            StartCoroutine(Death());
        }*/
    }

    /*IEnumerator Death()
    {
        float currentValue = 1f;

        while(currentValue > -9f)
        {
            currentValue -= 200 * Time.deltaTime;
            material.SetFloat("_Cuttof_Heigth", currentValue);
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
        Destroy(gameObject);
    }*/

    public virtual void Damage(int damage)
    {
        /*MinusHealth(damage);*/
    }

    public virtual void PoisonDamage(int totalDamage, int increasesDamage, float fullTime)
    {
        /*totalPoisonDamage += totalDamage;

        StartCoroutine(DamageFromPoison(increasesDamage, fullTime));

        if (!isPoisonTimerStart)
        {
            StartCoroutine(CountPoisonDamage(increasesDamage, fullTime));
        }*/
    }

    public virtual void FireDamage(int finalDamage, int increasingDamage)
    {
        /*increaseFireDamage += increasingDamage;
        StartCoroutine(CountFireDamage());
        finalFireDamage += finalDamage;
        numOfFireEffects++;
        if (!isFireTimerStart)
        {
            StartCoroutine(FinalFireDamage());
        }*/
    }

    /*IEnumerator CountFireDamage()
    {
        int numOfTiks = (int)(TIMETOFIREDAMAGE / FIREINTERVAL);

        while(numOfTiks > 0)
        {
            MinusHealth(increaseFireDamage);
            yield return new WaitForSeconds(FIREINTERVAL);
        }
    }
 
    IEnumerator FinalFireDamage()
    {
        isFireTimerStart = true;

        yield return new WaitForSeconds(5f);

        MinusHealth(finalFireDamage * numOfFireEffects);

        finalFireDamage = 0;
        numOfFireEffects = 0;
        increaseFireDamage = 0;

        isFireTimerStart = false;
    }

    IEnumerator CountPoisonDamage(int damage, float fullTime)
    {
        totalPoisonDuration = (int)(fullTime / POISONINTERVAL);

        while(totalPoisonDuration >= 0)
        {
            yield return new WaitForSeconds(POISONINTERVAL);

            totalPoisonDamage += increasePoisonDamage;
            MinusHealth(totalPoisonDamage);

            totalPoisonDuration -= POISONINTERVAL;
        }

        totalPoisonDamage = 0;
        increasePoisonDamage = 0;
        totalPoisonDuration = 0f;
    }

    IEnumerator DamageFromPoison(int increasesDamage, float fullTime)
    {
        increasePoisonDamage += increasesDamage;

        if(fullTime > totalPoisonDuration)
        {
            totalPoisonDuration = fullTime;
        }

        yield return new WaitForSeconds(fullTime);

        increasePoisonDamage -= increasesDamage;
    }*/
}
