using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class III_Spell : SpellUniversal
{
    [SerializeField]
    private float reloadTime = 4f;
    [SerializeField]
    private int damage = 4;
    [SerializeField]
    private float timeStunned = 1.5f;
    [SerializeField]
    private float distanceArrow = 10f;
    [SerializeField]
    private int numberOfArrows = 6;

    public const bool MOMENTARYCAST = false;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorModel;
    private GameObject effectModel;

    private string cursorName = "III/Cursor";
    private string effectName = "III/IceArrow";

    private float effectRadius = 3f;
    private bool isSpellReady = true;
    private float spellDistance = 20f;
    private float currentReload = 0f;

    private List<GameObject> effectList = new List<GameObject>();

    void Start()
    {
        cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        cursorModel.SetActive(false);

        effectPrefabModel = Resources.Load<GameObject>(effectName);
        effectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        effectModel.SetActive(false);

        for (int i = 0; i < numberOfArrows; i++)
        {
            effectPrefabModel = Resources.Load<GameObject>(effectName);
            GameObject instantinateEffect = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
            effectList.Add(instantinateEffect);
            //MMM mmm = instantinateEffect.GetComponentInChildren<MMM>();
            //mmm.SetValues(startDamage, increasDamage, fireDuration);
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
        cursorModel.transform.position = mousePosition;
    }

    public override void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        StartCoroutine(Reload());
        effectModel.SetActive(true);

        effectModel.transform.position = cursorModel.transform.position;

        cursorModel.transform.position += new Vector3(0f, -20f, 0f);

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
        int speedArrow = Random.Range(10, 20);
        int angularArrow = Random.Range(-45, 45);

        float characterRotation = Mathf.Atan2(characterDirection.y, characterDirection.x);
        float angularArrowRad = angularArrow * Mathf.PI / 180; // перетворення градусів в радіани

        Vector3 finalPoint = new Vector3(
            characterPosition.x + distanceArrow * Mathf.Cos(characterRotation + angularArrowRad),
            characterPosition.y,
            characterPosition.z + distanceArrow * Mathf.Sin(characterRotation + angularArrowRad)
        );
        Debug.Log(num + " " + finalPoint);

        effectList[num].SetActive(true);
        effectList[num].transform.position = characterPosition + characterDirection * 2f;
        Vector3 finalDirection = finalPoint.normalized;

        float currentDistance = 0f;
        while (Vector3.Distance(characterPosition, finalPoint) < distanceArrow)
        {
            effectList[num].transform.LookAt(finalPoint);
            effectList[num].transform.position += finalDirection * Time.deltaTime * speedArrow;
            yield return new WaitForEndOfFrame();
        }
    }
}
