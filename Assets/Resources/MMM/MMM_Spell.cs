using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class MMM_Spell : SpellUniversal
{
    [SerializeField]
    private float reloadTime = 4f;
    [SerializeField]
    private int startDamage = 10;
    [SerializeField]
    private int increasDamage = 1;
    [SerializeField]
    private float fireDuration = 4f;
    [SerializeField]
    private float precision = 30f; // ��������� ���������� ��������� � ����������� ���� 30%, ������ ����������. ������������ ����������� �� ������ 3:3:4 = �����:��������:������

    public const bool MOMENTARYCAST = false;

    private float reloadUnderMeteor = 0.2f;
    private int numberMeteor = 6;
    private float speefFall = 100;
    private float spellDistance = 20f;
    private float currentReload = 0f;

    private bool isSpellReady = true;

    private Vector3 centerSpell = Vector3.zero;
    private Vector3 currentGamepadPosition = Vector3.zero;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorForEffectPrefabModel;
    private GameObject boomEffectPrefab;
    private GameObject cursorModel;
    private List<GameObject> cursorList;
    private List<GameObject> effectList;
    private List<Vector3> cursorLocation = new List<Vector3>();

    private string cursorName = "MMM/CT_MMM";
    private string effectName = "MMM/ES_MMM";
    private string cursorForEffectName = "MMM/CFE_MMM";
    private string boomEffectName = "MMM/BoomEffect";

    private void Start()
    {
        currentGamepadPosition = Vector3.zero;
        cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
    }

    public override bool IsMomemtaryCast()
    {
        return MOMENTARYCAST;
    }

    public override bool IsSpellReady()
    {
        return isSpellReady;
    }

    public override float TimeReload()
    {
        return currentReload;
    }

    public override void FirstStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(true);

        centerSpell = mousePosition;

        cursorModel.transform.position = centerSpell;
    }

    public override void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        currentGamepadPosition = Vector3.zero;
        FirstStageOfCast(mousePosition, characterPosition, isGamepadUsing);
        StartCoroutine(Reload());
        cursorModel.SetActive(false);
        effectList = new List<GameObject>();
        cursorList = new List<GameObject>();

        for (int i = 0; i < numberMeteor; i++)
        {
            cursorLocation.Add(Vector3.zero);
        }

        for (int i = 0; i < numberMeteor; i++)
        {
            effectPrefabModel = Resources.Load<GameObject>(effectName);
            GameObject instantinateEffect = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
            effectList.Add(instantinateEffect);
            MMM mmm = instantinateEffect.GetComponentInChildren<MMM>();
            mmm.SetValues(startDamage, increasDamage, fireDuration);

            cursorForEffectPrefabModel = Resources.Load<GameObject>(cursorForEffectName);
            GameObject instantinateCurdorForEffect = Instantiate(cursorForEffectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
            instantinateCurdorForEffect.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            cursorList.Add(instantinateCurdorForEffect);
        }

        //centerSpell = cursorModel.transform.position;

        StartCoroutine(EffectCast());
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        currentGamepadPosition = Vector3.zero;
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

    IEnumerator EffectCast()
    {
        int numberCircle;
        for (int i = 0; i < numberMeteor; i++)
        {
            numberCircle = Random.Range(0, 101);
            if (numberCircle <= (int)precision)
            {
                float x = Random.Range(0f, 4f) - 2f;
                float z = Random.Range(0f, 4f) - 2f;
                cursorLocation[i] = new Vector3(centerSpell.x + x, centerSpell.y + 0.5f, centerSpell.z + z);
            }
            else if (numberCircle <= precision * 2)
            {
                float x = Random.Range(0f, 4f) - 2f;
                float z = Random.Range(0f, 4f) - 2f;

                if (x < 0)
                    x -= 2f;
                else
                    x += 2f;

                if (z < 0)
                    z -= 2f;
                else
                    z += 2f;

                cursorLocation[i] = new Vector3(centerSpell.x + x, centerSpell.y + 0.5f, centerSpell.z + z);
            }
            else
            {
                float x = Random.Range(0f, 4f) - 2f;
                float z = Random.Range(0f, 4f) - 2f;

                if (x < 0)
                    x -= 4f;
                else
                    x += 4f;

                if (z < 0)
                    z -= 4f;
                else
                    z += 4f;

                cursorLocation[i] = new Vector3(centerSpell.x + x, centerSpell.y + 0.5f, centerSpell.z + z);
            }
        }

        for (int i = 0; i < numberMeteor; i++)
        {
            cursorList[i].transform.position = cursorLocation[i];
            effectList[i].transform.position = cursorLocation[i] + new Vector3(Random.Range(0f, 16f) - 8f, 70f, Random.Range(0f, 16f) - 8f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < numberMeteor; i++)
        {
            StartCoroutine(MoveToPoint(effectList[i], cursorLocation[i], i));
            yield return new WaitForSeconds(reloadUnderMeteor);
        }
    }

    IEnumerator MoveToPoint(GameObject effect, Vector3 point, int index)
    {
        GameObject sphereOutside = effect.transform.Find("SphereOutside").gameObject;
        Vector3 sphereOutsideRotation;

        GameObject sphereMiddle = effect.transform.Find("SphereMiddle").gameObject;
        Vector3 sphereMiddleRotation;

        Vector3 angle1 = new Vector3(Random.Range(50f, 180f) * Time.deltaTime, Random.Range(50f, 180f) * Time.deltaTime, Random.Range(50f, 180f) * Time.deltaTime);
        Vector3 angle2 = new Vector3(Random.Range(50f, 180f) * Time.deltaTime, Random.Range(50f, 180f) * Time.deltaTime, Random.Range(50f, 180f) * Time.deltaTime);

        while (effect.transform.position.y - 0.5f > point.y)
        {
            sphereOutsideRotation = sphereOutside.transform.eulerAngles;
            sphereOutside.transform.rotation = Quaternion.Euler(sphereOutsideRotation.x + angle1.x, sphereOutsideRotation.y + angle1.y, sphereOutsideRotation.z + angle1.z);

            sphereMiddleRotation = sphereMiddle.transform.eulerAngles;
            sphereMiddle.transform.rotation = Quaternion.Euler(sphereOutsideRotation.x + angle2.x, sphereOutsideRotation.y + angle2.y, sphereOutsideRotation.z + angle2.z);

            effect.transform.position -= (effect.transform.position - point).normalized * Time.deltaTime * speefFall;
            yield return new WaitForEndOfFrame();
        }

        Destroy(cursorList[index]);

        /*float scaleMax = 1.5f;
        float currentScale = 1.0f;
        Vector3 localScale = effect.transform.localScale;
        while (currentScale <= scaleMax)
        {
            sphereOutsideRotation = sphereOutside.transform.eulerAngles;
            sphereOutside.transform.rotation = Quaternion.Euler(sphereOutsideRotation.x + angle1.x, sphereOutsideRotation.y + angle1.y, sphereOutsideRotation.z + angle1.z);

            sphereMiddleRotation = sphereMiddle.transform.eulerAngles;
            sphereMiddle.transform.rotation = Quaternion.Euler(sphereOutsideRotation.x + angle2.x, sphereOutsideRotation.y + angle2.y, sphereOutsideRotation.z + angle2.z);

            effect.transform.localScale = localScale * currentScale;
            currentScale += 5f * Time.deltaTime;
            effect.transform.position -= new Vector3(0f, 4 * Time.deltaTime, 0f);
            yield return new WaitForEndOfFrame();
        }

        effect.transform.localScale = localScale;*/
        boomEffectPrefab = Resources.Load<GameObject>(boomEffectName);
        GameObject boomEffect = Instantiate(boomEffectPrefab, effect.transform.position, Quaternion.identity);
        effect.transform.position = new Vector3(0f, -20f, 0f);
        Destroy(effect);
        yield return new WaitForSeconds(2f);
        Destroy(boomEffect);
    }

    public override float RadiusCast()
    {
        return spellDistance;
    }
}
