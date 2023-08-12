using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_MPS : MonoBehaviour
{
    [SerializeField]
    private float slow = 10f;
    [SerializeField]
    private float numOfHealPerSecond = 1f;
    [SerializeField]
    private float buffDuration = 20f;
    [SerializeField]
    private float reloadTime = 25f;

    public readonly bool MOMENTARYCAST = true;

    private bool isSpellReady = true;
    private string effectName = "MPS/Circle";
    private GameObject effectModel;
    private Vector3 characterPosition;
    private Vector3 shieldOffset = new Vector3(0f, 2.9f, 0f);
    private Vector3 shieldPosition;

    public void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        this.characterPosition = characterPosition;
    }

    public void CancelSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        //cursorModel.transform.position += new Vector3(0f, -20f, 0f);
    }

    public void CastSpellEnd(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        isSpellReady = false;
        StartCoroutine(Reload());
        this.characterPosition = characterPosition;
        StartCoroutine(ShieldMove());
    }

    IEnumerator ShieldMove()
    {
        shieldPosition = characterPosition + shieldOffset;
        effectModel = Resources.Load<GameObject>(effectName);
        GameObject buffEffect = Instantiate(effectModel, shieldPosition, Quaternion.identity);
        GameObject characterGirl = GameObject.Find("CharacterGirl");

        MPS mps = buffEffect.GetComponent<MPS>();
        mps.SetValues(slow, (float)1f/numOfHealPerSecond);

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
        yield return new WaitForSeconds(reloadTime);

        isSpellReady = true;
    }

    public bool IsSpellReady()
    {
        return isSpellReady;
    }
}
