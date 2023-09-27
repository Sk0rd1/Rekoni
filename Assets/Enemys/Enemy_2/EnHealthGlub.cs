using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class EnHealthGlub : EnemysHealth
{
    private int health = 50;
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

    //public override bool IsDeath { get; protected set; } = false;
    //public override int MaxHealth { get; protected set; } = 50;
    //public override bool IsBoss { get; protected set; } = false;

    private Renderer renderer;
    private Material[] material;
    private Animator animator;
    private EnMovBirb enMovBirb;

    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        material = renderer.materials;
        animator = GetComponent<Animator>();
        enMovBirb = GetComponent<EnMovBirb>();
    }

    protected override void MinusHealth(int damage)
    {
        health -= damage;
        //Debug.Log("HP" + health);

        if(health < 1 && !enemyIsDeath)
        {
            enemyIsDeath = true;
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        float currentValue = 1f;
        isDeath = true;
        GetComponent<Animator>().SetBool("isDeath", true);
        while(currentValue > -9f)
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

    public override void Damage(int damage)
    {
        MinusHealth(damage);
    }

    public override void PoisonDamage(int totalDamage, int increasesDamage, float fullTime)
    {
        totalPoisonDamage += totalDamage;

        StartCoroutine(DamageFromPoison(increasesDamage, fullTime));

        if (!isPoisonTimerStart)
        {
            StartCoroutine(CountPoisonDamage(increasesDamage, fullTime));
        }
    }

    public override void FireDamage(int finalDamage, int increasingDamage)
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
    }
}
