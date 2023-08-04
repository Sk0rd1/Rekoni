using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_IPS : MonoBehaviour
{
    private float reloadTime = 0f;
    private float effectSpeed = 10f;
    private bool second = true;

    private Vector3 centerSpell = Vector3.zero;
    private Vector3 effectDirection = Vector3.zero;

    private GameObject effectPrefabModel;
    private GameObject effectModel;
    private string effectName = "IPS/ES_IPS";
    private bool isSpellReady = true;

    private void Start()
    {
        effectPrefabModel = Resources.Load<GameObject>(effectName);
        effectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
    }

    public void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {

    }

    public void CastSpellEnd(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        StartCoroutine(Reload());

        //firstFrameToCast = true;

        centerSpell = cursorPosition;
        effectDirection = characterPosition - cursorPosition;
        effectDirection.Normalize();

        //cursorModel.transform.position += new Vector3(0f, -20f, 0f);

        StartCoroutine(EffectCast());
    }

    IEnumerator EffectCast()
    {
        second = true;
        StartCoroutine(Second());
        while (second)
        {
            effectModel.transform.position += effectDirection * effectSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Second()
    {
        yield return new WaitForSeconds(1);
        second = false;
    }

        IEnumerator Reload()
    {
        float currentTime = 0f;
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
