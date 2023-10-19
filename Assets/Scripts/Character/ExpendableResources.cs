using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpendableResources : MonoBehaviour
{
    private int numCoins = 0;

    private int sphereM = 0;
    private int sphereP = 0;
    private int sphereS = 0;
    private int sphereU = 0;
    private int sphereI = 0;

    GameObject textCoin;
    TextMeshProUGUI tCoin;

    private void Start()
    {
        // тут повинно бути зчитування значень Coin і Sphere з файлу з сейвом.
        numCoins = 13;
        textCoin = GameObject.Find("Main Camera/Canvas/CoinCount");
        tCoin = textCoin.GetComponent<TextMeshProUGUI>();
        tCoin.text = numCoins.ToString();
        StartCoroutine(CoinVisible());
    }

    public bool CreateSpell(int m, int p, int s, int u, int i)
    {
        if (sphereM < m || sphereP < p || sphereS < s || sphereU < u || sphereI < i)
        {
            return false;
        }
        else
        {
            sphereM -= m;
            sphereP -= p;
            sphereS -= s;
            sphereU -= u;
            sphereI -= i;
            return true;
        }
    }

    public bool Pay(int price)
    {
        if (price <= numCoins)
        {
            numCoins -= price;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlusCoin(int value = 1)
    {
        numCoins += value;
        tCoin.text = numCoins.ToString();
        StopAllCoroutines();
        StartCoroutine(CoinVisible());
    }

    private IEnumerator CoinVisible()
    {
        textCoin.SetActive(true);
        yield return new WaitForSeconds(4);
        textCoin.SetActive(false);
    }
}
