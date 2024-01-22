using RobinGoodfellow.CircleGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class UIS_Spell : SpellUniversal
{
    private float reloadTime = 4f;
    private int damage = 10;

    public const bool MOMENTARYCAST = false;

    private GameObject cursorModel;
    private GameObject hintModel;
    private GameObject radiusModel;
    private GameObject enemy;

    private bool isSpellReady = true;
    private float currentReload = 0f;

    void Start()
    {
        enemy = null;

        cursorModel = new GameObject();
        cursorModel.AddComponent<StrokeCircleGenerator>();
        StrokeCircleGenerator strokeCircleGenerator = cursorModel.GetComponent<StrokeCircleGenerator>();
        CircleData circleDataStroke = new CircleData(2f, 360f, 0f, 128, false);
        StrokeData strokeDataStroke = new StrokeData(0.16f, false);
        strokeCircleGenerator.CircleData = circleDataStroke;
        strokeCircleGenerator.StrokeData = strokeDataStroke;
        Material materialStroke = Resources.Load<Material>("_Cursors/PizzaMaterial");
        strokeCircleGenerator.GetComponent<MeshRenderer>().material = materialStroke;
        strokeCircleGenerator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        cursorModel.SetActive(false);


        radiusModel = new GameObject();
        radiusModel.AddComponent<DashCircleGenerator>();
        DashCircleGenerator dashCircleGenerator = radiusModel.GetComponent<DashCircleGenerator>();
        CircleData circleDataDash = new CircleData(RadiusCast() * 1.1f, 360f, 0f, 128, false);
        StrokeData strokeDataDash = new StrokeData(0.16f, false);
        dashCircleGenerator.CircleData = circleDataDash;
        dashCircleGenerator.StrokeData = strokeDataDash;
        Material materialDash = Resources.Load<Material>("_Cursors/PizzaMaterial");
        dashCircleGenerator.GetComponent<MeshRenderer>().material = materialDash;
        dashCircleGenerator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        radiusModel.SetActive(false);

        hintModel = new GameObject();
        hintModel.AddComponent<DashCircleGenerator>();
        DashCircleGenerator dashCircleGeneratorHint = hintModel.GetComponent<DashCircleGenerator>();
        CircleData circleDataDashHint = new CircleData(2f, 360f, 0f, 32, false);
        StrokeData strokeDataDashHint = new StrokeData(0.16f, false);
        dashCircleGeneratorHint.CircleData = circleDataDashHint;
        dashCircleGeneratorHint.StrokeData = strokeDataDashHint;
        Material materialDashHint = Resources.Load<Material>("_Cursors/PizzaMaterial");
        dashCircleGeneratorHint.GetComponent<MeshRenderer>().material = materialDashHint;
        dashCircleGeneratorHint.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        hintModel.SetActive(false);
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
        return 20f;
    }

    public override float TimeReload()
    {
        return currentReload;
    }

    public override void FirstStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(true);
        radiusModel.SetActive(true);
        hintModel.SetActive(true);
        enemy = FindClosestWithTag(mousePosition);
        if (enemy != null)
        {
            Vector3 targetPosition = enemy.transform.position;
            if (Vector3.Distance(targetPosition, characterPosition) <= RadiusCast() && Vector3.Distance(targetPosition, mousePosition) <= 8)
            {
                cursorModel.transform.position = targetPosition;
            }
            else
            {
                cursorModel.transform.position = mousePosition;
                enemy = null;
            }
        }
        else
        {
            cursorModel.transform.position = mousePosition;
            enemy = null;
        }

        hintModel.transform.position = mousePosition;
        radiusModel.transform.Rotate(new Vector3(0f, 0f, 3f) * Time.deltaTime);
        radiusModel.transform.position = characterPosition;
    }

    private GameObject FindClosestWithTag(Vector3 position)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public override void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        radiusModel.SetActive(false);
        hintModel.SetActive(false);
        cursorModel.SetActive(false);
        if (enemy != null)
        {
            StartCoroutine(Reload());
            //StartCoroutine(EffectCast());
            UIS uis = new UIS(enemy.transform, damage);
        }
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(false);
        radiusModel.SetActive(false);
        hintModel.SetActive(false);
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

    /*IEnumerator EffectCast()
    {
        Vector3 position = enemy.transform.position;
        GameObject gameObject = Instantiate(effectModel);
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        VFXPropertyBinder propertyBinder = gameObject.GetComponent<VFXPropertyBinder>();

        ArcLightBinder[] arcLightBinder = gameObject.GetComponentsInChildren<ArcLightBinder>();
        arcLightBinder[0].target = enemy.transform;
        arcLightBinder[0].property = "Pos1";
        arcLightBinder[1].target = enemy.transform;
        arcLightBinder[1].property = "Pos2";
        arcLightBinder[2].target = enemy.transform;
        arcLightBinder[2].property = "Pos3";
        arcLightBinder[3].target = enemy.transform;
        arcLightBinder[3].property = "Pos4";

        yield return new WaitForSeconds(1.5f);
        yield return null;
    }*/
}
