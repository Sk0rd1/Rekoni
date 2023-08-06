using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_PPI : MonoBehaviour
{
    public readonly bool MOMENTARYCAST = true;

    private float timeCastShield = 3f;
    private float reloadTime = 4f;

    private bool isSpellReady = true;
    private string effectName = "PPI/Shield";
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
        GameObject shieldEffect = Instantiate(effectModel, shieldPosition, Quaternion.identity);
        GameObject characterGirl = GameObject.Find("CharacterGirl");

        float currenrTime = 0f;
        while(currenrTime < timeCastShield)
        {
            yield return new WaitForEndOfFrame();
            shieldEffect.transform.position = characterGirl.transform.position + shieldOffset;
            currenrTime += Time.deltaTime;
        }

        Destroy(shieldEffect);
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
