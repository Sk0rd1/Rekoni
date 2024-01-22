using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class SSM : MonoBehaviour
{
    private float timeToDetonate = 100f;
    private bool isDetonate = false;
    private float angularSpeed = 10f;
    private int damage = 0;
    private List<GameObject> enemys = new List<GameObject>();

    public void SetValues(int damage)
    {
        this.damage = damage;
        GameObject circle = gameObject.transform.GetChild(0).gameObject;
        circle.SetActive(true);
        StartCoroutine(DetonateTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemys.Contains(other.gameObject));
                enemys.Add(other.gameObject);
            StartCoroutine(Detonate());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            try
            {
                enemys.Remove(other.gameObject);
            }
            catch { }
        }
    }

    private IEnumerator DetonateTimer()
    {
        yield return new WaitForSeconds(timeToDetonate);

        if (!isDetonate)
            StartCoroutine(Detonate());
    }

    private IEnumerator Detonate()
    {
        //yield return TimeManager.WaitForSecondsCharacter(1f);

        GameObject bomb = gameObject.transform.GetChild(1).gameObject;

        float currentTime = 0f;

        float angleX = Random.Range(-15f, 15f);
        float angleZ = Random.Range(-15f, 15f);

        float currentAngleX = 0f;
        float currentAngleZ = 0f;

        bool toAngleX = true;
        bool toAngleZ = true;

        while (currentTime < 2f)
        {
            if (toAngleX)
            {
                currentAngleX += angleX * angularSpeed * Time.deltaTime;
                if (Mathf.Abs(currentAngleX) >= angleX)
                    toAngleX = false;
            }
            else
            {
                currentAngleX -= angleX * angularSpeed * Time.deltaTime;
                if (Mathf.Abs(currentAngleX) <= 0.5f)
                {
                    toAngleX = true;
                    angleX = Random.Range(-15f, 15f);
                }
            }

            if (toAngleZ)
            {
                currentAngleZ += angleZ * angularSpeed * Time.deltaTime;
                if (Mathf.Abs(currentAngleZ) >= angleZ)
                    toAngleZ = false;
            }
            else
            {
                currentAngleZ -= angleX * angularSpeed * Time.deltaTime;
                if (Mathf.Abs(currentAngleZ) <= 0.5f)
                {
                    toAngleZ = true;
                    angleZ = Random.Range(-15f, 15f);
                }
            }

            bomb.transform.rotation = Quaternion.Euler(currentAngleX, 0f, currentAngleZ);
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        foreach (var en in enemys)
        {
            try
            {
                en.GetComponent<EnemysHealth>().Damage(damage, TypeDamage.Force);

                Vector3 enDirection = en.transform.position - gameObject.transform.position;
                enDirection.y = 0f;
                enDirection.Normalize();
                enDirection *= 5000f;

                en.GetComponent<Rigidbody>().AddForce(enDirection);
            }
            catch { }
        }
        
        Destroy(gameObject);
    }
}
