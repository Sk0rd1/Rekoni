using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpendableResources : MonoBehaviour
{
    public int NumCoins { get; private set; } = 0;

    public int SphereM { get; private set; } = 0;
    public int SphereP { get; private set; } = 0;
    public int SphereS { get; private set; } = 0;
    public int SphereU { get; private set; } = 0;
    public int SphereI { get; private set; } = 0;

    GameObject textCoin;
    TextMeshProUGUI tCoin;

    private void Start()
    {
        SaveManager sm = new SaveManager();
        sm.LoadGame();
        textCoin = GameObject.Find("Main Camera/Canvas/CoinCount");
        tCoin = textCoin.GetComponent<TextMeshProUGUI>();
        tCoin.text = NumCoins.ToString();
        StartCoroutine(CoinVisible());
    }

    public void SetValues(int numCoins, int sphereM, int sphereP, int sphereS, int sphereU, int sphereI)
    {
        this.NumCoins = numCoins;
        this.SphereM = sphereM;
        this.SphereP = sphereP;
        this.SphereS = sphereS;
        this.SphereU = sphereU;
        this.SphereI = sphereI;
    }

    public bool CreateSpell(int m, int p, int s, int u, int i)
    {
        if (SphereM < m || SphereP < p || SphereS < s || SphereU < u || SphereI < i)
        {
            return false;
        }
        else
        {
            SphereM -= m;
            SphereP -= p;
            SphereS -= s;
            SphereU -= u;
            SphereI -= i;
            return true;
        }
    }

    public bool Pay(int price)
    {
        if (price <= NumCoins)
        {
            NumCoins -= price;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlusCoin(int value = 1)
    {
        NumCoins += value;
        tCoin.text = NumCoins.ToString();
        StopAllCoroutines();
        StartCoroutine(CoinVisible());
    }

    private IEnumerator CoinVisible()
    {
        textCoin.SetActive(true);
        //yield return new WaitForSeconds(4);
        yield return new WaitForSeconds(4);
        textCoin.SetActive(false);
    }
}
