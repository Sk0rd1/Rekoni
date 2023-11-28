using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnMovBirb : EnemysMovement 
{
    private Vector3 chPositionMinus = Vector3.zero;
    private Vector3 chPositionPlus = Vector3.zero;
    Vector3 shortestPoint = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        if (!isSee && other.CompareTag("Player"))
        {
            isSee = true;
            characterGirl = other.transform;
            StartCoroutine(Move());
        }
    }

    private void Start()
    {
        moveSpeed = 8f;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        animator = GetComponent<Animator>();
        //animator.SetBool("isRunnning", false);
        enHealth = GetComponent<EnHealthBirb>();
        reloadTeleport = Random.Range(2f, 7f);
        teleportEffect1 = Resources.Load<GameObject>("_OtherObjects/Teleports/MagicCircleForTeleportEnemyBlue");
        teleportEffect2 = Instantiate(teleportEffect1, new Vector3(0f, -20f, 0f), Quaternion.identity);
        teleportEffect1 = Instantiate(teleportEffect1, new Vector3(0f, -20f, 0f), Quaternion.identity);
        teleportEffect1.SetActive(false);
        teleportEffect2.SetActive(false);
    }

    private IEnumerator Move()
    {
        while (transform.position != null)
        {
            if (enHealth.IsDeath())
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
                        if (mi < pl)
                            shortestPoint = chPositionMinus;
                        else
                            shortestPoint = chPositionPlus;
                    }


                    if (Vector3.Distance(characterGirl.transform.position, transform.position) < punchRadius)
                    {
                        agent.velocity = Vector3.zero;
                        StartCoroutine(DealDamage());
                    }
                    /*else if (Vector3.Distance(shortestPoint, transform.position) < punchRadius)
                    {
                        StartCoroutine(Teleport());
                    }
                    else if (reloadTeleport < 0f && Vector3.Distance(characterGirl.transform.position, transform.position) > 2 * punchRadius)
                    {
                        StartCoroutine(Teleport());
                    }*/
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

    /*private IEnumerator Move()
    {
        while (transform.position != null)
        {
            animator.speed = scaleGame;
            agent.speed = scaleGame * moveSpeed;

            if(scaleGame == 0f)
            {
                agent.Stop();
                agent.ResetPath();
            }
            else
            {
                agent.Resume();
            }

            if (enHealth.IsDeath())
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
            
            reloadTeleport -= Time.deltaTime * scaleGame;
            yield return new WaitForEndOfFrame();
        }
    }*/
}
