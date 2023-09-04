using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private int health = 50;
    private TextMeshProUGUI textTMP;

    private void Start()
    {
        GameObject obj = GameObject.Find("Main Camera/Canvas/Health");
        textTMP = obj.GetComponent<TextMeshProUGUI>();
        textTMP.text = health.ToString();
    }

    public int CurrentHealth()
    {
        return health;
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        if(health < 1)
        {
            health = 0;
            Debug.Log("Dead");
        }
        textTMP.text = health.ToString();
    }

    public void Heal(int heal)
    {
        health += heal;
        if(health > 100)
            health = 100;
        textTMP.text = health.ToString();
    }
}
