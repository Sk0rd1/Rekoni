using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen;

    public void Open()
    {
        isOpen = true;
    }

    void Update()
    {
        if(isOpen)
        {
            Transform cube = transform.GetChild(0);
            if (cube.transform.position.y > -7)
                cube.transform.position += new Vector3(0f, -5f * Time.deltaTime, 0f);
        }
    }
}
