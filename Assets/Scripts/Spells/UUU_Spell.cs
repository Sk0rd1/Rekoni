using RobinGoodfellow.CircleGenerator;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UUU_Spell : SpellUniversal
{
    private float reloadTime = 4f;
    private int heroDamage = 10;

    public const bool MOMENTARYCAST = false;

    private GameObject cursorModel;
    private GameObject hintModel;
    private GameObject effectParticl;
    private GameObject radiusModel;
    private GameObject enemy;

    private string effectName = "SUU/Debuff";
    private string particlName = "SUU/Explosion";

    private bool isSpellReady = true;
    private float currentReload = 0f;

    void Start()
    {
        effectParticl = Resources.Load<GameObject>(particlName);
        enemy = null;

        cursorModel = new GameObject();
        cursorModel.AddComponent<StrokeCircleGenerator>();
        StrokeCircleGenerator strokeCircleGenerator = cursorModel.GetComponent<StrokeCircleGenerator>();
        CircleData circleDataStroke = new CircleData(2f, 360f, 0f, 128, false);
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
        CircleData circleDataDash = new CircleData(RadiusCast() * 1.1f, 360f, 0f, 128, false);
        StrokeData strokeDataDash = new StrokeData(0.16f, false);
        dashCircleGenerator.CircleData = circleDataDash;
        dashCircleGenerator.StrokeData = strokeDataDash;
        Material materialDash = Resources.Load<Material>("Cursors/PizzaMaterial");
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
        Material materialDashHint = Resources.Load<Material>("Cursors/PizzaMaterial");
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
            StartCoroutine(EffectCast());
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

    IEnumerator EffectCast()
    {
        EnemysHealth eh = enemy.GetComponent<EnemysHealth>();
        if (eh.IsBoss())
        {
            // ���� ���� ��� �� ������ �� �������
        }
        else 
        {
            eh.Damage(eh.MaxHealth());
            GameObject.Find("CharacterGirl").GetComponent<Health>().DealDamage(heroDamage);
            //��� ������� ������� �������� ������
        }
        yield return null;
    }

    private IEnumerator OneEffect(Vector3 position)
    {
        Debug.Log("OneEffect");
        GameObject gameObject = Instantiate(effectParticl);
        gameObject.transform.position = position + new Vector3(0f, 0.5f, 0f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
