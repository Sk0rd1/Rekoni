using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IIS : MonoBehaviour
{
    private float stunDuration = 0f;
    public bool ISFREEZE { private get; set; } = false;

    public void SetValues(float stunDuration)
    {
        this.stunDuration = stunDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && ISFREEZE)
        {
            other.GetComponent<EnemysMovement>().IsStunned(stunDuration);
        }
    }
}
