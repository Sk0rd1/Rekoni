using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class EnemysHealth : MonoBehaviour
{
    protected int health;
    protected bool enemyIsDeath = false;

    protected const float POISONINTERVAL = 1f;
    protected int increasePoisonDamage = 0;
    protected int totalPoisonDamage = 0;
    protected float totalPoisonDuration = 0f;
    protected bool isPoisonTimerStart = false;

    protected const float FIREINTERVAL = 1f;
    protected const float TIMETOFIREDAMAGE = 5f;
    protected int finalFireDamage = 0;
    protected int increaseFireDamage = 0;
    protected int numOfFireEffects = 0;
    protected bool isFireTimerStart = false;

    protected Renderer renderer;
    protected Material[] material;
    protected Animator animator;
    protected EnMovBirb enMovBirb;

    protected int maxHealth = 1;

    public virtual int MaxHealth()
    {
        return maxHealth; 
    }

    protected bool isDeath = false;

    public virtual bool IsDeath()
    {
        return isDeath;
    }

    protected bool isBoss = true;

    public virtual bool IsBoss()
    {
        return isBoss;
    }

    protected virtual void MinusHealth(int damage)
    {
        health -= damage;

        if (health < 1 && !enemyIsDeath)
        {
            enemyIsDeath = true;
            StartCoroutine(Death());
        }
    }

    protected virtual IEnumerator Death()
    {
        float currentValue = 1f;
        isDeath = true;
        GetComponent<Animator>().SetBool("isDeath", true);
        while (currentValue > -9f)
        {
            currentValue -= 14 * Time.deltaTime;
            foreach (Material mat in material)
            {
                mat.SetFloat("_Cuttof_Heigth", currentValue);
            }
            yield return new WaitForEndOfFrame();
        }

        yield return null;
        Debug.Log("Destroy");
        Destroy(gameObject);
    }

    public virtual void Damage(int damage)
    {
        MinusHealth(damage);
    }

    public virtual void PoisonDamage(int totalDamage, int increasesDamage, float fullTime)
    {
        totalPoisonDamage += totalDamage;

        StartCoroutine(DamageFromPoison(increasesDamage, fullTime));

        if (!isPoisonTimerStart)
        {
            StartCoroutine(CountPoisonDamage(increasesDamage, fullTime));
        }
    }

    public virtual void FireDamage(int finalDamage, int increasingDamage)
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

    protected virtual IEnumerator CountFireDamage()
    {
        int numOfTiks = (int)(TIMETOFIREDAMAGE / FIREINTERVAL);

        while (numOfTiks > 0)
        {
            MinusHealth(increaseFireDamage);
            yield return new WaitForSeconds(FIREINTERVAL);
        }
    }

    protected virtual IEnumerator FinalFireDamage()
    {
        isFireTimerStart = true;

        yield return new WaitForSeconds(5f);

        MinusHealth(finalFireDamage * numOfFireEffects);

        finalFireDamage = 0;
        numOfFireEffects = 0;
        increaseFireDamage = 0;

        isFireTimerStart = false;
    }

    protected virtual IEnumerator CountPoisonDamage(int damage, float fullTime)
    {
        totalPoisonDuration = (int)(fullTime / POISONINTERVAL);

        while (totalPoisonDuration >= 0)
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

    protected virtual IEnumerator DamageFromPoison(int increasesDamage, float fullTime)
    {
        increasePoisonDamage += increasesDamage;

        if (fullTime > totalPoisonDuration)
        {
            totalPoisonDuration = fullTime;
        }

        yield return new WaitForSeconds(fullTime);

        increasePoisonDamage -= increasesDamage;
    }
}
