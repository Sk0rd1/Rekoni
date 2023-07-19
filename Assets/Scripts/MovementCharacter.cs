﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class MovementCharacter : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    MagicSpells magicSpells;

    [SerializeField]
    private float MOVESPEED = 15f;
    [SerializeField]
    private float turnDuration = 0.1f;
    [SerializeField]
    private float gravity = 15f;
    [SerializeField]
    private Camera cameraCharacter;
    [SerializeField]
    private Vector3 moveTimeDirection = new Vector3(200f, 0f, 0f);

    private bool isNewTime = true;
    private bool isCastSpell = false;
    private bool[] spellNum = { false/*L1*/, false/*R1*/, false/*L2*/, false/*R2*/};
    private bool isMoveTimeReady = true;
    private bool isMoveTimeCast = false;
    private bool isRollingReady = true;
    private bool isRolling = false;
    private bool isClimbing = false;
    private bool isFirstStageOfClimbing = false;
    private bool isMoveBox = false;
    private float oldValueY;
    private float moveSpeed;

    private GameObject currentBoxNew;
    private GameObject currentBoxOld;
    private GameObject boxForClimb;
    private GameObject boxForClimbNew;
    private GameObject boxForClimbOld;

    // V-sync and lock FPS
    //private void Awake()
    //{
    //    QualitySettings.vSyncCount = 0;
    //    Application.targetFrameRate = 60;
    //}

    private void Awake()
    {
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = 1000;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        oldValueY = transform.position.y;
        magicSpells = GetComponent<MagicSpells>();
    }

    void Update()
    {
        moveSpeed = MOVESPEED;
        Vector3 currentDirection = GetCurrentPosition();

        if (Input.GetButtonDown("Roll") && isRollingReady && !isMoveBox && !isClimbing && !isMoveTimeCast && !isCastSpell)
        {
            StartCoroutine(Roll());
        }

        if (Input.GetButtonDown("MoveTime") && !isMoveBox && !isClimbing && isMoveTimeReady && !isRolling && !isMoveTimeCast && !isCastSpell )
        {
            isMoveTimeCast = true;
            StartCoroutine(MoveTime());
        }

        if (Input.GetButtonDown("ClimbOnBox") && (currentBoxNew != null || currentBoxOld != null || boxForClimb != null) && !isMoveBox && !isClimbing && !isRolling && !isMoveTimeCast && !isCastSpell)
        {
            StartCoroutine(ClimbOnBox());
        }

        if (Input.GetButtonDown("MoveBox") && ((currentBoxNew != null && currentBoxOld != null) || isMoveBox) && !isClimbing && !isRolling && !isCastSpell )
        {
            StateOfMoveBox();
        }

        if (Input.GetButton("L1"))
        {
            spellNum[0] = true;
            spellNum[1] = false;
            spellNum[2] = false;
            spellNum[3] = false;
        }
        else if (Input.GetButton("R1"))
        {
            spellNum[0] = false;
            spellNum[1] = true;
            spellNum[2] = false;
            spellNum[3] = false;
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
            spellNum[0] = false;
            spellNum[1] = false;
            spellNum[2] = false;
            spellNum[3] = false;
        }

        Vector3 rightSteakDirection = RightSteak();
        if(!isMoveBox && (spellNum[0] || spellNum[1] || spellNum[2] || spellNum[3]))
        {
            isCastSpell = true;
            magicSpells.CastSpell(true, rightSteakDirection);
            animator.SetBool("isMoveBox", true);
        }
        else
        {
            if (!isMoveBox)
                animator.SetBool("isMoveBox", false);
            isCastSpell = false;
            magicSpells.CastSpell(false, rightSteakDirection);
        }

        if (!Input.GetButton("MoveTime"))
        {
            isMoveTimeCast = false;
            animator.SetBool("isMoveTime", false);
        }

        if (!isFalling() && !isMoveTimeCast && !isRolling && !isClimbing)
        {
            Running(currentDirection);
        }

        if (isMoveBox && (currentBoxNew != null && currentBoxOld != null) && !isClimbing && !isRolling && !isCastSpell)
        {
            MoveBox();
        }

        if (!isClimbing && !isMoveTimeCast)
        {
            controller.Move(new Vector3(0f, -gravity, 0f) * Time.deltaTime);
        }

        if (isRolling && !isCastSpell)
        {
            controller.Move(transform.forward * moveSpeed * 1.75f * Time.deltaTime);
        }

        if(isFirstStageOfClimbing)
        {
            transform.position += new Vector3(0f, 0.17f * gravity * Time.deltaTime, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoxInteractionNew"))
        {
            currentBoxNew = other.gameObject;
            boxForClimbNew = other.gameObject;

            string nameOld = currentBoxNew.name;
            string numberOld = nameOld.Substring(13);
            string nameNew = "BoxForMoveOld" + numberOld;

            currentBoxOld = GameObject.Find(nameNew);
        }

        if (other.CompareTag("BoxInteractionOld"))
        {
            currentBoxOld = other.gameObject;
            boxForClimbOld = other.gameObject;

            string nameNew = currentBoxOld.name;
            string numberNew = nameNew.Substring(13);
            string nameOld = "BoxForMoveOld" + numberNew;

            currentBoxOld = GameObject.Find(nameOld);
        }

        if(other.CompareTag("ToClimb"))
        {
            boxForClimb = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("BoxInteractionNew"))
        {
            boxForClimbNew = null;
        }

        if (other.CompareTag("BoxInteractionOld"))
        {
            boxForClimbOld = null;
        }

        if (other.CompareTag("ToClimb"))
        {
            boxForClimb = null;
        }
    }

    private void StateOfMoveBox()
    {
        if (isMoveBox)
        {
            bool isCancelMoveBox = true;
            Vector3 pointToCheckOld = currentBoxOld.transform.position;
            Vector3 pointToCheckNew = currentBoxNew.transform.position;

            Collider[] collidersNew = Physics.OverlapSphere(pointToCheckNew, 2.49f);
            Collider[] collidersOld = Physics.OverlapSphere(pointToCheckOld, 2.49f);

            foreach (Collider collider in collidersNew)
            {
                if (collider.tag == "BanTeleport")
                {
                    isCancelMoveBox = false;
                }
            }

            foreach (Collider collider in collidersOld)
            {
                if (collider.tag == "BanTeleport")
                {
                    isCancelMoveBox = false;
                }
            }

            if (isCancelMoveBox)
            {
                isMoveBox = false;
                animator.SetBool("isMoveBox", false);

                currentBoxNew.GetComponent<Rigidbody>().isKinematic = true;
                currentBoxOld.GetComponent<Rigidbody>().isKinematic = true;
            }

        }
        else
        {
            isMoveBox = true;
            currentBoxNew.GetComponent<Rigidbody>().isKinematic = false;
            currentBoxOld.GetComponent<Rigidbody>().isKinematic = false;
            animator.SetBool("isMoveBox", true);
        }

    }

    private Vector3 RightSteak()
    {
        float horizontalDirection = 0f;
        float verticalDirection = 0f;

        horizontalDirection = Input.GetAxis("Axis 4");
        verticalDirection = -Input.GetAxis("Axis 5");

        if (Input.GetKey(KeyCode.UpArrow))
            verticalDirection = 1;

        if (Input.GetKey(KeyCode.DownArrow))
            verticalDirection = -1;

        if (Input.GetKey(KeyCode.LeftArrow))
            horizontalDirection = -1;

        if (Input.GetKey(KeyCode.RightArrow))
            horizontalDirection = 1;

        Vector3 steakDirection = new Vector3(horizontalDirection, 0f, verticalDirection);
        steakDirection.Normalize();
        return steakDirection;
    }

    private void MoveBox()
    {
        Vector3 boxDirection = RightSteak();

        if (currentBoxNew != null && currentBoxOld != null)
        {
            if (isNewTime)
            {
                //currentBoxNew.transform.position += boxDirection * moveSpeed * Time.deltaTime;
                currentBoxNew.GetComponent<Rigidbody>().AddForce(5f * boxDirection * moveSpeed * Time.deltaTime, ForceMode.Impulse);
                currentBoxOld.transform.position = currentBoxNew.transform.position + moveTimeDirection;
                transform.LookAt(new Vector3(currentBoxNew.transform.position.x, transform.position.y, currentBoxNew.transform.position.z));
            }
            else
            {
                //currentBoxOld.transform.position += boxDirection * moveSpeed * Time.deltaTime;
                currentBoxOld.GetComponent<Rigidbody>().AddForce(5f * boxDirection * moveSpeed * Time.deltaTime, ForceMode.Force);
                Debug.Log(1000000000 * boxDirection * moveSpeed * Time.deltaTime);
                currentBoxNew.transform.position = currentBoxOld.transform.position - moveTimeDirection;
                transform.LookAt(new Vector3(currentBoxOld.transform.position.x, transform.position.y, currentBoxOld.transform.position.z));
            }

        }

        /*if (boxForMoveOld != null)
        {
            // тут повинен бути інший код
            string nameOld = boxForMoveOld.name;
            string numberOld = nameOld.Substring(13);
            string nameNew = "BoxForMoveNew" + numberOld;

            boxFromOldToNew = GameObject.Find(nameNew);

            boxForMoveOld.AddForce(boxDirection * 1000 * Time.deltaTime, ForceMode.Force);
            boxFromOldToNew.transform.position = boxForMoveOld.transform.position - moveTimeDirection;
            transform.LookAt(new Vector3(boxForMoveOld.transform.position.x, transform.position.y, boxForMoveOld.transform.position.z));
        }*/
    }

    private IEnumerator ClimbOnBox()
    {
        GameObject currentBox = null;

        if (boxForClimbOld != null)
        {
            Debug.Log("currentBoxOld");
            currentBox = boxForClimbOld;
        }

        if (boxForClimbNew != null)
        {
            Debug.Log("currentBoxNew");
            currentBox = boxForClimbNew;
        }

        if (boxForClimb != null)
        {
            Debug.Log("boxForClimb");
            currentBox = boxForClimb;
        }

        if (currentBox == null) yield break;

        isClimbing = true;

        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(RunToClimb(currentBox));

        //transform.LookAt(new Vector3(currentBox.transform.position.x, transform.position.y, currentBox.transform.position.z));
        animator.SetBool("isClimbing", true);
        isFirstStageOfClimbing = true;

        yield return new WaitForSeconds(0.9f);

        animator.SetBool("isClimbing", false);
        isFirstStageOfClimbing = false;

        Vector3 climbDirection = transform.forward * 1.2f;
        climbDirection.y += 3f;
        transform.position += climbDirection;

        animator.speed = 0.5f;

        yield return new WaitForSeconds(0.4f);

        animator.speed = 1f;
        isClimbing = false;
    }

    private IEnumerator RunToClimb(GameObject currentBox)
    {
        GameObject childObject = currentBox.transform.Find("CubeBanTeleport").gameObject;
        Collider childCollider = childObject.GetComponent<Collider>();

        Vector3 direction = childCollider.ClosestPoint(transform.position) - transform.position;

        direction.y = 0;
        direction.Normalize();

        transform.rotation = Quaternion.LookRotation(direction);


        Vector3 oldVector = Vector3.zero;
        Vector3 newVector = transform.position;
        controller.Move(direction * moveSpeed * Time.deltaTime);

        while (true)
        {
            animator.SetBool("isRunning", true);
            newVector = transform.position;
            if ((Mathf.Abs(direction.z) < 0.01f && Mathf.Abs(oldVector.x - newVector.x) < 0.01f) || (Mathf.Abs(direction.x) < 0.01f && Mathf.Abs(oldVector.z - newVector.z) < 0.01f) || (Mathf.Abs(oldVector.x - newVector.x) < 0.01f && Mathf.Abs(oldVector.z - newVector.z) < 0.01f))
            {
                yield break;
            }

            controller.Move(direction * moveSpeed * Time.deltaTime);
            oldVector = newVector;
            yield return null;
        }
    }


    private bool isFalling()
    {
        bool isFall = false;

        if ((oldValueY - transform.position.y) > 0.05f * Time.deltaTime)
        {
            //isRolling = false;
            isMoveTimeCast = false;
            isFall = true;
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        oldValueY = transform.position.y;

        return isFall;
    }

    private void Running(Vector3 currentDirection)
    {
        if (isMoveBox)
        {
            moveSpeed = MOVESPEED / 3f;
            Vector3 rotationVector = transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(0f, rotationVector.y, 0f);
            Vector3 relativeVector = transform.InverseTransformDirection(currentDirection);
            animator.SetFloat("MoveBoxHorizontal", relativeVector.x);
            animator.SetFloat("MoveBoxVertical", relativeVector.z);
        }

        if (spellNum[0] || spellNum[1] || spellNum[2] || spellNum[3])
        {
            moveSpeed = MOVESPEED / 3f;
            Vector3 rotationVector = transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(0f, rotationVector.y, 0f);
            Vector3 relativeVector = transform.InverseTransformDirection(currentDirection);
            animator.SetFloat("MoveBoxHorizontal", relativeVector.x);
            animator.SetFloat("MoveBoxVertical", relativeVector.z);
        }

        if (Mathf.Abs(currentDirection.x) + Mathf.Abs(currentDirection.z) > 0.1f)
        {
            animator.SetBool("isRunning", true);
            RotationCharacter(currentDirection);
            controller.Move(currentDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private Vector3 GetCurrentPosition()
    {
        Vector3 moveDirection = Vector3.zero;

        float horizontalInput = 0f;
        float verticalInput = 0f;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.W))
            verticalInput = 1;

        if (Input.GetKey(KeyCode.S))
            verticalInput = -1;

        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1;

        if (Input.GetKey(KeyCode.D))
            horizontalInput = 1;

        if ((oldValueY - transform.position.y) > 0.1f)
        {
            isRolling = false;
            isMoveTimeCast = false;
            horizontalInput = 0;
            verticalInput = 0;
            moveDirection.z = -gravity;
        }
        else
        {
            moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
            moveDirection.Normalize();
        }

        return moveDirection;
    }

    private IEnumerator MoveTime()
    {
        isMoveTimeReady = false;
        animator.SetBool("isRunning", false);
        animator.SetBool("isMoveTime", true);

        Vector3 pointToCheck = Vector3.zero;

        if (isNewTime)
        {
            pointToCheck = transform.position + moveTimeDirection;
        }
        else
        {
            pointToCheck = transform.position - moveTimeDirection;
        }

        pointToCheck.y += 1;

        yield return new WaitForSeconds(0.5f);

        Collider[] colliders = Physics.OverlapSphere(pointToCheck, 1f);
        pointToCheck.y -= 1;
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "BanTeleport" || collider.tag == "BoxBanTeleport")
            {
                animator.SetBool("isMoveTime", false);
                isMoveTimeReady = true;
                yield break;
            }
        }

        yield return new WaitForSeconds(1f);

        if (isMoveTimeCast)
        {
            transform.position = pointToCheck;
            cameraCharacter.transform.position = transform.position + new Vector3(0f, 40f, -11f);

            if (isNewTime)
                isNewTime = false;
            else
                isNewTime = true;

        }

        yield return new WaitForSeconds(0.5f);

        isMoveTimeCast = false;
        animator.SetBool("isMoveTime", false);
        isMoveTimeReady = true;
    }

    Quaternion newRotation;
    void RotationCharacter(Vector3 vector)
    {
        if (!isMoveBox)
        {
            if (vector != Vector3.zero)
            {
                float angle = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;

                newRotation = Quaternion.Euler(0f, angle, 0f);
            }

            StartCoroutine(SmoothRotate(newRotation, turnDuration));
        }
    }

    IEnumerator SmoothRotate(Quaternion targetRotation, float duration)
    {
        if (duration <= 0f)
        {
            transform.rotation = targetRotation;
            yield break;
        }

        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private IEnumerator Roll()
    {
        animator.SetBool("isRoll", true);
        isRolling = true;
        isRollingReady = false;

        animator.speed = 1.5f;

        controller.center = new Vector3(0f, 0.6f, 0f);
        controller.height = 1.2f;

        yield return new WaitForSeconds(0.2f);

        controller.center = new Vector3(0f, 0.45f, 0f);
        controller.height = 0.9f;

        yield return new WaitForSeconds(0.45f);

        animator.speed = 1.0f;
        animator.SetBool("isRoll", false);

        yield return new WaitForSeconds(0.05f);

        controller.center = new Vector3(0f, 0.9f, 0f);
        controller.height = 1.8f;
        isRolling = false;

        yield return new WaitForSeconds(0.7f);
        isRollingReady = true;
    }
}