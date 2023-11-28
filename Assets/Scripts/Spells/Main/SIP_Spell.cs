using RobinGoodfellow.CircleGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIP_Spell : SpellUniversal
{
    private float castRange = 25f;
    private float reloadTime = 1f;

    public const bool MOMENTARYCAST = false;
    private bool isSpellReady = true;
    private float currentReload = 0f;

    private string effectName = "SIP/Shoot";

    private GameObject cursorModel;
    private GameObject radiusModel;

    private void Start()
    {
        cursorModel = new GameObject();
        cursorModel.AddComponent<StrokeCircleGenerator>();
        StrokeCircleGenerator strokeCircleGenerator = cursorModel.GetComponent<StrokeCircleGenerator>();
        CircleData circleDataStroke = new CircleData(4f, 360f, 0f, 128, false);
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
        CircleData circleDataDash = new CircleData(castRange + 4f, 360f, 0f, 128, false);
        StrokeData strokeDataDash = new StrokeData(0.16f, false);
        dashCircleGenerator.CircleData = circleDataDash;
        dashCircleGenerator.StrokeData = strokeDataDash;
        Material materialDash = Resources.Load<Material>("_Cursors/PizzaMaterial");
        dashCircleGenerator.GetComponent<MeshRenderer>().material = materialDash;
        dashCircleGenerator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        radiusModel.SetActive(false);
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
        return castRange;
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
        radiusModel.SetActive(false);
        cursorModel.SetActive(false);
        StartCoroutine(Effect(mousePosition, characterPosition));
    }

    private IEnumerator Effect(Vector3 mousePosition, Vector3 characterPosition)
    {
        GameObject effect = Resources.Load<GameObject>(effectName);

        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < 8; i++)
        {
            GameObject gameObject1 = Instantiate(effect);
            GameObject gameObject2 = Instantiate(effect);
            gameObject1.transform.position = mousePosition + new Vector3(Random.Range(-2, 2), Random.Range(1, 7), Random.Range(-2, 2));
            gameObject2.transform.position = characterPosition + new Vector3(Random.Range(-2, 2), Random.Range(1, 7), Random.Range(-2, 2));
            list.Add(gameObject1);
            list.Add(gameObject2);
        }

        GameObject character = GameObject.Find("CharacterGirl");

        character.GetComponent<MovementCharacter>().StopMoveOnOneFram();
        character.transform.position = mousePosition;

        yield return new WaitForSeconds(3);

        for(int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
            list.Remove(list[i]);
        }
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        radiusModel.SetActive(false);
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
}
