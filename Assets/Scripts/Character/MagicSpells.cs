using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpells : MonoBehaviour
{
    private GameObject cursor;
    private InputManager inputManager;
    private short spellNum = 0;
    private short spellNumUp = 0;
    private bool[] spellNumReady = {false/*L1*/, false/*R1*/, false/*L2*/, false/*R2*/};
    private bool isCastSpell = false;
    private bool isGamepadUsing = false;
    private Vector3 mousePosition = new Vector3(0f, 0f, 1f);
    private Vector3 characterPosition = Vector3.zero;

    //spells
    private Spell_SSI spellPos1;
    private Spell_MMM spellPos2;
    private Spell_IPS spellPos3;
    //private Spell_MMM spellPod4;
    //end of spells

    void Start()
    {
        cursor = GameObject.Find("CursorBase");
        cursor.transform.position = new Vector3(0f, -20f, 0f);
        inputManager = GetComponent<InputManager>();

        //spells
        spellPos1 = GameObject.Find("SpellsList").GetComponent<Spell_SSI>();
        spellPos2 = GameObject.Find("SpellsList").GetComponent<Spell_MMM>();
        spellPos3 = GameObject.Find("SpellsList").GetComponent<Spell_IPS>();

        //end of spells
    }

    void Update()
    {
        spellNum = inputManager.SpellNum();
        spellNumUp = inputManager.SpellNumUp();

        

        if (spellNum == 1)
        {
            CursorMove();
            spellPos1.CastSpell(mousePosition, transform.position, isGamepadUsing);
            spellNumReady[spellNum - 1] = true;
        }
        else if (spellNum == 2 && spellPos2.IsSpellReady())
        {
            CursorMove();
            spellPos2.CastSpell(mousePosition, transform.position, isGamepadUsing);
            spellNumReady[spellNum - 1] = true;
        }
        else if (spellNum == 3 && spellPos3.IsSpellReady())
        {
            CursorMove();
            spellPos3.CastSpell(mousePosition, transform.position, isGamepadUsing);
            spellNumReady[spellNum - 1] = true;
        }
        else
        {
            spellNumReady[0] = true;
            spellNumReady[1] = true;
            spellNumReady[2] = true;
            spellNumReady[3] = true;

            cursor.transform.position = new Vector3(0f, 100f, 0f);
        }

        if (spellNumUp == 1)
        {
            spellPos1.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
        }
        else if (spellNumUp == 2 && spellPos2.IsSpellReady())
        {
            spellPos2.CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
        }
    }

    /*private void CheckSpellNumReady()
    {
        //spellNumReady[0] = spellL1.IsSpellReady();
        spellNumReady[1] = spellL2.IsSpellReady();
        //spellNumReady[2] = spellL3.IsSpellReady();
        //spellNumReady[3] = spellL4.IsSpellReady();
    }*/

    private void CursorMove()
    {
        Vector3 directionAxis = Vector3.forward;
        if (isGamepadUsing)
        {
            //if (mousePosition == Vector3.zero)
            //    mousePosition = transform.forward;
            //mousePosition.Normalize();
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
        this.characterPosition = characterPosition;
    }

    public Vector3 GetCursorPosition()
    {
        return cursor.transform.position;
    }

    public bool[] SpellNumReadyToCast()
    {
        return spellNumReady;
    }

    private void LateUpdate()
    {
        if(isCastSpell)
            transform.LookAt(new Vector3(cursor.transform.position.x, transform.position.y, cursor.transform.position.z));
    }
}
