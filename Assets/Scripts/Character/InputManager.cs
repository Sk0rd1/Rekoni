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
    public bool ButtonPause { get; private set; }

    private Camera cameraCharacter;
    private Vector3 leftSteakDirection = Vector3.zero;
    private GameObject cursorPrefabPosition;
    private GameObject cursorPosition;

    private bool is3SpellCast = false;
    private bool is4SpellCast = false;

    private Pause pause;


    private void Awake()
    {
        cursorPrefabPosition = Resources.Load<GameObject>("_OtherObjects/CursorPosition");
        cursorPosition = Instantiate(cursorPrefabPosition);
    }

    private void Start()
    {
        cameraCharacter = Camera.main;
        pause = GameObject.Find("Main Camera").GetComponent<Pause>();
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
        OnPause();
        OnInventory();
        CheckGamepad();
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
        if ((Input.GetKeyDown(KeyCode.JoystickButton2) && isGamepadUsing) || (Input.GetKeyDown(KeyCode.F) && !isGamepadUsing))
        {
            ButtonMoveBox = true;
            SaveManager sm = new SaveManager();
            sm.SaveGame();
        }
        else
        {
            ButtonMoveBox = false;
        }
    }

    private void PressClimbOnBox()
    {
        if ((Input.GetKeyDown(KeyCode.JoystickButton1) && isGamepadUsing) || (Input.GetKeyDown(KeyCode.V) && !isGamepadUsing))
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
            catch { }
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

    public void CheckGamepad()
    {
        // хз чи потрібен наступний код
        /*if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Debug.Log("Mouse");
            isGamepadUsing = false;
        }*/

        int joystickCount = Input.GetJoystickNames().Length;
        for (int i = 1; i <= joystickCount; i++)
        {

            float axisValue = Input.GetAxis("Axis " + i);
            if (axisValue != 0)
            {
                isGamepadUsing = true;
            }
        }

        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
            {
                string name = (kcode.ToString() + "000").Substring(0, 3);
                if(name == "Joy")
                    isGamepadUsing = true;
                else
                    isGamepadUsing = false;
            }
        }
    }

    private void OnPause()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("Main Camera").GetComponent<Pause>().PressExit();
        }
    }

    private void OnInventory()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.I))
        {
            GameObject.Find("Main Camera").GetComponent<Pause>().PressInventory();
        }
    }
}
