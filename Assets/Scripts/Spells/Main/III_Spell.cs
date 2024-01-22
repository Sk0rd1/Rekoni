using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class III_Spell : SpellUniversal
{
    private float reloadTime = 4f;
    private int damage = 4;
    private float timeStunned = 1.5f;
    private float distanceArrow = 26f;
    private int numberOfArrows = 10;

    public const bool MOMENTARYCAST = false;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorModel;

    private string cursorName = "_Cursors/Pizza60III";
    private string effectName = "III/IceArrow";

    //private float effectRadius = 3f;
    private bool isSpellReady = true;
    private float spellDistance = 20f;
    private float currentReload = 0f;

    private List<GameObject> effectList = new List<GameObject>();

    void Start()
    {
        cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.EulerAngles(-90f, 0f, 0f));
        //cursorModel.transform.localScale *= 3f; 
        cursorModel.SetActive(false);

        for (int i = 0; i < numberOfArrows; i++)
        {
            effectPrefabModel = Resources.Load<GameObject>(effectName);
            GameObject instantinateEffect = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
            instantinateEffect.SetActive(false);
            effectList.Add(instantinateEffect);
            III iii = instantinateEffect.GetComponent<III>();
            iii.SetValues(damage, timeStunned);
        }
    }

    public override bool IsMomemtaryCast()
    {
        return MOMENTARYCAST;
    }

    public override bool IsSpellReady()
    {
        return isSpellReady;
    }

    public override float RadiusCast()
    {
        return spellDistance;
    }

    public override float TimeReload()
    {
        return currentReload;
    }

    public override void FirstStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(true);
        cursorModel.transform.position = characterPosition;
        Vector3 lookRotation = mousePosition - characterPosition;
        lookRotation.y = 0f;
        lookRotation.Normalize();
        float rotationY = Mathf.Atan2(lookRotation.z, lookRotation.x) * Mathf.Rad2Deg;

        cursorModel.transform.rotation = Quaternion.Euler(90f, - rotationY + 90, 0); //для інших курсорів міняти тільки Y
    }

    public override void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        StartCoroutine(Reload());

        cursorModel.transform.position += new Vector3(0f, -20f, 0f);
        cursorModel.SetActive(false);

        for (int i = 0; i < numberOfArrows; i++)
        {
            StartCoroutine(MoveArrow(i, mousePosition, characterPosition));
        }
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(false);
    }

    IEnumerator Reload()
    {
        isSpellReady = false;
        currentReload = reloadTime;
        while (currentReload >= 0f)
        {
            currentReload -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isSpellReady = true;
    }

    private IEnumerator MoveArrow(int num, Vector3 mousePosition, Vector3 characterPosition)
    {
        Vector3 characterDirection = mousePosition - characterPosition;
        characterDirection.Normalize();
        int speedArrow = Random.Range(30, 50);
        int angularArrow = Random.Range(-30, 30);

        float characterRotation = Mathf.Atan2(characterDirection.y, characterDirection.x);
        float angularArrowRad = angularArrow * Mathf.PI / 180; // перетворення градусів в радіани

        Vector3 finalPoint = RotatePointAroundPivot(mousePosition, characterPosition, angularArrow);

        effectList[num].transform.position = characterPosition + characterDirection * 2f;
        effectList[num].transform.position = new Vector3(effectList[num].transform.position.x, mousePosition.y, effectList[num].transform.position.z);
        effectList[num].SetActive(true);
        Vector3 finalDirection = finalPoint - characterPosition;
        finalDirection.y = 0;
        finalDirection.Normalize();

        float currentDistance = 0f;
        effectList[num].transform.LookAt(finalPoint);
        while (currentDistance < distanceArrow)
        {
            effectList[num].transform.position += finalDirection * Time.deltaTime * speedArrow;
            currentDistance += speedArrow * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        effectList[num].transform.position += new Vector3(0f, -30f, 0f);
        yield return new WaitForSeconds(0.5f);
        effectList[num].SetActive(false);
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
    {
        float valueY = point.y;
        point.y = 0;
        pivot.y = 0;
        Vector3 dir = point - pivot;
        dir.y = 0;
        dir = Quaternion.Euler(0, angle, 0) * dir;
        point = dir + pivot;
        point.y = valueY;
        return point;
    }
}
