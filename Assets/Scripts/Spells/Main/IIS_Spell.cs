using RobinGoodfellow.CircleGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class IIS_Spell : SpellUniversal
{
    private float stunDuration = 4f;
    private float reloadTime = 5f;
    private float radiusSpell = 1f;
    private float radiusEffect = 1f;
    private GameObject cursorModel;
    private GameObject radiusModel;
    private GameObject effectModel;
    private const bool MOMENTARYCAST = false;
    private bool isSpellReady = true;
    private float currentReload = 0f;
    private IIS iis;

    private string effectName = "IIS/FreezeCircle";

    private void Start()
    {
        cursorModel = new GameObject();
        cursorModel.AddComponent<StrokeCircleGenerator>();
        StrokeCircleGenerator strokeCircleGenerator = cursorModel.GetComponent<StrokeCircleGenerator>();
        CircleData circleDataStroke = new CircleData(radiusEffect * 4f, 360f, 0f, 128, false);
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
        CircleData circleDataDash = new CircleData(RadiusCast() * 1.341f, 360f, 0f, 128, false);
        StrokeData strokeDataDash = new StrokeData(0.16f, false);
        dashCircleGenerator.CircleData = circleDataDash;
        dashCircleGenerator.StrokeData = strokeDataDash;
        Material materialDash = Resources.Load<Material>("_Cursors/PizzaMaterial");
        dashCircleGenerator.GetComponent<MeshRenderer>().material = materialDash;
        dashCircleGenerator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        radiusModel.SetActive(false);

        GameObject effectPrefabModel = Resources.Load<GameObject>(effectName);
        effectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        iis = effectModel.GetComponent<IIS>();
        iis.SetValues(stunDuration);
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
        return 12f;
    }

    public override float TimeReload()
    {
        return currentReload;
    }

    public void SetValues(float timeCast, float radiusSpell)
    {
        this.stunDuration = timeCast;
        this.radiusSpell = radiusSpell;
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

        radiusModel.SetActive(false);
        cursorModel.SetActive(false);

        StartCoroutine(EffectCast());
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

    IEnumerator EffectCast()
    {
        iis.ISFREEZE = true;

        yield return new WaitForSeconds(0.1f);

        iis.ISFREEZE = false;

        yield return new WaitForSeconds(stunDuration - 0.1f);

        effectModel.SetActive(false);
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        radiusModel.SetActive(false);
        cursorModel.SetActive(false);
    }
}
