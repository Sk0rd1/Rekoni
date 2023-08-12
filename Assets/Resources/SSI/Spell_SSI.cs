using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_SSI : MonoBehaviour
{
    [SerializeField]
    private float reloadTime = 4f;
    [SerializeField]
    private float timeCast = 3f;
    [SerializeField]
    private float timePeriod = 1f;
    [SerializeField]
    private int damage = 4;
    [SerializeField]
    private float forceAttraction = 0.1f;

    public readonly bool MOMENTARYCAST = false;

    SSI ssi;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorModel;
    private GameObject effectModel;

    private string cursorName = "SSI/Cursor";
    private string effectName = "SSI/BlackHole";

    private float effectRadius = 3f;
    private bool isSpellReady = true;
    private float spellDistance = 20f;

    private Vector3 currentGamepadPosition = Vector3.zero;

    void Start()
    {

        cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);

        effectPrefabModel = Resources.Load<GameObject>(effectName);
        effectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        ssi = effectModel.GetComponentInChildren<SSI>();
    }

    public void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.transform.position = DistanceWithRadius(cursorPosition, characterPosition, isGamepadUsing);
    }

    private Vector3 DistanceWithRadius(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        Vector3 pointCenterSpell = PointCenterSpell(cursorPosition, characterPosition, isGamepadUsing) - characterPosition;

        if (isGamepadUsing)
        {
            if (Mathf.Sqrt(pointCenterSpell.x * pointCenterSpell.x + pointCenterSpell.z * pointCenterSpell.z) < spellDistance)
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

        effectModel.transform.position = cursorModel.transform.position;
        
        ssi.SetValues(timeCast, damage, forceAttraction);

        cursorModel.transform.position += new Vector3(0f, -20f, 0f);

        StartCoroutine(EffectCast(effectModel));
    }

    IEnumerator EffectCast(GameObject effect)
    {
        float currentEffectRadius = 0.1f;
        effect.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);

        ssi.IsCastBH = true;

        while (currentEffectRadius < effectRadius)
        {
            currentEffectRadius += 5f * Time.deltaTime;
            effect.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(timeCast);

        while (currentEffectRadius > 0.2f)
        {
            currentEffectRadius -= 10f * Time.deltaTime;
            effect.transform.localScale = new Vector3(currentEffectRadius, currentEffectRadius, currentEffectRadius);
            yield return new WaitForEndOfFrame();
        }

        ssi.IsCastBH = false;

        effectModel.transform.position += new Vector3(0f, -20f, 0f);
    }

    public bool IsSpellReady()
    {
        return isSpellReady;
    }

    IEnumerator Reload()
    {
        isSpellReady = false;
        yield return new WaitForSeconds(reloadTime);
        isSpellReady = true;
    }
}