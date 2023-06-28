/*Багів немає, я все пофіксив все, бо я талант
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 * ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class MovementCharacter : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float turnDuration = 0.1f;
    [SerializeField]
    private float gravity = 15f;
    [SerializeField]
    private Camera cameraCharacter;
    [SerializeField]
    private Vector3 moveTimeDirection = new Vector3(200f, 0f, 0f);


    private Vector3 rollMoveDirection = new Vector3(0f, 0f, 1f);
    private Vector3 climbMoveDirection = Vector3.zero;
    private bool isNewTime = true;
    private bool isMoveTimeReady = true;
    private bool isMoveTimeCast = false;
    private bool isRollingReady = true;
    private bool nowCastMoveTime = false;
    private bool isRolling = false;
    private bool isClimbing = false;
    private bool isMoveBox = false;
    private float oldValueY;

    private Rigidbody boxForMoveNew;
    private Rigidbody boxForMoveOld;
    private GameObject currentBoxNew;
    private GameObject currentBoxOld;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        oldValueY = transform.position.y;
    }

    void Update()
    {
        Vector3 currentDirection = GetCurrentPosition();
        nowCastMoveTime = false;

        if (Input.GetButtonDown("Roll") && isRollingReady && !isMoveBox && !isClimbing && !isMoveTimeCast)
        {
            StartCoroutine(Roll());
        }

        if (Input.GetButtonDown("MoveTime") && !isMoveBox && !isClimbing && isMoveTimeReady && !isRolling)
        {

            isMoveTimeCast = true;
            StartCoroutine(MoveTime());
        }

        if (Input.GetButtonDown("ClimbOnBox") && (currentBoxNew != null || currentBoxOld != null) && !isMoveBox && !isClimbing && !isRolling)
        {
            StartCoroutine(ClimbOnBox());
        }

        if (Input.GetButtonDown("MoveBox") && (currentBoxNew != null || currentBoxOld != null || isMoveBox) && !isClimbing && !isRolling)
        {
            if (isMoveBox)
            {
                isMoveBox = false;
                moveSpeed *= 2f;
                animator.SetBool("isMoveBox", false);
            }
            else
            {
                moveSpeed /= 2f;
                isMoveBox = true;
                animator.SetBool("isMoveBox", true);
                GetBoxForMove();
            }
        }

        if (!Input.GetButton("MoveTime"))
        {
            isMoveTimeCast = false;
            animator.SetBool("isMoveTime", false);
        }

        if (!isFalling() && !isMoveTimeCast && !isRolling)
        {
            Running(currentDirection);
        }

        if (isMoveBox && (boxForMoveNew != null || boxForMoveOld) && !isClimbing && !isRolling)
        {
            MoveBox();
        }

        if(!isClimbing && !isMoveTimeCast)
        {
            controller.Move(new Vector3(0f, -gravity, 0f) * Time.deltaTime);
        }

        if (isRolling)
        {
            controller.Move(rollMoveDirection * moveSpeed * 1.75f * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoxInteractionNew"))
        {
            currentBoxNew = other.gameObject;
            currentBoxOld = null;
        }

        if (other.CompareTag("BoxInteractionOld"))
        {
            currentBoxOld = other.gameObject;
            currentBoxNew = null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BoxInteractionNew"))
        {
            currentBoxNew = null;
        }

        if (other.CompareTag("BoxInteractionNew"))
        {
            currentBoxOld = null;
        }
    }


    private void GetBoxForMove()
    {
        try
        {
            boxForMoveNew = currentBoxNew.GetComponent<Rigidbody>();
        }
        catch
        {
            boxForMoveNew = null;
        }

        try
        {
            boxForMoveOld = currentBoxOld.GetComponent<Rigidbody>();
        }
        catch
        {
            boxForMoveOld = null;
        }
    }

    private void MoveBox()
    {
        float horizontalDirection = Input.GetAxis("Axis 4");
        float verticalDirection = -Input.GetAxis("Axis 5");

        if (Input.GetKey(KeyCode.UpArrow))
            verticalDirection = 1;

        if (Input.GetKey(KeyCode.DownArrow))
            verticalDirection = -1;

        if (Input.GetKey(KeyCode.LeftArrow))
            horizontalDirection = -1;

        if (Input.GetKey(KeyCode.RightArrow))
            horizontalDirection = 1;

        Vector3 boxDirection = new Vector3(horizontalDirection, 0f, verticalDirection);
        boxDirection.Normalize();

        if (boxForMoveNew != null)
        {
            boxForMoveNew.AddForce(boxDirection * 1000 * Time.deltaTime, ForceMode.Force);
            transform.LookAt(new Vector3(boxForMoveNew.transform.position.x, transform.position.y, boxForMoveNew.transform.position.z));
        }

        if (boxForMoveOld != null)
        {
            string nameOld = boxForMoveOld.name;
            string numberOld = nameOld.Substring(13);
            string nameNew = "BoxForMoveNew" + numberOld;

            GameObject boxFtomOldToNew = GameObject.Find(nameNew);

            boxForMoveOld.AddForce(boxDirection * 1000 * Time.deltaTime, ForceMode.Force);
            boxFtomOldToNew.transform.position = boxForMoveOld.transform.position - moveTimeDirection;
            transform.LookAt(new Vector3(boxForMoveOld.transform.position.x, 0f, boxForMoveOld.transform.position.z));
        }
    }

    private IEnumerator ClimbOnBox()
    {
        GameObject currentBox;

        if (currentBoxNew == null)
            currentBox = currentBoxOld;
        else
            currentBox = currentBoxNew;

        isClimbing = true;
        Quaternion rotationOld = transform.rotation;
        transform.LookAt(currentBox.transform);
        Quaternion rotationNew = transform.rotation;
        transform.rotation = new Quaternion(rotationOld.x, rotationNew.y, rotationOld.z, 1f);

        Vector3 direction = currentBox.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        controller.Move(direction * moveSpeed * Time.deltaTime);

        yield return new WaitForSeconds(0.1f);
        //transform.LookAt(currentBox.transform);
        animator.SetBool("isClimbing", true);

        yield return new WaitForSeconds(1.1f);

        Vector3 climbDirection = transform.forward * 1.2f;
        climbDirection.y += 5;
        transform.position += climbDirection;

        animator.SetBool("isClimbing", false);

        yield return new WaitForSeconds(0.05f);
        isClimbing = false;
    }

    private bool isFalling()
    {
        bool isFall = false;
        isRolling = false;
        if (Mathf.Abs(oldValueY - transform.position.y) > 0.1f)
        {
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

        if (Mathf.Abs(currentDirection.x) + Mathf.Abs(currentDirection.z) > 0.1)
        {
            animator.SetBool("isRunning", true);
            RotationCharacter(currentDirection);
            controller.Move(currentDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (isMoveBox)
        {
            Vector3 rotationVector = transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(0f, rotationVector.y, 0f);
            Vector3 relativeVector = transform.InverseTransformDirection(currentDirection);
            animator.SetFloat("MoveBoxHorizontal", relativeVector.x);
            animator.SetFloat("MoveBoxVertical", relativeVector.z);
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

        if (Mathf.Abs(oldValueY - transform.position.y) > 0.1f)
        {
            horizontalInput = 0;
            verticalInput = 0;
            moveDirection.z = -gravity;
        }
        else
        {
            moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
            moveDirection.Normalize();
        }

        if (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z) > 0.1 && !isRolling)
        {
            rollMoveDirection = moveDirection;
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

        yield return new WaitForSeconds(0.5f);

        Collider[] colliders = Physics.OverlapSphere(pointToCheck, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger)
            {
                animator.SetBool("isMoveTime", false);
                isMoveTimeReady = true;
                yield break;
            }
        }

        yield return new WaitForSeconds(1f);

        if (isMoveTimeCast)
        {
            nowCastMoveTime = true;
            transform.position = pointToCheck;
            cameraCharacter.transform.position = transform.position + new Vector3(0f, 22.5f, -18f);

            if (isNewTime)
                isNewTime = false;
            else
                isNewTime = true;

        }

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isMoveTime", false);

        yield return new WaitForSeconds(0.5f);

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