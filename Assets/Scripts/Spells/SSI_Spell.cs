using RobinGoodfellow.CircleGenerator;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SSI_Spell : SpellUniversal
{
    private float reloadTime = 4f;
    private float timeCast = 3f;
    private float timePeriod = 0.5f;
    private int damage = 4;
    private float forceAttraction = 0.1f;

    public const bool MOMENTARYCAST = false;

    SSI ssi;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorModel;
    private GameObject effectModel;
    private GameObject radiusModel;
    private Renderer rendererInside;
    private Material[] materialInside;

    DashCircleGenerator circle;

    private string cursorName = "Cursors/Pizza360";
    private string effectName = "SSI/BlackHole";
    private string radiusName = "Cursors/Pizza360Hole";

    private float effectRadius = 3f;
    private bool isSpellReady = true;
    private float currentReload = 0f;

    void Start()
    {
        /*cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        cursorModel.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        rendererInside = cursorModel.GetComponentInChildren<Renderer>();
        materialInside = rendererInside.materials;
        foreach (Material mat in materialInside)
        {
            mat.SetFloat("_PowerRadius", 0.002f);
        }
        cursorModel.SetActive(false);*/

        cursorModel = new GameObject();
        cursorModel.AddComponent<StrokeCircleGenerator>();
        StrokeCircleGenerator strokeCircleGenerator = cursorModel.GetComponent<StrokeCircleGenerator>();
        CircleData circleDataStroke = new CircleData(7.9f, 360f, 0f, 128, false);
        StrokeData strokeDataStroke = new StrokeData(0.16f, false);
        strokeCircleGenerator.CircleData = circleDataStroke;
        strokeCircleGenerator.StrokeData = strokeDataStroke;
        Material materialStroke = Resources.Load<Material>("Cursors/PizzaMaterial");
        strokeCircleGenerator.GetComponent<MeshRenderer>().material = materialStroke;
        strokeCircleGenerator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        cursorModel.SetActive(false);


        radiusModel = new GameObject();
        radiusModel.AddComponent<DashCircleGenerator>();
        DashCircleGenerator dashCircleGenerator = radiusModel.GetComponent<DashCircleGenerator>();
        CircleData circleDataDash = new CircleData(RadiusCast() * 1.392f, 360f, 0f, 128, false);
        StrokeData strokeDataDash = new StrokeData(0.16f, false);
        dashCircleGenerator.CircleData = circleDataDash;
        dashCircleGenerator.StrokeData = strokeDataDash;
        Material materialDash = Resources.Load<Material>("Cursors/PizzaMaterial");
        dashCircleGenerator.GetComponent<MeshRenderer>().material = materialDash;
        dashCircleGenerator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        radiusModel.SetActive(false);

        effectPrefabModel = Resources.Load<GameObject>(effectName);
        effectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        ssi = effectModel.GetComponentInChildren<SSI>();
        effectModel.SetActive(false);
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
        cursorModel.transform.position = mousePosition;
        radiusModel.transform.Rotate(new Vector3(0f, 0f, 3f) * Time.deltaTime);
        radiusModel.transform.position = characterPosition;
    }

    public override void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        StartCoroutine(Reload());
        effectModel.SetActive(true);

        effectModel.transform.position = cursorModel.transform.position;

        ssi.SetValues(timeCast, damage, forceAttraction);

        //cursorModel.transform.position += new Vector3(0f, -20f, 0f);
        radiusModel.SetActive(false);
        cursorModel.SetActive(false);

        StartCoroutine(EffectCast(effectModel));
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(false);
        radiusModel.SetActive(false);
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

    IEnumerator EffectCast(GameObject effect)
    {
        float currentEffectRadius = 0.1f;
        effect.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);

        ssi.IsCastBH = true;

        while (currentEffectRadius < effectRadius)
        {
            currentEffectRadius += 5f * Time.deltaTime;
            effect.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(timeCast);

        while (currentEffectRadius > 0.2f)
        {
            currentEffectRadius -= 10f * Time.deltaTime;
            effect.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);
            yield return new WaitForEndOfFrame();
        }

        ssi.IsCastBH = false;

        effectModel.transform.position += new Vector3(0f, -20f, 0f);
    }
}
