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
    private float currentReload = 0f;

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

    public override float RadiusCast()
    {
        return spellDistance;
    }

    public override float TimeReload()
    {
        return currentReload;
    }

    public override void FirstStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(true);
        cursorModel.transform.position = mousePosition;
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
        currentReload = reloadTime;
        while (currentReload >= 0f)
        {
            currentReload -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
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
}
