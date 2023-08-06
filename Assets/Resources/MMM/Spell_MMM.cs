using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Spell_MMM : MonoBehaviour
{
    public readonly bool MOMENTARYCAST = false;

    private float reloadUnderMeteor = 0.2f;
    private int numberMeteor = 6;
    private float speefFall = 150;
    private float reloadTime = 4f;
    private float radiusSpell = 15f;


    private bool firstFrameToCast = true;
    private float currentTime = 0f;
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

    void Start()
    {
        cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
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

            cursorForEffectPrefabModel = Resources.Load<GameObject>(cursorForEffectName);
            GameObject instantinateCurdorForEffect = Instantiate(cursorForEffectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
            instantinateCurdorForEffect.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            cursorList.Add(instantinateCurdorForEffect);
        }
    }

    public void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        if (firstFrameToCast)
        {
            firstFrameToCast = false;
            currentGamepadPosition = Vector3.zero;
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

    public void CancelSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.transform.position += new Vector3(0f, -20f, 0f);
    }

    public void CastSpellEnd(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
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
            numberCircle = Random.Range(0, 11);

            if (numberCircle < 6)
            {
                float x = Random.Range(0f, 4f) - 2f;
                float z = Random.Range(0f, 4f) - 2f;
                cursorLocation[i] = new Vector3(centerSpell.x + x, centerSpell.y + 0.5f, centerSpell.z + z);
            }
            else if (numberCircle < 9)
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

        cursorList[index].transform.position = new Vector3(0f, -20f, 0f);

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
    }

    private Vector3 DistanceWithRadius(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        Vector3 pointCenterSpell = PointCenterSpell(cursorPosition, characterPosition, isGamepadUsing) - characterPosition;

        if (isGamepadUsing)
        {
            if(Mathf.Sqrt(pointCenterSpell.x * pointCenterSpell.x + pointCenterSpell.z * pointCenterSpell.z) < radiusSpell)
            {
                return pointCenterSpell + characterPosition;
            }
            else
            {
                pointCenterSpell.y = 0f;
                pointCenterSpell.Normalize();

                currentGamepadPosition = pointCenterSpell * radiusSpell;

                pointCenterSpell = characterPosition + currentGamepadPosition;

                return pointCenterSpell;
            }
        }
        else
        {
            float deltaX = cursorPosition.x - characterPosition.x;
            float deltaZ = cursorPosition.z - characterPosition.z;

            if (Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ) < radiusSpell)
            {
                return pointCenterSpell + characterPosition;
            }
            else
            {
                Vector3 pointDirection = cursorPosition - characterPosition;
                pointDirection.y = 0f;
                pointDirection.Normalize();

                pointCenterSpell = characterPosition + pointDirection * radiusSpell;

                return pointCenterSpell;
            }
        }
    }

    IEnumerator Reload()
    {
        currentTime = 0;
        isSpellReady = false;
        while (currentTime < reloadTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        isSpellReady = true;
    }

    public bool IsSpellReady()
    {
        return isSpellReady;
    }
}
