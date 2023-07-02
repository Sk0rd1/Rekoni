using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    private float health = 100f;
    //Walking/Patroling
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    [SerializeField] private float timeBetweenAttaks;
    private bool alredyAttacked;
    [SerializeField] private GameObject projectile;

    [SerializeField] private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("CharacterModel").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInAttackRange && !playerInSightRange) Patroling();
        if (!playerInAttackRange && playerInSightRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude <2f)
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 1f, whatIsGround))
            walkPointSet = true;

    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        if(!alredyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward*32f,ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);


            alredyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttaks);
        }
    }

    private void ResetAttack()
    {
        alredyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 1f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
