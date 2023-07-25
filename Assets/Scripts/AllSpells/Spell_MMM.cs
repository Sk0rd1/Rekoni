using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Spell_MMM : MonoBehaviour
{
    private float offsetCursorPosition = 1.2f;
    private float timeCast = 1.5f;
    private float cursorSpeed = 10f;

    private bool isSpellCast = false;
    private float currentTime = 0f;
    private bool firstFrameToCast = true;
    private float reloadUnderMeteor = 0.2f;
    private int numberMeteor = 6;
    private float speefFall = 150;

    private GameObject character;
    private Vector3 centerSpell = Vector3.zero;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorForEffectPrefabModel;
    private GameObject cursorModel;
    private List<GameObject> cursorList;
    private List<GameObject> effectList;
    private List<Vector3> cursorLocation = new List<Vector3>();

    private string cursorName = "MMM/CT_MMM";
    private string effectName = "MMM/ES_MMM";
    private string cursorForEffectName = "MMM/CFE_MMM";

    void Start()
    {
        character = GameObject.Find("CharacterGirl");

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
            cursorList.Add(instantinateCurdorForEffect);
        }
    }

    public void CastSpell(Vector3 cursorDirection)
    {
        if (firstFrameToCast)
        {
            firstFrameToCast = false;
            StartCoroutine(TimerCast());
        }
        else
        {
            if (isSpellCast)
            {
                cursorModel.transform.position = character.transform.position + cursorDirection * cursorSpeed * (currentTime + offsetCursorPosition);
            }
            else
            {
                cursorModel.transform.position = character.transform.position + cursorDirection * cursorSpeed * (timeCast + offsetCursorPosition);
            }
        }
    }

    public void CastSpellEnd(Vector3 cursorDirection)
    {
        firstFrameToCast = true;

        centerSpell = cursorModel.transform.position;

        cursorModel.transform.position += new Vector3(0f, -20f, 0f);

        StartCoroutine(EffectCast());
    }

    IEnumerator EffectCast()
    {
        float currentTime = 0f;
        for(int i = 0; i < numberMeteor; i++)
        {
            cursorLocation[i] = new Vector3(centerSpell.x + Random.Range(0f, 16f) - 8f, centerSpell.y, centerSpell.z + Random.Range(0f, 16f) - 8f);            
        }

        for(int i = 0; i < numberMeteor; i++) 
        {
            cursorList[i].transform.position = cursorLocation[i];
            effectList[i].transform.position = cursorLocation[i] + new Vector3(Random.Range(0f, 16f) - 8f, 70f, Random.Range(0f, 16f) - 8f);
            //StartCoroutine(MoveToPoint(effectList[i], cursorLocation[i], i));
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

        while (effect.transform.position.y > point.y)
        {
            sphereOutsideRotation = sphereOutside.transform.eulerAngles;
            sphereOutside.transform.rotation = Quaternion.Euler(sphereOutsideRotation.x + angle1.x, sphereOutsideRotation.y + angle1.y, sphereOutsideRotation.z + angle1.z);

            sphereMiddleRotation = sphereMiddle.transform.eulerAngles;
            sphereMiddle.transform.rotation = Quaternion.Euler(sphereOutsideRotation.x + angle2.x, sphereOutsideRotation.y + angle2.y, sphereOutsideRotation.z + angle2.z);

            effect.transform.position -= (effect.transform.position - point).normalized * Time.deltaTime * speefFall;
            yield return new WaitForEndOfFrame();
        }

        cursorList[index].transform.position = new Vector3(0f, -20f, 0f);

        float scaleMax = 1.5f;
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

        effect.transform.localScale = localScale;
        effect.transform.position = new Vector3(0f, -20f, 0f);

        
        yield return null;
    }

    IEnumerator TimerCast()
    {
        isSpellCast = true;
        currentTime = 0;
        while (currentTime < timeCast)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        isSpellCast = false;
    }
}
