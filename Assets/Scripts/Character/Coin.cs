using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int numCoins = 0;

    public bool Pay(int price)
    {
        if(price <= numCoins)
        {
            numCoins -= price;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PkusCoin()
    {
        numCoins++;
    }

    public void SetNumCoins(int value)
    {
        numCoins = value;
    }
}
