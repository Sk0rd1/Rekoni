using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{

    CharacterController controller;
    Animator animator;
    private Camera cameraCharacter;
    private Vector3 moveTimeDirection;


    private float moveSpeed;
    private float gravity;
    private float turnDuration;

    private Vector3 climbMoveDirection = Vector3.zero;
    private bool nowCastMoveTime;
    private bool isClimbing;
    private bool isMoveBox;
    private void InitializeVariables()
    {
        MovementCharacter point = new MovementCharacter();
        moveSpeed = point.MOVESPEED;
        gravity = point.GRAVITY;
        turnDuration = point.TURNDURATION;
        isMoveBox =point.ISMOVEBOX;
        isClimbing = point.ISCLIBING;
        nowCastMoveTime = point.NOWCASTMOVETIME;
        moveTimeDirection=point.MOVETIMEDIRECTION;
    }




    private bool firstStageOfClimbing = false;
    private bool secondStageOfClimbing = false;

    private Rigidbody boxForMoveNew;
    private Rigidbody boxForMoveOld;
    private GameObject currentBoxNew;
    private GameObject currentBoxOld;
    public Rigidbody BOXORMOVENEW
    { get { return boxForMoveNew; } }
    public Rigidbody BOXFORMOVEOLD
    { get { return boxForMoveNew; } }
    public GameObject CURRENTBOXNEW
    { get { return currentBoxNew; } }
    public GameObject CURRENTBOXOLD
    { get { return currentBoxOld; } }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetBoxForMove()
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

        Vector3 boxDirection = new Vector3(horizontalDirection, 0f, verticalDirection);

        if (boxForMoveNew != null)
        {
            boxForMoveNew.AddForce(boxDirection * 1000 * Time.deltaTime, ForceMode.Force);
            transform.LookAt(new Vector3(boxForMoveNew.transform.position.x, 0f, boxForMoveNew.transform.position.z));
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

        yield return new WaitForSeconds(0.3f);

        animator.SetBool("isClimbing", true);

        firstStageOfClimbing = true;

        yield return new WaitForSeconds(0.6f);

        firstStageOfClimbing = false;
        secondStageOfClimbing = true;

        yield return new WaitForSeconds(0.4f);

        secondStageOfClimbing = false;

        animator.SetBool("isClimbing", false);

        isClimbing = false;
    }

}
