using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_1 : MonoBehaviour
{
    [SerializeField]
    private float offsetCursorPosition = 1f;
    [SerializeField]
    private float timeCast = 2f;
    [SerializeField]
    private float cursorSpeed = 5f;


    private GameObject character;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorModel;
    private GameObject effectModel;

    private string cursorName = "CursorTarget";
    private string effectName = "EffectSpell_1";

    private bool isSpellCast = false;
    private float currentTime = 0f;
    private bool firstFrameToCast = true;
    private float effectRadius = 7f;

    

    void Start()
    {
        character = GameObject.Find("CharacterGirl");

        cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);

        effectPrefabModel = Resources.Load<GameObject>(effectName);
        effectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);

        cursorModel.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
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

        effectModel.transform.position = cursorModel.transform.position;
        cursorModel.transform.position += new Vector3(0f, -20f, 0f);

        StartCoroutine(EffectCast());
    }

    IEnumerator EffectCast()
    {
        float currentEffectRadius = 0.1f;
        effectModel.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);

        while(currentEffectRadius < effectRadius)
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
