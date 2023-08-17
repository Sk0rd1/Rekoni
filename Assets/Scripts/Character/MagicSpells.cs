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

    private Spell_XXX[] spellPos = new Spell_XXX[8];

    void Start()
    {
        SpellNumCast = 0;
        cursor = GameObject.Find("CursorBase");
        cursor.transform.position = new Vector3(0f, -20f, 0f);
        inputManager = GetComponent<InputManager>();

        //spells
        spellPos[0] = GameObject.Find("SpellsList").GetComponent<Spell_SSI>();
        spellPos[1] = GameObject.Find("SpellsList").GetComponent<Spell_MMM>();
        spellPos[2] = GameObject.Find("SpellsList").GetComponent<Spell_PPI>();
        spellPos[3] = GameObject.Find("SpellsList").GetComponent<Spell_MPS>();
        //end of spells
    }

    public void CheckCast()
    {
        spellNum = inputManager.SpellNum;// 0 - no; 1 - 0,0; 2 - 0,1; 3 - 1,0; 4 - 1,1;
        spellNumUp = inputManager.SpellNumUp;// 0 - no; 1 - 0,0; 2 - 0,1; 3 - 1,0; 4 - 1,1;

        if(spellNum != 0)
        {
            if (spellPos[spellNum - 1].IsSpellReady())
            {
                for (int i = 0; i < 4; i++)
                {
                    spellPos[i].CancelSpell(mousePosition, transform.position, isGamepadUsing);
                }

                Debug.Log("Coroutine");
                StartCoroutine(OneOfSpell(spellNum));
            }
        }
    }

    IEnumerator OneOfSpell(int num)
    {

        while ((spellNum == 0 || spellNum == num) && spellNumUp == 0 && !spellPos[num - 1].MomentaryCast())
        {
            CursorMove();
            spellPos[num - 1].CastSpell(mousePosition, transform.position, isGamepadUsing);
            yield return new WaitForEndOfFrame();
        }

        cursor.transform.position += new Vector3(0f, -100f, 0f);
        if (num == spellNum || spellNum == 0)
            spellPos[num - 1].CastSpellEnd(mousePosition, transform.position, isGamepadUsing);
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
