using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellManager : MonoBehaviour
{
    [SerializeField]
    float cursorGamepadSpeed = 5.0f;

    Transform cursorPosition;
    InputManager inputManager;

    SpellUniversal[] spellPos = new SpellUniversal[8];

    private int spellNumUp = 0;
    private int spellNum = 0;
    private int numOfPrevSpell = 0;
    private bool isCastSpell = false;
    private bool secondCircleOfSpells = false;
    private bool isGamepadUsing = false;
    private bool isCanselSpell = false;
    private int numOfCircle = 0;

    TextMeshProUGUI[] textNum = new TextMeshProUGUI[8];
    GameObject[] frontImage = new GameObject[8];
    private GameObject back1; 
    private GameObject back2;

    private Vector3 resultPosition = Vector3.zero;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        cursorPosition = GameObject.Find("CursorPosition(Clone)").GetComponent<Transform>();

        back1 = GameObject.Find("Main Camera/Canvas/Back_1");
        back2 = GameObject.Find("Main Camera/Canvas/Back_2");

        spellPos[0] = GameObject.Find("SpellsList").GetComponent<SSI_Spell>();
        spellPos[1] = GameObject.Find("SpellsList").GetComponent<MMM_Spell>();
        spellPos[2] = GameObject.Find("SpellsList").GetComponent<PPI_Spell>();
        spellPos[3] = GameObject.Find("SpellsList").GetComponent<PSM_Spell>();
        spellPos[4] = GameObject.Find("SpellsList").GetComponent<III_Spell>();
        spellPos[5] = GameObject.Find("SpellsList").GetComponent<MSI_Spell>();
        spellPos[6] = GameObject.Find("SpellsList").GetComponent<UUU_Spell>();
        spellPos[7] = GameObject.Find("SpellsList").GetComponent<UUS_Spell>();

        for (int i = 0; i < 8; i++)
        {
            int frontNum = i + 1;
            string resultFront = "Main Camera/Canvas/Front_" + frontNum;
            frontImage[i] = GameObject.Find(resultFront);
            string resultNum = "Main Camera/Canvas/Num_" + frontNum;
            textNum[i] = GameObject.Find(resultNum).GetComponent<TextMeshProUGUI>();
        }
    }

    public void CheckCast()
    {
        isGamepadUsing = inputManager.IsGamepadUsing();
        spellNum = inputManager.SpellNum;
        spellNumUp = inputManager.SpellNumUp;

        if (inputManager.IsCancelSpell)
        {
            isCanselSpell = true;
            CancelAllSpells();
            return;
        }
        else
        {
            isCanselSpell = false;
        }

        secondCircleOfSpells = inputManager.ButtonNextSpells;

        if(secondCircleOfSpells)
        {
            numOfCircle = 4;
            back1.SetActive(false);
            back2.SetActive(true);
        }
        else
        {
            numOfCircle = 0;
            back2.SetActive(false);
            back1.SetActive(true);
        }

        if (spellNum != 0)
            spellNum += numOfCircle;

        if (isGamepadUsing)
        {
            if(spellNumUp != 0)
                spellNumUp += numOfCircle;

            if (spellNum != 0)
            {
                if (spellPos[spellNum - 1].IsSpellReady() && numOfPrevSpell != spellNum)
                {
                    numOfPrevSpell = spellNum;
                    StartCoroutine(OneOfSpellGamepad(spellNum));
                }

                if (!spellPos[spellNum - 1].IsSpellReady())
                {
                    CancelAllSpells();
                }
            }
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
            spellPos[currentSpellNum - 1].SecondStageOfCast(CursorPosition(cursorPosition.position, spellPos[currentSpellNum - 1].RadiusCast()), transform.position, isGamepadUsing);
        }
        else
        {
            isCastSpell = true;
            while (spellNumUp != 1 && (spellNum == 0 || currentSpellNum == spellNum) && !isCanselSpell)
            {
                isCastSpell = true;
                spellPos[currentSpellNum - 1].FirstStageOfCast(CursorPosition(cursorPosition.position, spellPos[currentSpellNum - 1].RadiusCast()), transform.position, isGamepadUsing);
                yield return new WaitForEndOfFrame();
            }
            isCastSpell = false;
            if ((spellNum == 0 || currentSpellNum == spellNum) && !isCanselSpell)
            {
                spellPos[currentSpellNum - 1].SecondStageOfCast(CursorPosition(cursorPosition.position, spellPos[currentSpellNum - 1].RadiusCast()), transform.position, isGamepadUsing);
            }
        }
        numOfPrevSpell = 0;
    }

    private Vector3 CursorPosition(Vector3 position, float radius)
    {
        Vector3 radiusPosition = position - transform.position;

        if(radiusPosition.x * radiusPosition.x + radiusPosition.z * radiusPosition.z < radius * radius)
        {
            return position;
        }
        else
        {
            Vector3 newPosition = new Vector3(radiusPosition.x, 0f, radiusPosition.z);
            newPosition.Normalize();
            newPosition *= radius;
            newPosition.y = position.y;
            
            return newPosition + new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }

    private IEnumerator OneOfSpellGamepad(int currentSpellNum)
    {
        CancelAllSpells();

        if (spellPos[currentSpellNum - 1].IsMomemtaryCast())
        {
            spellPos[currentSpellNum - 1].SecondStageOfCast(cursorPosition.position, transform.position, isGamepadUsing);
        }
        else
        {
            isCastSpell = true;
            Vector3 gamepadPosition = new Vector3();
            resultPosition = transform.forward.normalized * 5f;
            while ((spellNum == 0 || currentSpellNum == spellNum) && !isCanselSpell)
            {
                if (currentSpellNum == spellNumUp)
                    break;
                isCastSpell = true;
                gamepadPosition = GamepadPosition(spellPos[currentSpellNum - 1].RadiusCast());
                spellPos[currentSpellNum - 1].FirstStageOfCast(gamepadPosition, transform.position, isGamepadUsing);
                yield return new WaitForEndOfFrame();
            }
            resultPosition = Vector3.zero;
            isCastSpell = false;
            if ((spellNum == 0 || currentSpellNum == spellNum) && !isCanselSpell && spellNumUp == currentSpellNum)
            {
                spellPos[currentSpellNum - 1].SecondStageOfCast(gamepadPosition, transform.position, isGamepadUsing);
            }
        }
        numOfPrevSpell = 0;
    }

    private Vector3 GamepadPosition(float radiusCast)
    {
        resultPosition += cursorPosition.position * Time.deltaTime * cursorGamepadSpeed;
        resultPosition.y = 0;

        if(Mathf.Sqrt(resultPosition.x * resultPosition.x + resultPosition.z * resultPosition.z) > radiusCast)
        {
            resultPosition = resultPosition.normalized * radiusCast;
        }

        return (resultPosition + transform.position);
    }

    private void CancelAllSpells()
    {
        resultPosition = Vector3.zero;
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
            if (isGamepadUsing)
            {
                transform.LookAt(transform.position + resultPosition);
            }
            else
            {
                transform.LookAt(cursorPosition);
            }
        }
    }

    private void ReloadToUI()
    {
        for(int i = 0; i < 8; i++)
        {
            if (spellPos[i].IsSpellReady())
            {
                frontImage[i].SetActive(false);
                textNum[i].text = " ";
            }
            else
            {
                frontImage[i].SetActive(true);
                int reload = (int)spellPos[i].TimeReload() + 1;
                textNum[i].text = reload.ToString();
            }
        }
    }
}
