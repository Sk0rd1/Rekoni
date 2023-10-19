using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressingPlate : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    private bool isDoorOpen = false;
    private int countObjects = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("BoxBanTeleport"))
        {
            countObjects++;
            if(countObjects == 1 && !isDoorOpen)
            {
                isDoorOpen = true;
                door.GetComponent<Door>().Open(isDoorOpen);
                transform.GetChild(1).transform.position -= new Vector3(0, 0.35f, 0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("BoxBanTeleport"))
        {
            countObjects--;
            if(countObjects == 0 && isDoorOpen)
            {
                isDoorOpen = false;
                door.GetComponent<Door>().Open(isDoorOpen);
                transform.GetChild(1).transform.position += new Vector3(0, 0.35f, 0);
            }
        }
    }
}
