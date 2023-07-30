using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class EnemysHealth : MonoBehaviour
{
    private int health = 1000;

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


    void Update()
    {
        Debug.Log("HP" + health);

        if(health < 1)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
    }

    public void PoisonDamage(int totalDamage, int increasesDamage, float fullTime)
    {
        totalPoisonDamage += totalDamage;

        StartCoroutine(DamageFromPoison(increasesDamage, fullTime));

        if (!isPoisonTimerStart)
        {
            StartCoroutine(CountPoisonDamage(increasesDamage, fullTime));
        }
    }

    public void FireDamage(int finalDamage, int increasingDamage)
    {
        increaseFireDamage += increasingDamage;
        StartCoroutine(CountFireDamage());
        finalFireDamage += finalDamage;
        numOfFireEffects++;
        if (!isFireTimerStart)
        {
            StartCoroutine(FinalFireDamage());
        }
    }

    IEnumerator CountFireDamage()
    {
        int numOfTiks = (int)(TIMETOFIREDAMAGE / FIREINTERVAL);

        while(numOfTiks > 0)
        {
            health -= increaseFireDamage;
            yield return new WaitForSeconds(FIREINTERVAL);
        }
    }
 
    IEnumerator FinalFireDamage()
    {
        isFireTimerStart = true;

        yield return new WaitForSeconds(5f);

        health -= finalFireDamage * numOfFireEffects;

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
            health -= totalPoisonDamage;

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
    }
}
