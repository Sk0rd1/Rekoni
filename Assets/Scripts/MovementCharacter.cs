/*Баги
 * 1 - Коробку можна перемістити персонажем(хз що робити, бо isKinematic не варіант і додавання Rigidbody до персонажа також не допомагає)
 * 2 - X не завжди спрацьовує коли потрібно взяти коробку(можливо потрібно збільшити область тригеру)
 * 3 + під час бігу натискання телепортації відміняє телепорт і через це на наступний раз йде телепортація за карту
 * 4 - Y взагалі не працює(хз що робити)
 * 5 - потрбіно зробити щоб при бізі спочатку був поворот а тільки потім рух
 * 6 - зробити норальний підйом на коробку
 * 7 - зробити щось схоже на східці (45 градусів)
 * 8 - добавити ричаг та двері
 * 9 - анімація падіння не завжди програється
 * 10 - відміна телепортації якщо заважає обєкт
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
    public float MOVESPEED
    { get { return moveSpeed; } }
    public float TURNDURATION
    { get { return turnDuration; } }
    public float GRAVITY
    { get { return gravity; } }
    public Vector3 MOVETIMEDIRECTION
    { get { return moveTimeDirection; } }

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
    public bool NOWCASTMOVETIME
    { get { return nowCastMoveTime; } }
    public bool ISCLIBING
    { get { return isClimbing; } }
    public bool ISMOVEBOX
    { get { return isMoveBox; } }

    private float oldValueY;
    private bool firstStageOfClimbing = false;
    private bool secondStageOfClimbing = false;

    private Rigidbody boxForMoveNew;
    private Rigidbody boxForMoveOld;
    private GameObject currentBoxNew;
    private GameObject currentBoxOld;
    /*private void InitializeVariables()
    {
        BoxMove point = new BoxMove();
        boxForMoveNew = point.BOXORMOVENEW;
        boxForMoveOld = point.BOXFORMOVEOLD;
        currentBoxNew = point.CURRENTBOXNEW;
        currentBoxOld = point.CURRENTBOXOLD;
    }*/



    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        oldValueY = transform.position.y;
    }

    void Update()
    {
        Vector3 currentDirection = GetCurrentPosition();
        if (!nowCastMoveTime)
        {
            isMoveTimeCast = false;
            isRolling = false;
            controller.Move(new Vector3(0f, -gravity, 0f) * Time.deltaTime);
        }

         if (Input.GetButtonDown("Roll") && isRollingReady && !isMoveBox && !isClimbing && !isMoveTimeCast)
        {
            StartCoroutine(Roll());
        }

        else if (Input.GetButtonDown("MoveTime") && !isMoveBox && !isClimbing && isMoveTimeReady && !isRolling)
        {
            isMoveTimeCast = true;
            StartCoroutine(MoveTime());
        }
        
        /*if (Input.GetButtonDown("ClimbOnBox") && (currentBoxNew != null || currentBoxOld != null) && !isMoveBox && !isClimbing && !isRolling)
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
        }*/

        else if (!Input.GetButton("MoveTime"))
        {
            isMoveTimeCast = false;
            animator.SetBool("isMoveTime", false);
        }

        if (!isFalling() && !isMoveTimeCast && !isRolling)
        {
            Running(currentDirection);
        }




        /*if (isMoveBox && (boxForMoveNew != null || boxForMoveOld) && !isClimbing && !isRolling)
        {
            MoveBox();
        }*/

        if (isRolling && !isClimbing && !isMoveTimeCast)
        {
            controller.Move(rollMoveDirection * moveSpeed * 1.75f * Time.deltaTime);
        }

        StageOfClimbing();
    }

    private void StageOfClimbing()
    {
        if(firstStageOfClimbing)
        {

            //controller.Move(new Vector3(0f, 2 * gravity, 0f) * Time.deltaTime);
        }
        if(secondStageOfClimbing)
        {

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


   

    private bool isFalling()
    {
        bool isFall = false;

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
        isRolling = false;
        isMoveTimeCast = false;
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

        if(isMoveBox)
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

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection.Normalize();

        moveDirection.y = -gravity;

        if(Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z) > 0.1 && !isRolling)
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

        if (isMoveTimeCast)
        {
            if (isNewTime)
            {
                pointToCheck = transform.position + moveTimeDirection;

                isNewTime = false;
            }
            else
            {
                pointToCheck = transform.position - moveTimeDirection;

                isNewTime = true;
            }

            nowCastMoveTime = true;
            boxForMoveNew = null;
            boxForMoveOld = null;
        }

        yield return new WaitForSeconds(1.5f);

        transform.position = pointToCheck;
        cameraCharacter.transform.position = transform.position + new Vector3(0f, 22.5f, -18f);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isMoveTime", false);

        yield return new WaitForSeconds(0.5f);

        isMoveTimeReady = true;
    }

        Quaternion newRotation;
    // потрібно зробити плавним поворот, можливо додати нові анімації для розвороту
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

    //нормально не повертається, потрібно фіксити
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
