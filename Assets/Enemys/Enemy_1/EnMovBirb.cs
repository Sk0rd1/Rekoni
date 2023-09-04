using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnMovBirb : EnemysMovement 
{
    private float moveSpeed = 7.0f;
    private bool isSee = false;
    private bool readyToFight = true;
    private float punchRadius = 4.0f;

    private float currentPercentSpeed = 100f;
    private float minimalPercentSpeed = 10f;
    private NavMeshAgent agent;
    private Transform characterGirl;
    private Animator animator;
    private EnHealthBirb healthBirb;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        animator = GetComponent<Animator>();
        animator.SetBool("isRunnning", false);
        healthBirb = GetComponent<EnHealthBirb>();
    }

    public override void Slow(float slow)
    {
        currentPercentSpeed -= slow;
        Debug.Log("slow " + slow + " cPS " + currentPercentSpeed);
        if(currentPercentSpeed < minimalPercentSpeed)
        {
            currentPercentSpeed = minimalPercentSpeed;
        }

        if(currentPercentSpeed > 100f)
        {
            currentPercentSpeed = 100f;
        }

        agent.speed = currentPercentSpeed * moveSpeed / 100f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isSee = true;
            characterGirl = other.transform;
        }
    }

    private void Update()
    {
        if (healthBirb.IsDeath)
        {
            agent.Stop();
            animator.speed /= 1.5f;
            animator.SetBool("isDeath", true);
        }
        else
        {
            if (isSee && readyToFight)
            {
                Debug.Log("isSee, readyToFight");
                if (Vector3.Distance(characterGirl.transform.position, transform.position) < punchRadius)
                {
                    Debug.Log("DealDamage");
                    StartCoroutine(DealDamage());
                }
                else
                {
                    Debug.Log("Run");
                    animator.SetBool("isRunning", true);
                    agent.SetDestination(characterGirl.transform.position);
                }
            }
        }
    }

    private IEnumerator DealDamage()
    {
        readyToFight = false;
        animator.SetBool("isRunning", false);
        animator.SetBool("isPunch", true);
        agent.SetDestination(transform.position);
        agent.Stop();
        agent.ResetPath();
        yield return new WaitForSeconds(0.667f);
        if (Vector3.Distance(characterGirl.transform.position, transform.position) < punchRadius * 1.5f)
            characterGirl.GetComponent<Health>().DealDamage(10);
        yield return new WaitForSeconds(0.15f);
        animator.SetBool("isPunch", false);
        yield return new WaitForSeconds(0.5f);
        readyToFight = true;
        agent.Resume();
    }
}
