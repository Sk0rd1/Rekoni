using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Spell_MMM : Spell_XXX
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
    private float precision = 30f; // стандарта ймовірність попадання в центральний круг 30%, лінійна залежність. Свіввідношення ймовірностей по кругам 3:3:4 = центр:середина:ззовні

    public const bool MOMENTARYCAST = false;

    private float reloadUnderMeteor = 0.2f;
    private int numberMeteor = 6;
    private float speefFall = 100;
    private float spellDistance = 20f;

    private bool isSpellReady = true;
    private bool firstFrameToCast = true;

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

    public override void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        if (firstFrameToCast)
        {
            firstFrameToCast = false;
            currentGamepadPosition = Vector3.zero;
            cursorPrefabModel = Resources.Load<GameObject>(cursorName);
            cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        }
        else
        {
            cursorModel.transform.position = DistanceWithRadius(cursorPosition, characterPosition, isGamepadUsing);
        }
    }

    private Vector3 PointCenterSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        Vector3 pointCenterSpell = Vector3.zero;

        if (isGamepadUsing)
        {
            if(cursorPosition.x < 0f)
            {
                cursorPosition.x = cursorPosition.x * cursorPosition.x;
                cursorPosition.x = -cursorPosition.x;
            }
            else
            {
                cursorPosition.x = cursorPosition.x * cursorPosition.x;
            }

            if (cursorPosition.z < 0f)
            {
                cursorPosition.z = cursorPosition.z * cursorPosition.z;
                cursorPosition.z = -cursorPosition.z;
            }
            else
            {
                cursorPosition.z = cursorPosition.z * cursorPosition.z;
            }
            currentGamepadPosition += cursorPosition * Time.deltaTime * 20f;

            pointCenterSpell = characterPosition + currentGamepadPosition;
        }
        else
        {
            pointCenterSpell = cursorPosition;
        }

        return pointCenterSpell;
    }

    public override void CancelSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        if (cursorModel != null)
            cursorModel.transform.position += new Vector3(0f, -20f, 0f);
    }

    public override void CastSpellEnd(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        Destroy(cursorModel);
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
        StartCoroutine(Reload());

        firstFrameToCast = true;

        centerSpell = cursorModel.transform.position;

        cursorModel.transform.position += new Vector3(0f, -20f, 0f);

        StartCoroutine(EffectCast());
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

        for(int i = 0; i < numberMeteor; i++) 
        {
            cursorList[i].transform.position = cursorLocation[i];
            effectList[i].transform.position = cursorLocation[i] + new Vector3(Random.Range(0f, 16f) - 8f, 70f, Random.Range(0f, 16f) - 8f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        for(int i = 0; i < numberMeteor; i++)
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

    private Vector3 DistanceWithRadius(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        Vector3 pointCenterSpell = PointCenterSpell(cursorPosition, characterPosition, isGamepadUsing) - characterPosition;

        if (isGamepadUsing)
        {
            if(Mathf.Sqrt(pointCenterSpell.x * pointCenterSpell.x + pointCenterSpell.z * pointCenterSpell.z) < spellDistance)
            {
                return pointCenterSpell + characterPosition;
            }
            else
            {
                pointCenterSpell.y = 0f;
                pointCenterSpell.Normalize();

                currentGamepadPosition = pointCenterSpell * spellDistance;

                pointCenterSpell = characterPosition + currentGamepadPosition;

                return pointCenterSpell;
            }
        }
        else
        {
            float deltaX = cursorPosition.x - characterPosition.x;
            float deltaZ = cursorPosition.z - characterPosition.z;

            if (Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ) < spellDistance)
            {
                return pointCenterSpell + characterPosition;
            }
            else
            {
                Vector3 pointDirection = cursorPosition - characterPosition;
                pointDirection.y = 0f;
                pointDirection.Normalize();

                pointCenterSpell = characterPosition + pointDirection * spellDistance;

                return pointCenterSpell;
            }
        }
    }

    IEnumerator Reload()
    {
        isSpellReady = false;
        yield return new WaitForSeconds(reloadTime);
        isSpellReady = true;
    }

    public override bool MomentaryCast()
    {
        return MOMENTARYCAST;
    }

    public override bool IsSpellReady()
    {
        return isSpellReady;
    }
}
