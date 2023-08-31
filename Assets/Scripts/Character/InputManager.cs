using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private bool isGamepadUsing = true;

    public int SpellNum { get; private set; }
    public int SpellNumUp { get; private set; }
    public bool IsCancelSpell { get; private set; }
    public bool ButtonRoll { get; private set; }
    public bool ButtonMoveTime { get; private set; }
    public bool ButtonClimbOnBox { get; private set; }
    public bool ButtonMoveBox { get; private set; }
    public bool ButtonNextSpells { get; private set; }

    private Camera cameraCharacter;
    private Vector3 leftSteakDirection = Vector3.zero;
    private GameObject cursorPrefabPosition;
    private GameObject cursorPosition;


    private void Awake()
    {
        cursorPrefabPosition = Resources.Load<GameObject>("OtherObjects/CursorPosition");
        cursorPosition = Instantiate(cursorPrefabPosition);
    }

    private void Start()
    {
        // я хз чи присвоїлась камера
        cameraCharacter = Camera.main;

        SpellNum = 0;
        SpellNumUp = 0;
        IsCancelSpell = false;
        ButtonRoll = false;
        ButtonMoveTime = false;
        ButtonMoveBox = false;
        ButtonClimbOnBox = false;
    }

    public void CheckButton()
    {
        PressRoll();
        PressMoveTime();
        PressMoveBox();
        PressClimbOnBox();
        PressSpellUp();
        PressSpell();
        CountRightSteak();
        CountLeftSteak();
        CancelSpell();
        NextCircleOfSpell();
    }

    private void PressRoll()
    {
        if((Input.GetKey(KeyCode.JoystickButton0) && isGamepadUsing) || (Input.GetKey(KeyCode.Space) && !isGamepadUsing))
        {
            ButtonRoll = true;
        }
        else
        {
            ButtonRoll = false;
        }
    }

    private void PressMoveTime()
    {
        if ((Input.GetKey(KeyCode.JoystickButton3) && isGamepadUsing) || (Input.GetKey(KeyCode.G) && !isGamepadUsing))
        {
            ButtonMoveTime = true;
        }
        else
        {
            ButtonMoveTime = false;
        }
    }

    private void PressMoveBox()
    {
        if ((Input.GetKey(KeyCode.JoystickButton2) && isGamepadUsing) || (Input.GetKey(KeyCode.F) && !isGamepadUsing))
        {
            ButtonMoveBox = true;
        }
        else
        {
            ButtonMoveBox = false;
        }
    }

    private void PressClimbOnBox()
    {
        if ((Input.GetKey(KeyCode.JoystickButton1) && isGamepadUsing) || (Input.GetKey(KeyCode.V) && !isGamepadUsing))
        {
            ButtonClimbOnBox = true;
        }
        else
        {
            ButtonClimbOnBox = false;
        }
    }

    private void NextCircleOfSpell()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ButtonNextSpells = true;
        }
        else
        {
            ButtonNextSpells = false;
        }
    }

    private void CancelSpell()
    {
        if (isGamepadUsing)
        {
            // при натиску на правий стік, спел відміняється
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                IsCancelSpell = true;
            }
            else
            {
                IsCancelSpell = false;
            }
        }
    }

    private void PressSpell()
    {
        if (isGamepadUsing)
        {
            if (Input.GetButton("L1"))
            {
                SpellNum = 1;
            }
            else if (Input.GetButton("R1"))
            {
                SpellNum = 2;
            }
            /*else if (Input.GetButton("L2"))
            {
                spellNum[0] = false;
                spellNum[1] = false;
                spellNum[2] = true;
                spellNum[3] = false;
            }
            else if (Input.GetButton("R2"))
            {
                spellNum[0] = false;
                spellNum[1] = false;
                spellNum[2] = false;
                spellNum[3] = true;
            }*/
            else
            {
                SpellNum = 0;
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SpellNum = 1;
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SpellNum = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SpellNum = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SpellNum = 4;
            }
            else
            {
                SpellNum = 0;
            }
        }
    }

    private void PressSpellUp()
    {
        if (isGamepadUsing)
        {
            if (Input.GetButtonUp("L1"))
            {
                SpellNumUp = 1;
            }
            else if (Input.GetButtonUp("R1"))
            {
                SpellNumUp = 2;
            }
            /*else if (Input.GetButtonUp("L2"))
            {
                spellNumUp[0] = false;
                spellNumUp[1] = false;
                spellNumUp[2] = true;
                spellNumUp[3] = false;
            }
            else if (Input.GetButtonUp("R2"))
            {
                spellNumUp[0] = false;
                spellNumUp[1] = false;
                spellNumUp[2] = false;
                spellNumUp[3] = true;
            }*/
            else
            {
                SpellNumUp = 0;
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                SpellNumUp = 1;
            }
            else
            {
                SpellNumUp = 0;
            }
        }
    }

    private void CountRightSteak()
    {
        if (isGamepadUsing)
        {
            float horizontalDirection = 0f;
            float verticalDirection = 0f;

            horizontalDirection = Input.GetAxis("Axis 4");
            verticalDirection = -Input.GetAxis("Axis 5");

            Vector3 steakDirection = new Vector3(horizontalDirection, 0f, verticalDirection);
            cursorPosition.transform.position = steakDirection;
        }
        else
        {
            RaycastHit hit;
            Ray ray = cameraCharacter.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            Vector3 newHitPoint = Vector3.zero;

            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Floor")))
            {
                newHitPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }

            try 
            {
                cursorPosition.transform.position = newHitPoint;
            }
            catch
            {
                Debug.Log("GG");
            }
            //rightSteakDirection = newHitPoint;
            //rSvalue = newHitPoint;

            //newHitPoint = newHitPoint - transform.position;
            //newHitPoint.Normalize();
            //rightSteakDirection = newHitPoint;
        }
    }

    private void CountLeftSteak()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (isGamepadUsing)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
                verticalInput = 1;

            if (Input.GetKey(KeyCode.S))
                verticalInput = -1;

            if (Input.GetKey(KeyCode.A))
                horizontalInput = -1;

            if (Input.GetKey(KeyCode.D))
                horizontalInput = 1;
        }

        leftSteakDirection = new Vector3(horizontalInput, 0f, verticalInput);
        leftSteakDirection.Normalize();
    }

    /*public Vector3 RightSteakDirection()
    {
        return rightSteakDirection;
    }*/

    /*public Vector3 MousePosition()
    {
        return rSvalue;
    }*/

    public bool IsGamepadUsing()
    {
        return isGamepadUsing;
    }

    public Vector3 LeftSteakDirection()
    {
        return leftSteakDirection;
    }

    public void IsGamepadUsing(bool isGamepadUsing)
    {
        this.isGamepadUsing = isGamepadUsing;
    }
}
