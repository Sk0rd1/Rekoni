using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemysMovement : MonoBehaviour
{
    /*private float currentPercentSpeed = 100f;
    private float minimalPercentSpeed = 10f;
    private NavMeshAgent agent;
    private GameObject characterGirl;*/

    private void Start()
    {
        /*agent = GetComponent<NavMeshAgent>();
        characterGirl = GameObject.Find("CharacterGirl");*/
    }

    public virtual void Slow(float slow)
    {
        /*currentPercentSpeed -= slow;
        if(currentPercentSpeed < minimalPercentSpeed)
        {
            currentPercentSpeed = minimalPercentSpeed;
        }
        //Debug.Log("Speed " + currentPercentSpeed);*/
    }
    
    private void Update()
    {
        //agent.SetDestination(characterGirl.transform.position);
    }
}
