using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] float initMoveSpeed = 5f;
    [SerializeField] float stopThreshold = 0.1f;
    [SerializeField] float slowRate = 2f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Rigidbody rb;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        moveSpeed = initMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("0"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                isMoving = true;
                moveSpeed = initMoveSpeed;
            }
        }
        MoveBall();
    }

    //void HandleInput ()
    //{
    //    if (Input.GetButtonDown("0"))
    //    {
    //        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            targetPosition = hit.point;
    //            isMoving = true;
    //        }
    //    }
    //}

    void MoveBall()
    {
        if (isMoving)
        {

                Vector3 direction = (targetPosition - transform.position).normalized;
                Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
                rb.MovePosition(newPosition);


                if (Vector3.Distance(transform.position, targetPosition) < stopThreshold)
                {
                    isMoving = false;
                    return;
                }

                if (moveSpeed > 0)
                {
                    moveSpeed -= slowRate * Time.deltaTime;
                    moveSpeed = Mathf.Max(moveSpeed, 0);
                }
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Hole"))
    //    {
    //        //Ball has entered hole move to next one
    //    }
    //}
