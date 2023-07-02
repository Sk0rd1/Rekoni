using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressingPlate : MonoBehaviour
{
    public UnityEvent OnPressed;
    private bool isPressed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            Transform cube = transform.GetChild(0);
            cube.transform.position += new Vector3(0f, -0.49f, 0f);
            isPressed = true;
        }
        OnPressed.Invoke();
    }
}
