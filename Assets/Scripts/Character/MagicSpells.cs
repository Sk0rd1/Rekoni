using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpells : MonoBehaviour
{
    private GameObject cursor;
    private InputManager inputManager;
    private int spellNum = 0;
    private int spellNumUp = 0;
    private bool isCastSpell = false;
    private bool isGamepadUsing = false;
    private Vector3 mousePosition = new Vector3(0f, 0f, 1f);
    private Vector3 characterPosition = Vector3.zero;

    public int SpellNumCast { get; private set; }

    //spells
    private Spell_SSI spellPos1;
    private Spell_MMM spellPos2;
    private Spell_PPI spellPos3;
    private Spell_MPS spellPos4;
    //end of spells

    void Start()
    {
        SpellNumCast = 0;
        cursor = GameObject.Find("CursorBase");
        cursor.transform.position = new Vector3(0f, -20f, 0f);
        inputManager = GetComponent<InputManager>();

        //spells
        spellPos1 = GameObject.Find("SpellsList").GetComponent<Spell_SSI>();
        spellPos2 = GameObject.Find("SpellsList").GetComponent<Spell_MMM>();
        spellPos3 = GameObject.Find("SpellsList").GetComponent<Spell_PPI>();
        spellPos4 = GameObject.Find("SpellsList").GetComponent<Spell_MPS>();
        //end of spells
    }

    public void CheckCast()
    {
        if (inputManager.SpellNum != 0)
        {
            spellNum = inputManager.SpellNum;
        }

        spellNumUp = inputManager.SpellNumUp;

        if (spellNum == 1 && spellPos1.IsSpellReady())
        {
            if (spellPos1.MOMENTARYCAST)
            {
                spellPos1.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
                SpellNumCast = 0;
            }
            else
            {
                CursorMove();
                spellPos1.CastSpell(mousePosition, transform.position, isGamepadUsing);
                SpellNumCast = 1;
                isCastSpell = true;
            }
        }
        else if (spellNum == 2 && spellPos2.IsSpellReady())
        {
            if (spellPos2.MOMENTARYCAST)
            {
                spellPos2.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
                SpellNumCast = 0;
            }
            else
            {
                CursorMove();
                spellPos2.CastSpell(mousePosition, transform.position, isGamepadUsing);
                SpellNumCast = 2;
                isCastSpell = true;
            }
        }
        else if (spellNum == 3 && spellPos3.IsSpellReady())
        {
            if (spellPos3.MOMENTARYCAST)
            {
                spellPos3.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
                SpellNumCast = 0;
            }
            else
            {
                CursorMove();
                spellPos3.CastSpell(mousePosition, transform.position, isGamepadUsing);
                SpellNumCast = 3;
                isCastSpell = true;
            }
        }
        else if (spellNum == 4 && spellPos4.IsSpellReady())
        {
            if (spellPos4.MOMENTARYCAST)
            {
                spellPos4.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
                SpellNumCast = 0;
            }
            else
            {
                CursorMove();
                spellPos4.CastSpell(mousePosition, transform.position, isGamepadUsing);
                SpellNumCast = 4;
                isCastSpell = true;
            }
        }
        else
        {
            SpellNumCast = 0;
            cursor.transform.position = new Vector3(0f, -100f, 0f);
        }

        if (spellNumUp != 0 && spellNum == 1)
        {
            spellNum = 0;
            spellPos1.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
            isCastSpell = false;
        }
        else if (spellNumUp != 0 && spellNum == 2)
        {
            spellNum = 0;
            spellPos2.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
            isCastSpell = false;
        }
        else if (spellNumUp != 0 && spellNum == 3)
        {
            spellNum = 0;
            spellPos3.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
            isCastSpell = false;
        }
        else if (spellNumUp != 0 && spellNum == 4)
        {
            spellNum = 0;
            spellPos4.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
            isCastSpell = false;
        }

        if (spellNum == 1 && !spellPos1.IsSpellReady())
        {
            spellNum = 0;
        } 
        else if (spellNum == 2 && !spellPos2.IsSpellReady())
        {
            spellNum = 0;
        }
        else if (spellNum == 3 && !spellPos3.IsSpellReady())
        {
            spellNum = 0;
        }
        else if (spellNum == 4 && !spellPos4.IsSpellReady())
        {
            spellNum = 0;
        }

        if (inputManager.IsCancelSpell)
        {
            if (spellNum == 1 && !spellPos1.MOMENTARYCAST)
            {
                spellPos1.CancelSpell(mousePosition, transform.position, isGamepadUsing);
            }
            if (spellNum == 2 && !spellPos2.MOMENTARYCAST)
            {
                spellPos2.CancelSpell(mousePosition, transform.position, isGamepadUsing);
            }
            if (spellNum == 3 && !spellPos3.MOMENTARYCAST)
            {
                spellPos3.CancelSpell(mousePosition, transform.position, isGamepadUsing);
            }
            if (spellNum == 4 && !spellPos4.MOMENTARYCAST)
            {
                spellPos4.CancelSpell(mousePosition, transform.position, isGamepadUsing);
            }

            spellNum = 0;
        }
    }

    private void CursorMove()
    {
        Vector3 directionAxis = Vector3.forward;
        if (isGamepadUsing)
        {
            directionAxis = mousePosition;
            directionAxis.Normalize();
        }
        else
        {
            directionAxis = mousePosition - transform.position;
            directionAxis.Normalize();
        }
        directionAxis.y += 0.5f;
        directionAxis *= 3f;

        cursor.transform.position = transform.position + directionAxis;

        Quaternion characterRotation = transform.rotation;
        Vector3 eulerAngles = characterRotation.eulerAngles;

        cursor.transform.rotation = Quaternion.Euler(90f, 0f, -eulerAngles.y);
    }

    public void CastSpell(bool isCastSpell, Vector3 mousePosition, bool isGamepadUsing)
    {
        this.mousePosition = mousePosition;
        this.isCastSpell = isCastSpell;
        this.isGamepadUsing = isGamepadUsing;
    }

    public Vector3 GetCursorPosition()
    {
        return cursor.transform.position;
    }

    private void LateUpdate()
    {
        if(isCastSpell)
            transform.LookAt(new Vector3(cursor.transform.position.x, transform.position.y, cursor.transform.position.z));
    }
}
