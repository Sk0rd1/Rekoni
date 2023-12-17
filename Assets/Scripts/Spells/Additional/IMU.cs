using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class IMU : MonoBehaviour
{
    private int damage;
    private float duration;
    private float radius;
    private float period = 0.2f;

    private bool isParent = false;

    private struct ListForIMU
    {
        public GameObject enemy;
        public GameObject imu;

        public ListForIMU(GameObject enemy, GameObject imu)
        {
            this.enemy = enemy;
            this.imu = imu;
        }

    }

    List<ListForIMU> list;

    public void SetValues(int damage, float duration, float radius, GameObject enemy, bool isParent)
    {
        list = new List<ListForIMU>();
        this.damage = damage;
        this.duration = duration;
        this.radius = radius;
        this.isParent = isParent;
        StartCoroutine(Move(enemy));
        if(isParent)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private IEnumerator Move(GameObject enemy)
    {
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
        if (isParent)
        {
            if (other.CompareTag("Enemy"))
            {
                /*bool isNear = false;
                var allColliders = Physics.OverlapSphere(other.transform.position, 5f);
                foreach (var collider in allColliders)
                {
                    Debug.Log(other.tag);
                    if (other.CompareTag("IMU"))
                    {
                        isNear = true;
                        break;
                    }
                }*/

                GameObject go = Instantiate(Resources.Load<GameObject>("IMU/Light"), other.transform);
                IMU imu = go.GetComponent<IMU>();
                imu.SetValues(damage, duration, radius, other.gameObject, false);

                list.Add(new ListForIMU(other.gameObject, go));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isParent)
        {
            if (other.CompareTag("Enemy"))
            {
                ListForIMU result = list.Find(item => item.enemy == other.gameObject);

                if (result.imu != null)
                {
                    list.Remove(result);
                    Destroy(result.imu);
                }
            }
        }
    }

    /*private void Share(GameObject defaultEnemy)
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
    }*/
}

