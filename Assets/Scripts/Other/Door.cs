using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private float startPosY; 
    private float endPosY;

    private void Start()
    {
        startPosY = transform.position.y;
        endPosY = startPosY - 10f;
    }

    public void Open(bool isOpen)
    {
        StopAllCoroutines();
        StartCoroutine(OpenDoor(isOpen));
    }

    private IEnumerator OpenDoor(bool isOpen)
    { 
        float currentDistance = 0;
        float doorSpeed = 10;
        Transform cube = transform.GetChild(0);
        if (isOpen)
        {
            while (cube.position.y > endPosY)
            {
                currentDistance += doorSpeed * Time.deltaTime;
                cube.position -= new Vector3(0f, doorSpeed * Time.deltaTime, 0f);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (cube.position.y < startPosY)
            {
                currentDistance += doorSpeed * Time.deltaTime;
                cube.position += new Vector3(0f, doorSpeed * Time.deltaTime, 0f);
                yield return new WaitForEndOfFrame();
            }
        }

    }
}
