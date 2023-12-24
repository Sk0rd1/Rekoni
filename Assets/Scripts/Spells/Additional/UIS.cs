using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX.Utility;

public class UIS : MonoBehaviour
{
    private float timeLife = 1f;
    private int damage = 0;

    private string effectName = "UIS/ArcLight";
    private GameObject effectModel;

    List<GameObject> listEnemys = new List<GameObject>();

    public UIS(Transform targetEnemy, int damage)
    {
        this.damage = damage;
        GameObject characterGirl = GameObject.Find("CharacterGirl");
        effectModel = Resources.Load<GameObject>(effectName);

        StartCoroutine(FirstLighting(targetEnemy, characterGirl.transform));
        Debug.Log(1);
    }

    private IEnumerator FirstLighting(Transform targetEnemy, Transform characterGirl)
    {
        Debug.Log("Error1");
        GameObject gameObject = Instantiate(effectModel, characterGirl);
        VFXPropertyBinder propertyBinder = gameObject.GetComponent<VFXPropertyBinder>();
        Debug.Log("Error2");

        ArcLightBinder[] arcLightBinder = gameObject.GetComponentsInChildren<ArcLightBinder>();

        Debug.Log("Error3");

        arcLightBinder[0].property = "Pos1";
        arcLightBinder[0].target = characterGirl;

        Debug.Log("Error4");
        arcLightBinder[1].property = "Pos2";
        arcLightBinder[1].target = characterGirl;

        arcLightBinder[2].property = "Pos3";
        arcLightBinder[2].target = targetEnemy;

        arcLightBinder[3].property = "Pos4";
        arcLightBinder[3].target = targetEnemy;

        yield return new WaitForSeconds(1f);

        Debug.Log(2);
        Destroy(gameObject);
    }
}
