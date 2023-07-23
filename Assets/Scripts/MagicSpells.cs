using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpells : MonoBehaviour
{
    private GameObject cursor;
    private InputManager inputManager;
    private short spellNum = 0;
    private short spellNumUp = 0;
    private bool isCastSpell = false;
    private Vector3 steakDirection = new Vector3(0f, 0f, 1f);

    //spells
    private Spell_1 spellL1;
    private Spell_MMM spellL2;
    //end of spells

    void Start()
    {
        cursor = GameObject.Find("CursorBase");
        //character = GameObject.Find("CharacterGirl");
        cursor.transform.position = new Vector3(0f, -20f, 0f);
        inputManager = GetComponent<InputManager>();

        //spells
        spellL1 = GameObject.Find("SpellsList").GetComponent<Spell_1>();
        spellL2 = GameObject.Find("SpellsList").GetComponent<Spell_MMM>();

        //end of spells
    }

    void Update()
    {
        spellNum = inputManager.SpellNum();
        spellNumUp = inputManager.SpellNumUp();

            switch (spellNum)
            {
                case 1:
                    CursorMove(steakDirection);
                    spellL1.CastSpell(steakDirection);
                break;

                case 2:
                    CursorMove(steakDirection);
                    spellL2.CastSpell(steakDirection);
                break;

                default:
                cursor.transform.position = new Vector3(0f, 100f, 0f);
                break;
            }

            switch (spellNumUp)
            {
                case 1:
                    spellL1.CastSpellEnd(steakDirection);
                    break;

                case 2:
                    spellL2.CastSpellEnd(steakDirection);
                    break;

                default:

                    break;
            }
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
