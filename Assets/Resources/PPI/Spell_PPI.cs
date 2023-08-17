using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_PPI : Spell_XXX
{
    public const bool MOMENTARYCAST = true;
    [SerializeField]
    private float shieldDuration = 3f;
    [SerializeField]
    private float reloadTime = 4f;
    [SerializeField]
    private int numOfAttack = 4;

    private bool isSpellReady = true;
    private string effectName = "PPI/Shield";
    private GameObject effectModel;
    private Vector3 characterPosition; 
    private Vector3 shieldOffset = new Vector3(0f, 2.9f, 0f);
    private Vector3 shieldPosition;

    public override void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        this.characterPosition = characterPosition;
    }

    public override void CancelSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        //cursorModel.transform.position += new Vector3(0f, -20f, 0f);
    }

    public override void CastSpellEnd(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
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
        while(currenrTime < shieldDuration)
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

    public override bool MomentaryCast()
    {
        return MOMENTARYCAST;
    }

    public override bool IsSpellReady()
    {
        return isSpellReady;
    }
}
