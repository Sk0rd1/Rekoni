using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class Health : MonoBehaviour
{
    private class ShieldInfo
    {
        public int num { get; set; }
        public GameObject obj { get; set; }
    }

    private int health = 50;

    private TextMeshProUGUI textTMP;
    GameObject objHud;
    List<ShieldInfo> numOfShields = new List<ShieldInfo>();

    private void Start()
    {
        GameObject obj = GameObject.Find("Main Camera/Canvas/Health");
        objHud = GameObject.Find("Main Camera/Canvas/DealDamage");
        textTMP = obj.GetComponent<TextMeshProUGUI>();
        textTMP.text = health.ToString();
    }

    public int CurrentHealth()
    {
        return health;
    }

    public void DealDamage(int damage)
    {
        int currentNumOfShields = 0;
        for (int i = 0;  i < numOfShields.Count; i++) 
        {
            currentNumOfShields += numOfShields[i].num;
        }

        for (int i = 0; i < numOfShields.Count; i++)
        {
            if (numOfShields[i].num == 0)
            {
                try
                {
                    numOfShields[i].obj.SetActive(false);
                }
                catch { }
                numOfShields.RemoveAt(i);
            }
        }

        if (currentNumOfShields > 0)
        {
            for (int i = 0; i < numOfShields.Count; i++)
            {
                if (numOfShields[i].num != 0)
                {
                    ShieldInfo newShield = new ShieldInfo { num = (numOfShields[i].num - 1), obj = numOfShields[i].obj };
                    numOfShields[i] = newShield;
                    break;
                }
            }
            // тут повинен вмикатись ефект на щиті
        }
        else
        {
            health -= damage;
            StartCoroutine(DamageCast());
            if (health < 1)
            {
                health = 0;
                Debug.Log("Dead");
            }
            textTMP.text = health.ToString();
        }
    }

    private IEnumerator DamageCast()
    {
        objHud.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        objHud.SetActive(false);
    }

    public void Heal(int heal)
    {
        health += heal;
        if(health > 100)
            health = 100;
        textTMP.text = health.ToString();
    }

    public void AddShield(int numOfShields, float timeShield, GameObject shield)
    {
        StartCoroutine(Shield(numOfShields, timeShield, shield));
    }

    private IEnumerator Shield(int numOfShields, float timeShield, GameObject shield)
    {
        ShieldInfo newShield = new ShieldInfo { num = numOfShields, obj = shield };

        this.numOfShields.Add(newShield);

        //this.numOfShields[this.numOfShields.Count - 1].obj.SetActive(false);
        yield return new WaitForSeconds(timeShield);

        this.numOfShields.Remove(newShield);
    }
}
