using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysMovement : MonoBehaviour
{
    private float currentPercentSpeed = 100f;
    private float minimalPercentSpeed = 10f;

    public void Slow(float slow)
    {
        currentPercentSpeed -= slow;
        if(currentPercentSpeed < minimalPercentSpeed)
        {
            currentPercentSpeed = minimalPercentSpeed;
        }
        //Debug.Log("Speed " + currentPercentSpeed);
    }
}
