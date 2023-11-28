using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemysMovement : MonoBehaviour
{
    protected float moveSpeed = 7.0f;
    protected float minimalPercentSpeed = 10f;
    protected float minReload = 4f;
    protected float maxReload = 12f;
    protected float punchRadius = 4.0f;
    protected bool inLeftWorld = true;

    protected float numOfStunns = 0f;
    protected bool isStunned = false;
    protected float timeStunned = 0f;
    protected float reloadTeleport;
    protected float currentPercentSpeed = 100f;
    protected bool isSee = false;
    protected NavMeshAgent agent;
    protected Transform characterGirl;
    protected Animator animator;
    protected EnemysHealth enHealth;
    protected GameObject teleportEffect1;
    protected GameObject teleportEffect2;
    protected bool readyToFight = true;

    protected virtual IEnumerator Move()
    {
        yield return null;
    }

    public virtual void IsStunned(float time)
    {
        numOfStunns++;
        StartCoroutine(Stunned(time));
    }

    protected virtual IEnumerator Stunned(float time)
    {
        isStunned = true;
        yield return new WaitForSeconds(time);
        if (--numOfStunns == 0) isStunned = false;
    }

    public virtual void Slow(float slow)
    {

        if(slow < 0)
        {
            currentPercentSpeed = currentPercentSpeed * 100f / (100f + slow);
        }
        else
        {
            currentPercentSpeed = currentPercentSpeed * (100f - slow) / 100f;
        }

        if (currentPercentSpeed < minimalPercentSpeed)
        {
            currentPercentSpeed = minimalPercentSpeed;
        }

        if (currentPercentSpeed > 100f)
        {
            currentPercentSpeed = 100f;
        }

        agent.speed = currentPercentSpeed * moveSpeed / 100f;
    }

    protected virtual IEnumerator Teleport()
    {
        readyToFight = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        reloadTeleport = 1000f;
        Vector3 pointToCheck = Vector3.zero;
        animator.SetBool("isTeleport", true);
        animator.SetBool("isRunning", true);

        if (inLeftWorld)
        {
            pointToCheck = transform.position + new Vector3(1000f, 0f, 0f);
        }
        else
        {
            pointToCheck = transform.position - new Vector3(1000f, 0f, 0f);
        }
        teleportEffect1.transform.position = transform.position;
        teleportEffect2.transform.position = pointToCheck;

        teleportEffect1.SetActive(true);
        teleportEffect2.SetActive(true);

        pointToCheck.y += 1;

        yield return new WaitForSeconds(0.867f);

        Collider[] colliders = Physics.OverlapSphere(pointToCheck, 1f);
        pointToCheck.y -= 1;
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "BanTeleport" || collider.tag == "BoxBanTeleport")
            {
                reloadTeleport = 2f;
                animator.SetBool("isTeleport", false);
                teleportEffect1.SetActive(false);
                yield break;
            }
        }

        animator.SetBool("isTeleport", false);
        transform.position = pointToCheck;

        if (inLeftWorld)
            inLeftWorld = false;
        else
            inLeftWorld = true;

        yield return new WaitForSeconds(0.1f);
        teleportEffect1.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        teleportEffect2.SetActive(false);
        readyToFight = true;
        animator.speed = 1.0f;
        reloadTeleport = Random.Range(minReload, maxReload);
    }

    protected virtual IEnumerator DealDamage()
    {
        readyToFight = false;
        animator.SetBool("isPunch", true);
        //animator.SetBool("isRunning", false);
        agent.SetDestination(transform.position);
        agent.isStopped = true;
        agent.ResetPath();
        yield return new WaitForSeconds(0.367f);
        if (Vector3.Distance(characterGirl.transform.position, transform.position) < punchRadius * 1.5f)
            characterGirl.GetComponent<Health>().DealDamage(10);
        yield return new WaitForSeconds(0.65f);
        animator.SetBool("isPunch", false);
        yield return new WaitForSeconds(0.3f);
        readyToFight = true;
        agent.isStopped = false;
    }
}
