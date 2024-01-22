using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragonBullet : MonoBehaviour
{
    private float bulletSpeed = 25f;
    private float bulletDistance = 15f;
    private int bulletDamage = 10;

    private GameObject characterGirl;

    private Vector3 moveDirection = Vector3.zero;
    private float distance = 0f;
    private bool isHit = false;

    private string dragonBoomStr = "_Enemys/Ammo/MagicBullets/DragonBoom";
    private GameObject dragonBoomPrefab;

    void Start()
    {
        characterGirl = GameObject.Find("CharacterGirl");

        this.transform.LookAt(characterGirl.transform.position);

        moveDirection = characterGirl.transform.position - transform.position;
        moveDirection.y = 0;
        moveDirection.Normalize();

        StartCoroutine(Move());

        dragonBoomPrefab = Resources.Load<GameObject>(dragonBoomStr);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHit = true;
            try
            {
                other.GetComponent<Health>().DealDamage(bulletDamage);
            }
            catch
            {
                Debug.Log("Помилка нанесення урона в DragonBullet.cs");
            }

            StartCoroutine(AfterEffect());
        }
    }

    private IEnumerator Move()
    {
        while (distance < bulletDistance || isHit)
        {
            transform.position += moveDirection * Time.fixedDeltaTime * bulletSpeed;
            distance += Time.fixedDeltaTime * bulletSpeed;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(AfterEffect());
    }

    private IEnumerator AfterEffect()
    {
        Vector3 position = transform.position;

        this.transform.position = new Vector3(0f, -10f, 0f);

        GameObject dragonBoom = Instantiate(dragonBoomPrefab, position, Quaternion.identity);

        yield return new WaitForSeconds(1f);
        Debug.Log("after WaitFor");

        Destroy(dragonBoom);
        Destroy(gameObject);
    }
}
