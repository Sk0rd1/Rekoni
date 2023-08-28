using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    Transform cursorPosition;
    InputManager inputManager;

    SpellUniversal[] spellPos = new SpellUniversal[8];

    private int spellNumUp = 0;
    private int spellNum = 0;
    private int numOfPrevSpell = 0;
    private bool isCastSpell = false;
    private bool isGamepadUsing = false;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        cursorPosition = GameObject.Find("CursorPosition(Clone)").GetComponent<Transform>();

        for (int i = 0; i < 8; i++)
        {
            spellPos[i] = GameObject.Find("SpellsList").GetComponent<MPS_Spell>();
        }
        spellPos[0] = GameObject.Find("SpellsList").GetComponent<SSI_Spell>();
        spellPos[1] = GameObject.Find("SpellsList").GetComponent<MMM_Spell>();
        spellPos[2] = GameObject.Find("SpellsList").GetComponent<PPI_Spell>();
    }

    public void CheckCast()
    {
        isGamepadUsing = inputManager.IsGamepadUsing();
        if (isGamepadUsing)
        {

        }
        else
        {
            spellNum = inputManager.SpellNum;
            spellNumUp = inputManager.SpellNumUp;
            if (spellNum != 0)
            {
                if (spellPos[spellNum - 1].IsSpellReady() && numOfPrevSpell != spellNum)
                {
                    numOfPrevSpell = spellNum;
                    StartCoroutine(OneOfSpell(spellNum));
                }

                if (!spellPos[spellNum - 1].IsSpellReady())
                {
                    CancelAllSpells();
                }
            }
        }
    }

    private IEnumerator OneOfSpell(int currentSpellNum)
    {
        CancelAllSpells();

        if (spellPos[currentSpellNum - 1].IsMomemtaryCast())
        {
            spellPos[currentSpellNum - 1].SecondStageOfCast(cursorPosition.position, transform.position, isGamepadUsing);
        }
        else
        {
            isCastSpell = true;
            while (spellNumUp != 1 && (spellNum == 0 || currentSpellNum == spellNum))
            {
                isCastSpell = true;
                spellPos[currentSpellNum - 1].FirstStageOfCast(cursorPosition.position, transform.position, isGamepadUsing);
                yield return new WaitForEndOfFrame();
            }
            isCastSpell = false;
            if (spellNum == 0 || currentSpellNum == spellNum)
            {
                spellPos[currentSpellNum - 1].SecondStageOfCast(cursorPosition.position, transform.position, isGamepadUsing);
            }
        }
        numOfPrevSpell = 0;
    }

    private void CancelAllSpells()
    {
        for (int i = 0; i < 8; i++)
        {
            spellPos[i].CancelCast(cursorPosition.position, transform.position, isGamepadUsing);
        }
    }

    public bool IsSpellCast()
    {
        return isCastSpell;
    }

    private void LateUpdate()
    {
        if (isCastSpell)
        {
            transform.LookAt(cursorPosition);
        }
    }
}
