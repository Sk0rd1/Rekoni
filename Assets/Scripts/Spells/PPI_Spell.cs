using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPI_Spell : SpellUniversal
{
    public const bool MOMENTARYCAST = true;
    private float shieldDuration = 7f;
    private float reloadTime = 4f;
    private int numOfAttack = 4;

    private bool isSpellReady = true;
    private string effectName = "PPI/Shield";
    private GameObject effectModel;
    private Vector3 shieldOffset = new Vector3(0f, 2.9f, 0f);
    private float currentReload = 0f;

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

    }

    public override void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        StartCoroutine(Reload());
        StartCoroutine(ShieldMove());
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {

    }

    IEnumerator ShieldMove()
    {
        GameObject characterGirl = GameObject.Find("CharacterGirl");
        Vector3 shieldPosition = characterGirl.transform.position + shieldOffset;
        effectModel = Resources.Load<GameObject>(effectName);
        GameObject shieldEffect = Instantiate(effectModel, shieldPosition, Quaternion.identity);
        characterGirl.GetComponent<Health>().AddShield(numOfAttack, shieldDuration, shieldEffect);

        float currenrTime = 0f;
        while (currenrTime < shieldDuration)
        {
            yield return new WaitForEndOfFrame();
            shieldEffect.transform.position = characterGirl.transform.position + shieldOffset;
            currenrTime += Time.deltaTime;
        }

        Destroy(shieldEffect);
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
}
