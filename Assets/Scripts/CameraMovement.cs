using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 offsetPosition = new Vector3(0f, 22.5f, -18f);
    [SerializeField]
    private Quaternion rotation = new Quaternion(48f, 0f, 0f, 0f);
    [SerializeField]
    private GameObject character;
    [SerializeField]
    //SAFASFSFAASFSAFSFASFFSAFSA
    public Vector3 GetOffsetPosition() { return offsetPosition; }

    void Start()
    {

    }
    void Update()
    {

        Vector3 newPosition = new Vector3(character.transform.position.x + offsetPosition.x, character.transform.position.y + offsetPosition.y, character.transform.position.z + offsetPosition.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, 5f * Time.deltaTime);

    }
}