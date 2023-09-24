using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnMovBirb : EnemysMovement 
{
    private bool inLeftWorld = true;

    private float moveSpeed = 7.0f;
    private bool isSee = false;
    private bool readyToFight = true;
    private float punchRadius = 4.0f;
    private float timeStunned = 0f;
    private float numOfStunns = 0f;
    private bool isStunned = false;
    private float reloadTeleport;
    private float minReload = 4f;
    private float maxReload = 12f;

    private Vector3 chPositionMinus = Vector3.zero;
    private Vector3 chPositionPlus = Vector3.zero;
    Vector3 shortestPoint = Vector3.zero;


    private float currentPercentSpeed = 100f;
    private float minimalPercentSpeed = 10f;
    private NavMeshAgent agent;
    private Transform characterGirl;
    private Animator animator;
    private EnHealthBirb healthBirb;
    private GameObject teleportEffect1;
    private GameObject teleportEffect2;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        animator = GetComponent<Animator>();
        //animator.SetBool("isRunnning", false);
        healthBirb = GetComponent<EnHealthBirb>();
        reloadTeleport = Random.Range(2f, 7f);
        teleportEffect1 = Resources.Load<GameObject>("OtherObjects/Teleports/MagicCircleForTeleportEnemyBlue");
        teleportEffect2 = Instantiate(teleportEffect1, new Vector3(0f, -20f, 0f), Quaternion.identity);
        teleportEffect1 = Instantiate(teleportEffect1, new Vector3(0f, -20f, 0f), Quaternion.identity);
        teleportEffect1.SetActive(false);
        teleportEffect2.SetActive(false);
    }

    public override void IsStunned(float time)
    {
        numOfStunns++;
        StartCoroutine(Stunned(time));
    }

    private IEnumerator Stunned(float time)
    {
        isStunned = true;
        yield return new WaitForSeconds(time);
        if(--numOfStunns == 0) isStunned = false;
    }

    public override void Slow(float slow)
    {
        currentPercentSpeed -= slow;
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
        if(!isSee && other.CompareTag("Player"))
        {
            isSee = true;
            characterGirl = other.transform;
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        while (transform.position != null)
        {
            if (healthBirb.IsDeath())
            {
                agent.Stop();
                animator.speed /= 1.5f;
                animator.SetBool("isDeath", true);
                teleportEffect1.SetActive(false);
                teleportEffect2.SetActive(false);
                break;
            }
            else
            {
                if (isSee && readyToFight && !isStunned)
                {
                    chPositionMinus = characterGirl.transform.position - new Vector3(1000, 0, 0);
                    chPositionPlus = characterGirl.transform.position + new Vector3(1000, 0, 0);

                    float ch = Vector3.Distance(characterGirl.transform.position, transform.position);
                    float mi = Vector3.Distance(chPositionMinus, transform.position);
                    float pl = Vector3.Distance(chPositionPlus, transform.position);
                    if (ch < mi)
                    {
                        if (ch < pl)
                            shortestPoint = characterGirl.transform.position;
                        else
                            shortestPoint = chPositionPlus;
                    }
                    else
                    {
                        if(mi < pl)
                            shortestPoint = chPositionMinus;
                        else
                            shortestPoint = chPositionPlus;
                    }


                    if (Vector3.Distance(characterGirl.transform.position, transform.position) < punchRadius)
                    {
                        agent.velocity = Vector3.zero;
                        StartCoroutine(DealDamage());
                    }
                    else if(Vector3.Distance(shortestPoint, transform.position) < punchRadius)
                    {
                        StartCoroutine(Teleport());
                    }
                    else if(reloadTeleport < 0f && Vector3.Distance(characterGirl.transform.position, transform.position) > 2 * punchRadius)
                    {
                        StartCoroutine(Teleport());
                    }
                    else
                    {
                        animator.SetBool("isRunning", true);
                        agent.SetDestination(shortestPoint);
                    }
                }
                else
                {
                    animator.SetBool("isRunning", false);
                    agent.ResetPath();
                }
            }
            
            reloadTeleport -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Teleport()
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

    private IEnumerator DealDamage()
    {
        readyToFight = false;
        animator.SetBool("isPunch", true);
        //animator.SetBool("isRunning", false);
        agent.SetDestination(transform.position);
        agent.Stop();
        agent.ResetPath();
        yield return new WaitForSeconds(0.367f);
        if (Vector3.Distance(characterGirl.transform.position, transform.position) < punchRadius * 1.5f)
            characterGirl.GetComponent<Health>().DealDamage(10);
        yield return new WaitForSeconds(0.65f);
        animator.SetBool("isPunch", false);
        yield return new WaitForSeconds(0.3f);
        readyToFight = true;
        agent.Resume();
    }
}
