using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_SSI : MonoBehaviour
{
    [SerializeField]
    private float offsetCursorPosition = 1f;
    [SerializeField]
    private float timeCast = 2f;
    [SerializeField]
    private float cursorSpeed = 5f;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorModel;
    private GameObject effectModel;

    private string cursorName = "SSI/Cursor";
    private string effectName = "SSI/BlackHole";

    private bool isSpellCast = false;
    private float currentTime = 0f;
    private bool firstFrameToCast = true;
    private float effectRadius = 7f;

    private Vector3 currentGamepadPosition = Vector3.zero;



    void Start()
    {
        cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);

        effectPrefabModel = Resources.Load<GameObject>(effectName);
        effectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);

    }

    public void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        if (firstFrameToCast)
        {
            currentGamepadPosition = Vector3.zero;
            firstFrameToCast = false;
            StartCoroutine(TimerCast());
        }
        else
        {
            cursorModel.transform.position = PointCenterSpell(cursorPosition, characterPosition, isGamepadUsing);
        }
    }

    private Vector3 PointCenterSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        Vector3 pointCenterSpell = Vector3.zero;

        if (isGamepadUsing)
        {
            if (cursorPosition.x < 0f)
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

            currentGamepadPosition += cursorPosition;

            pointCenterSpell = characterPosition + currentGamepadPosition * Time.deltaTime;
        }
        else
        {
            Debug.Log(cursorPosition);
            pointCenterSpell = cursorPosition;
        }

        return pointCenterSpell;
    }

    public void CastSpellEnd(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        firstFrameToCast = true;

        Debug.Log("CM " + cursorModel.transform.position);
        effectModel.transform.position = cursorModel.transform.position;
        Debug.Log("EM " + effectModel.transform.position);
        cursorModel.transform.position += new Vector3(0f, -20f, 0f);

        //StartCoroutine(EffectCast());
    }

    IEnumerator EffectCast()
    {
        float currentEffectRadius = 0.1f;
        effectModel.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);

        while (currentEffectRadius < effectRadius)
        {
            currentEffectRadius += 15f * Time.deltaTime;
            effectModel.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);
            yield return new WaitForEndOfFrame();
        }

        while (currentEffectRadius > 0.5f)
        {
            currentEffectRadius -= 15f * Time.deltaTime;
            effectModel.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);
            yield return new WaitForEndOfFrame();
        }

        effectModel.transform.position += new Vector3(0f, -20f, 0f);
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
