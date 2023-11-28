using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IMU : MonoBehaviour
{
    private int damage;
    private float duration;
    private float radius;
    private float period = 0.2f;

    public void SetValues(int damage, float duration, float radius)
    {
        this.damage = damage;
        this.duration = duration;
        this.radius = radius;
    }

    public void MoveTo(GameObject enemy)
    {
        StartCoroutine(Move(enemy));
    }

    private IEnumerator Move(GameObject enemy)
    {
        StartCoroutine(MoveLight(enemy));
        EnemysHealth eh = enemy.GetComponent<EnemysHealth>();
        float currentDuration = 0;
        while (currentDuration <= duration)
        {
            if (eh.IsDeath()) break;
            //Share(enemy);
            try
            {
                duration += period;
            
                eh.Damage(damage, TypeDamage.Thunder);
            }
            catch 
            {
                break;
            }
            yield return new WaitForSeconds(period);
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))   
        {
            /*var allColliders = Physics.OverlapSphere(other.transform.position, 0.5f);
            foreach (var collider in allColliders)
            {
                if (other.CompareTag("IMU"))
                    Debug.Log("GG");
            }*/
            if (Vector3.Distance(other.transform.position, this.transform.position) > 0.5f)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("IMU/Light"));
                IMU imu = go.GetComponent<IMU>();
                imu.SetValues(damage, duration, radius);
                imu.MoveTo(other.gameObject);
            }
        }
        
    }

    private IEnumerator MoveLight(GameObject enemy)
    {
        EnemysHealth enemysHealth = enemy.GetComponent<EnemysHealth>();
        while (!enemysHealth.IsDeath())
        {
            transform.position = enemy.transform.position;
            yield return new WaitForEndOfFrame();
        }
    }


    private void Share(GameObject defaultEnemy)
    {
        var enemyColliders = Physics.OverlapSphere(defaultEnemy.transform.position, 1f, LayerMask.GetMask("Enemy"));
        var allColliders = Physics.OverlapSphere(defaultEnemy.transform.position, 1f);

        List<Collider> imuColliders = new List<Collider>();

        foreach (var allCollider in allColliders)
        {
            if (allCollider.CompareTag("IMU"))
            {
                imuColliders.Add(allCollider);
            }
        }

        Debug.Log("EC " + enemyColliders.Length + "  IC " + imuColliders.Count);

        foreach (var enemyCollider in enemyColliders)
        {
            bool isNear = false;

            foreach (var imuCollider in imuColliders)
            {
                if (enemyCollider.transform.position == imuCollider.transform.position)
                {
                    isNear = true;
                    Debug.Log(true);
                    break;
                }
            }

            if (!isNear)
            {
                Debug.Log("!isNear");
                GameObject go = Instantiate(Resources.Load<GameObject>("IMU/Light"));
                IMU imu = go.GetComponent<IMU>();
                imu.SetValues(damage, duration, radius);
                imu.MoveTo(enemyCollider.gameObject);
            }
            Debug.Log("End");
        }
    }
}
