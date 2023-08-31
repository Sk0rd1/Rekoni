using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
    private bool firstCircleOfSpells = true;
    private int numOfCircle = 0;

    private GameObject back1; 
    private GameObject back2; 

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        cursorPosition = GameObject.Find("CursorPosition(Clone)").GetComponent<Transform>();

        back1 = GameObject.Find("Main Camera/Canvas/Back_1");
        back2 = GameObject.Find("Main Camera/Canvas/Back_2");

        for (int i = 0; i < 8; i++)
        {
            spellPos[i] = GameObject.Find("SpellsList").GetComponent<MPS_Spell>();
        }
        spellPos[0] = GameObject.Find("SpellsList").GetComponent<SSI_Spell>();
        spellPos[1] = GameObject.Find("SpellsList").GetComponent<MMM_Spell>();
        spellPos[2] = GameObject.Find("SpellsList").GetComponent<PPI_Spell>();

        spellPos[4] = GameObject.Find("SpellsList").GetComponent<MMM_Spell>();
        spellPos[5] = GameObject.Find("SpellsList").GetComponent<MMM_Spell>();
        spellPos[6] = GameObject.Find("SpellsList").GetComponent<MMM_Spell>();
        spellPos[7] = GameObject.Find("SpellsList").GetComponent<MMM_Spell>();
    }

    public void CheckCast()
    {
        isGamepadUsing = inputManager.IsGamepadUsing();
        spellNum = inputManager.SpellNum;
        spellNumUp = inputManager.SpellNumUp;

        if (inputManager.ButtonNextSpells)
        {
            if(firstCircleOfSpells)
            {
                Debug.Log("true");
                firstCircleOfSpells = false;
                numOfCircle += 4;
                back1.SetActive(false);
                back2.SetActive(true);
            }
            else
            {
                Debug.Log("false");
                firstCircleOfSpells = true;
                numOfCircle -= 4;
                back1.SetActive(true);
                back2.SetActive(false);
            }
        }

        if (spellNum != 0)
            spellNum += numOfCircle;

        if (isGamepadUsing)
        {

        }
        else
        {
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
        ReloadToUI();
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

    private void ReloadToUI()
    {
        for(int i = 0; i < 8; i++)
        {
            int frontNum = i + 1;

            string resultFront = "Main Camera/Canvas/Front_" + frontNum;
            GameObject frontImage = GameObject.Find(resultFront);

            string resultNum = "Main Camera/Canvas/Num_" + frontNum;
            TextMeshProUGUI textNum = GameObject.Find(resultNum).GetComponent<TextMeshProUGUI>();

            if (spellPos[i].IsSpellReady())
            {
                frontImage.SetActive(false);
                textNum.text = " ";

            }
            else
            {
                frontImage.SetActive(true);
                int reload = (int)spellPos[i].TimeReload() + 1;
                textNum.text = reload.ToString();
            }
        }
    }
}
