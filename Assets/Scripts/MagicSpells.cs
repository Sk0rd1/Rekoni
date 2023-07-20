using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpells : MonoBehaviour
{
    private GameObject cursor;
    private GameObject character;
    private bool isCastSpell = false;
    private Vector3 steakDirection = new Vector3(0f, 0f, 1f);

    //spells
    Spell_1 spellL1;
    //end of spells

    void Start()
    {
        cursor = GameObject.Find("CursorBase");
        character = GameObject.Find("CharacterGirl");
        cursor.transform.position = new Vector3(0f, -20f, 0f);

        //spells
        spellL1 = GameObject.Find("SpellsList").GetComponent<Spell_1>();
        //end of spells
    }

    void Update()
    {
        if (isCastSpell)
        {
            if (Input.GetButton("L1"))
            {
                //steakDirection = character.transform.forward;
                CursorMove(steakDirection);
                spellL1.CastSpell(steakDirection);
            }
            else
            if (Input.GetButton("R1"))
            {
                CursorMove(steakDirection);
            }

            if (Input.GetButtonUp("L1"))
            {
                spellL1.CastSpellEnd(steakDirection);
            }
        }
        else
        {
            cursor.transform.position = new Vector3(0f, 100f, 0f);
        }
        Debug.Log(steakDirection);
    }

    private void CursorMove(Vector3 directionAxis)
    {
        directionAxis.y += 0.5f;
        directionAxis *= 3f;

        cursor.transform.position = transform.position + directionAxis;

        Quaternion characterRotation = transform.rotation;
        Vector3 eulerAngles = characterRotation.eulerAngles;

        cursor.transform.rotation = Quaternion.Euler(90f, 0f, -eulerAngles.y);
    }

    public void CastSpell(bool isCastSpell, Vector3 steakDirection)
    {
        if (steakDirection != Vector3.zero)
            this.steakDirection = steakDirection;
        this.isCastSpell = isCastSpell;
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
