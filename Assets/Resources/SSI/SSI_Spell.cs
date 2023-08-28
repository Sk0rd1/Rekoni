using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSI_Spell : SpellUniversal
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

    public const bool MOMENTARYCAST = false;

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
        cursorModel.SetActive(false);

        effectPrefabModel = Resources.Load<GameObject>(effectName);
        effectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        ssi = effectModel.GetComponentInChildren<SSI>();
        effectModel.SetActive(false);
    }

    public override bool IsMomemtaryCast()
    {
        return MOMENTARYCAST;
    }

    public override bool IsSpellReady()
    {
        return isSpellReady;
    }

    public override void FirstStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(true);
        cursorModel.transform.position = DistanceWithRadius(mousePosition, characterPosition, isGamepadUsing);
    }

    public override void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        StartCoroutine(Reload());
        effectModel.SetActive(true);

        effectModel.transform.position = cursorModel.transform.position;

        ssi.SetValues(timeCast, damage, forceAttraction);

        cursorModel.transform.position += new Vector3(0f, -20f, 0f);

        StartCoroutine(EffectCast(effectModel));
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(false);
    }

    IEnumerator Reload()
    {
        isSpellReady = false;
        yield return new WaitForSeconds(reloadTime);
        isSpellReady = true;
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
}
