using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpells : MonoBehaviour
{
    GameObject cursor;
    Animator animator;
    MovementCharacter movementCharacter;
    private bool isCastSpell = false;
    private Vector3 steakDirection = Vector3.zero;

    void Start()
    {
        cursor = GameObject.Find("Cursor");
        animator = GetComponent<Animator>();
        movementCharacter = GetComponent<MovementCharacter>();
        cursor.transform.position = new Vector3(0f, -20f, 0f);
    }

    void Update()
    {
        if (isCastSpell)
        {
            if (steakDirection == Vector3.zero)
            {
                steakDirection = transform.forward;
            }
            //else
            //{
            //    CursorMove(steakDirection);
            //}

            if (Input.GetButton("L1"))
            {
                CursorMove(steakDirection);
            }
            else
            if (Input.GetButton("R1"))
            {
                CursorMove(steakDirection);
            }
        }
        else
        {
            cursor.transform.position = new Vector3(0f, 100f, 0f);
        }
    }

    private void CursorMove(Vector3 directionAxis)
    {
        directionAxis.y += 2f;
        directionAxis *= 2f;

        cursor.transform.position = transform.position + directionAxis;
    }

    public void CastSpell(bool isCastSpell, Vector3 steakDirection)
    {
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
