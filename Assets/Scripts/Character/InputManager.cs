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

    private short spellNum = 0;
    private short spellNumUp = 0;

    private bool buttonRoll = false;
    private bool buttonMoveTime = false;
    private bool buttonMoveBox = false;
    private bool buttonClimbOnBox = false;

    private Vector3 rSvalue = Vector3.zero;


    private Camera cameraCharacter;
    private Vector3 rightSteakDirection = Vector3.zero;
    private Vector3 leftSteakDirection = Vector3.zero;


    private void Start()
    {
        // я хз чи присвоїлась камера
        cameraCharacter = Camera.main;
    }

    private void Update()
    {
        PressRoll();
        PressMoveTime();
        PressMoveBox();
        PressClimbOnBox();
        PressSpellUp();
        PressSpell();
        CountRightSteak();
        CountLeftSteak();
    }

    private void PressRoll()
    {
        if((Input.GetKey(KeyCode.JoystickButton0) && isGamepadUsing) || (Input.GetKey(KeyCode.Space) && !isGamepadUsing))
        {
            buttonRoll = true;
        }
        else
        {
            buttonRoll = false;
        }
    }

    private void PressMoveTime()
    {
        if ((Input.GetKey(KeyCode.JoystickButton3) && isGamepadUsing) || (Input.GetKey(KeyCode.G) && !isGamepadUsing))
        {
            buttonMoveTime = true;
        }
        else
        {
            buttonMoveTime = false;
        }
    }

    private void PressMoveBox()
    {
        if ((Input.GetKey(KeyCode.JoystickButton2) && isGamepadUsing) || (Input.GetKey(KeyCode.F) && !isGamepadUsing))
        {
            buttonMoveBox = true;
        }
        else
        {
            buttonMoveBox = false;
        }
    }

    private void PressClimbOnBox()
    {
        if ((Input.GetKey(KeyCode.JoystickButton1) && isGamepadUsing) || (Input.GetKey(KeyCode.V) && !isGamepadUsing))
        {
            buttonClimbOnBox = true;
        }
        else
        {
            buttonClimbOnBox = false;
        }
    }

    private void PressSpell()
    {
        if (isGamepadUsing)
        {
            if (Input.GetButton("L1"))
            {
                spellNum = 1;
            }
            else if (Input.GetButton("R1"))
            {
                spellNum = 2;
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
                spellNum = 0;
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                spellNum = 1;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                spellNum = 2;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                spellNum = 3;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                spellNum = 4;
            }
        }
    }

    private void PressSpellUp()
    {
        if (isGamepadUsing)
        {
            if (Input.GetButtonUp("L1"))
            {
                spellNumUp = 1;
            }
            else if (Input.GetButtonUp("R1"))
            {
                spellNumUp = 2;
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
                spellNumUp = 0;
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                spellNumUp = spellNum;
                spellNum = 0;
            }
            else
            {
                spellNumUp = 0;
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
            rSvalue = steakDirection;
            steakDirection.Normalize();
            rightSteakDirection = steakDirection;
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
            rightSteakDirection = newHitPoint;
            rSvalue = newHitPoint;
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

    public Vector3 RightSteakDirection()
    {
        return rightSteakDirection;
    }

    public Vector3 MousePosition()
    {
        return rSvalue;
    }

    public bool IsGamepadUsing()
    {
        return isGamepadUsing;
    }

    public Vector3 LeftSteakDirection()
    {
        return leftSteakDirection;
    }

    public short SpellNum()
    {
        return spellNum;
    }

    public short SpellNumUp()
    {
        return spellNumUp;
    }

    public bool Roll()
    {
        return buttonRoll;
    }

    public bool MoveTime()
    {
        return buttonMoveTime;
    }

    public bool MoveBox()
    {
        return buttonMoveBox;
    }

    public bool ClimbOnBox()
    {
        return buttonClimbOnBox;
    }

    public void IsGamepadUsing(bool isGamepadUsing)
    {
        this.isGamepadUsing = isGamepadUsing;
    }
}
