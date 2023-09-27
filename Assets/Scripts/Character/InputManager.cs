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

    private bool is3SpellCast = false;
    private bool is4SpellCast = false;


    private void Awake()
    {
        cursorPrefabPosition = Resources.Load<GameObject>("_OtherObjects/CursorPosition");
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
        ButtonNextSpells = false;
    }

    public void CheckButton()
    {
        /*if (Input.GetKeyDown(KeyCode.JoystickButton1))
            Debug.Log(1);
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
            Debug.Log(2);
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
            Debug.Log(3);
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
            Debug.Log(4);
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
            Debug.Log(5);
        if (Input.GetKeyDown(KeyCode.JoystickButton6))
            Debug.Log(6);
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
            Debug.Log(7);
        if (Input.GetKeyDown(KeyCode.JoystickButton8))
            Debug.Log(8);
        if (Input.GetKeyDown(KeyCode.JoystickButton9))
            Debug.Log(9);
        if (Input.GetKeyDown(KeyCode.JoystickButton10))
            Debug.Log(10);
        if (Input.GetKeyDown(KeyCode.JoystickButton11))
            Debug.Log(11);
        if (Input.GetKeyDown(KeyCode.JoystickButton12))
            Debug.Log(12);
        if (Input.GetKeyDown(KeyCode.JoystickButton13))
            Debug.Log(13);
        if (Input.GetKeyDown(KeyCode.JoystickButton14))
            Debug.Log(14);
        if (Input.GetKeyDown(KeyCode.JoystickButton15))
            Debug.Log(15);
        if (Input.GetKeyDown(KeyCode.JoystickButton16))
            Debug.Log(16);
        if (Input.GetKeyDown(KeyCode.JoystickButton17))
            Debug.Log(17);
        if (Input.GetKeyDown(KeyCode.JoystickButton18))
            Debug.Log(18);
        if (Input.GetKeyDown(KeyCode.JoystickButton19))
            Debug.Log(19);

        if (Input.GetKeyUp(KeyCode.JoystickButton1))
            Debug.Log(100);
        if (Input.GetKeyUp(KeyCode.JoystickButton2))
            Debug.Log(200);
        if (Input.GetKeyUp(KeyCode.JoystickButton3))
            Debug.Log(300);
        if (Input.GetKeyUp(KeyCode.JoystickButton4))
            Debug.Log(400);
        if (Input.GetKeyUp(KeyCode.JoystickButton5))
            Debug.Log(500);
        if (Input.GetKeyUp(KeyCode.JoystickButton6))
            Debug.Log(600);
        if (Input.GetKeyUp(KeyCode.JoystickButton7))
            Debug.Log(700);
        if (Input.GetKeyUp(KeyCode.JoystickButton8))
            Debug.Log(800);
        if (Input.GetKeyUp(KeyCode.JoystickButton9))
            Debug.Log(900);
        if (Input.GetKeyUp(KeyCode.JoystickButton10))
            Debug.Log(1000);
        if (Input.GetKeyUp(KeyCode.JoystickButton11))
            Debug.Log(1100);
        if (Input.GetKeyUp(KeyCode.JoystickButton12))
            Debug.Log(1200);
        if (Input.GetKeyUp(KeyCode.JoystickButton13))
            Debug.Log(1300);
        if (Input.GetKeyUp(KeyCode.JoystickButton14))
            Debug.Log(1400);
        if (Input.GetKeyUp(KeyCode.JoystickButton15))
            Debug.Log(15);
        if (Input.GetKeyUp(KeyCode.JoystickButton16))
            Debug.Log(1600);
        if (Input.GetKeyUp(KeyCode.JoystickButton17))
            Debug.Log(1700);
        if (Input.GetKeyUp(KeyCode.JoystickButton18))
            Debug.Log(1800);
        if (Input.GetKeyUp(KeyCode.JoystickButton19))
            Debug.Log(1900);

        if (Input.GetAxis("Axis 1") != 0)
            Debug.Log("A1");
        if (Input.GetAxis("Axis 2") != 0)
            Debug.Log("A2");
        if (Input.GetAxis("Axis 3") != 0)
            Debug.Log("A3");
        if (Input.GetAxis("Axis 4") != 0)
            Debug.Log("A4");
        if (Input.GetAxis("Axis 5") != 0)
            Debug.Log("A5");
        if (Input.GetAxis("Axis 6") != 0)
            Debug.Log("A6");
        if (Input.GetAxis("Axis 7") != 0)
            Debug.Log("A7");
        if (Input.GetAxis("Axis 8") != 0)
            Debug.Log("A8");*/

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
        if (isGamepadUsing)
        {
            float value = Input.GetAxis("Axis 6");
            if (Mathf.Abs(value) > 0.1f)
            {
                if (value > 0)
                {
                    ButtonNextSpells = true;
                }
                else
                {
                    ButtonNextSpells = false;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ButtonNextSpells = !ButtonNextSpells;
            }
        }
    }

    private void CancelSpell()
    {
        if (isGamepadUsing)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton9))
            {
                IsCancelSpell = true;
            }
            else
            {
                IsCancelSpell = false;
            }
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
            if (Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                SpellNum = 1;
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                SpellNum = 2;
            }
            else if(Input.GetAxis("Axis 9") > 0.5f && !is3SpellCast)
            {
                is3SpellCast = true;
                SpellNum = 3;
            }
            else if (Input.GetAxis("Axis 10") > 0.5f && !is4SpellCast)
            {
                is4SpellCast = true;
                SpellNum = 4;
            }
            else
            {
                SpellNum = 0;
            }

            if (Input.GetAxis("Axis 9") < 0.5f)
                is3SpellCast = false;
            if (Input.GetAxis("Axis 10") < 0.5f)
                is4SpellCast = false;
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
            if (Input.GetKeyUp(KeyCode.JoystickButton4))
            {
                SpellNumUp = 1;
            }
            else if (Input.GetKeyUp(KeyCode.JoystickButton5))
            {
                SpellNumUp = 2;
            }
            else if (Input.GetAxis("Axis 9") < 0.5f && is3SpellCast)
            {
                SpellNumUp = 3;
            }
            else if (Input.GetAxis("Axis 10") < 0.5f && is4SpellCast)
            {
                SpellNumUp = 4;
            }
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
