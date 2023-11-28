using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Vector3 Offset = new Vector3 (0f, 3f, 0f);

    void Start()
    {
        Destroy(gameObject, 0.45f);

        var cameraToLookAt = Camera.main;
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);

        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
    }
}
