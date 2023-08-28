using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPS_Spell : SpellUniversal
{
    [SerializeField]
    private float slow = 10f;
    [SerializeField]
    private float numOfHealPerSecond = 1f;
    [SerializeField]
    private float buffDuration = 20f;
    [SerializeField]
    private float reloadTime = 25f;

    private const bool MOMENTARYCAST = true;

    private bool isSpellReady = true;
    private string effectName = "MPS/Circle";
    private GameObject effectModel;
    private Vector3 shieldOffset = new Vector3(0f, 2.9f, 0f);

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
        effectModel = Resources.Load<GameObject>(effectName);
        GameObject characterGirl = GameObject.Find("CharacterGirl");
        Vector3 shieldPosition = characterGirl.transform.position + shieldOffset;
        GameObject buffEffect = Instantiate(effectModel, shieldPosition, Quaternion.identity);

        MPS mps = buffEffect.GetComponent<MPS>();
        mps.SetValues(slow, (float)1f / numOfHealPerSecond);

        float currenrTime = 0f;
        while (currenrTime < buffDuration)
        {
            yield return new WaitForEndOfFrame();
            buffEffect.transform.position = characterGirl.transform.position + shieldOffset;
            currenrTime += Time.deltaTime;
        }

        Destroy(buffEffect);
    }

    IEnumerator Reload()
    {
        isSpellReady = false;
        yield return new WaitForSeconds(reloadTime);
        isSpellReady = true;
    }
}
