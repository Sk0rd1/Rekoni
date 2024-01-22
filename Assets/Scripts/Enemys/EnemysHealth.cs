using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemysHealth : MonoBehaviour
{
    protected int health;
    protected bool enemyIsDeath = false;

    protected const float POISONINTERVAL = 1f;
    protected int totalPoisonDamage = 0;
    protected int countOfPoisons = 0;

    protected const float FIREINTERVAL = 1f;
    protected const float PERCENTVALUE = 20f;
    protected float maxFireDamage = 0;
    protected float totalFireDamage = 0;
    protected int countOfFires = 0;
    protected bool isFireTimerStart = false;

    protected Renderer renderer;
    protected Material[] material;
    protected Animator animator;
    protected EnemysMovement enMov;
    protected string coinPath = "_OtherObjects/Coin";
    protected string textPath = "_OtherObjects/FloatingText";

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

    protected virtual void MinusHealth(int damage, TypeDamage typeDamage)
    {
        health -= damage;

        var go = Instantiate(Resources.Load<GameObject>(textPath), transform.position, Quaternion.identity, transform);
        TextMesh text = go.GetComponent<TextMesh>();
        text.text = damage.ToString();
        text.color = DamageInfo.GetColor(typeDamage);


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

        GameObject coin = Resources.Load<GameObject>(coinPath);
        for (int i = 0; i < 10;  i++) 
        {
            GameObject currentCoin = Instantiate(coin);
            currentCoin.transform.position = transform.position;
            Rigidbody rb = currentCoin.GetComponent<Rigidbody>();
            Vector3 coinDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            coinDirection.Normalize();
            coinDirection /= Random.Range(1f, 10f);
            coinDirection *= 5f;
            coinDirection.y = 5f;
            rb.velocity = coinDirection;
        }

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

    public virtual void Damage(int damage, TypeDamage typeDamage)
    {
        MinusHealth(damage, typeDamage);
    }

    public virtual void PoisonDamage(int damage, float fullTime)
    {
        countOfPoisons++;
        StartCoroutine(DamageFromPoison(damage, fullTime));
    }

    public virtual void FireDamage(int damage)
    {
        maxFireDamage += damage;
        totalFireDamage += damage;
        countOfFires++;

        if (countOfFires > 11) countOfFires = 11;

        if (!isFireTimerStart)
        {
            isFireTimerStart = true;
            StartCoroutine(DamageFromFire());
        }
    }
    protected virtual IEnumerator DamageFromFire()
    {
        while (totalFireDamage > 0)
        {
            yield return new WaitForSeconds(FIREINTERVAL);

            int currentDamage = (int)(maxFireDamage * PERCENTVALUE / 100f);

            if (totalFireDamage > currentDamage)
            {
                MinusHealth((int)(totalFireDamage - currentDamage), TypeDamage.Fire);
                //Debug.Log("t  " + (totalFireDamage - currentDamage));
            }
            else
            {
                MinusHealth((int)totalFireDamage, TypeDamage.Fire);
                //Debug.Log("f  " + totalFireDamage);
            }

            float value = maxFireDamage * ((PERCENTVALUE - countOfFires + 1) / 100f);
            if (value < 1)
            {
                totalFireDamage -= 1;
            }
            else
            {
                totalFireDamage -= value;
            }

            totalFireDamage -= (int)(maxFireDamage * ((PERCENTVALUE - countOfFires + 1) / 100f));
            Debug.Log(maxFireDamage * ((PERCENTVALUE - countOfFires) / 100f));
        }

        maxFireDamage = 0;
        isFireTimerStart = false;
    }


    protected virtual IEnumerator DamageFromPoison(int damage, float fullTime)
    {
        while (fullTime >= 0f)
        {
            yield return new WaitForSeconds(POISONINTERVAL);

            MinusHealth(damage + totalPoisonDamage, TypeDamage.Poison);
            fullTime -= POISONINTERVAL;
        }

        countOfPoisons--;

        if (countOfPoisons == 0)
        {
            totalPoisonDamage = 0;
        }
        else
        {
            totalPoisonDamage += (int)Mathf.Sqrt(damage);
        }
    }
}
