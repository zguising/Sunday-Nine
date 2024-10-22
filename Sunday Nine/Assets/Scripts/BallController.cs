using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] float stopThreshold = 0.1f;
    [SerializeField] float slowRate = 0.2f;
    [SerializeField] float normalDrag = 0.05f;
    [SerializeField] float greenDrag = 2f;
    [SerializeField] TextMeshProUGUI powertext;

    private Vector3 targetPosition;
    private Vector3 dragStartPos;
    private bool isDragging = false;
    private Rigidbody rb;
    //private float moveSpeed;
    private float shotPower = 0f;
    private float maxPower = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.drag = normalDrag;
        powertext.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > stopThreshold)
        {
            rb.drag = Mathf.Lerp(rb.drag, slowRate, Time.deltaTime);
        }
        else if (rb.velocity.magnitude <= stopThreshold && rb.velocity.magnitude != 0)
        {
            rb.velocity = Vector3.zero;
            rb.drag = normalDrag;
        }
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    dragStartPos = Input.mousePosition;
                    isDragging = true;
                    shotPower = 0f;
                    powertext.gameObject.SetActive(true);
                }
            }
        }

        if (isDragging)
        {
            Vector3 currentMousePos = Input.mousePosition;
            float dragDistance = Vector3.Distance(dragStartPos, currentMousePos);

            shotPower = Mathf.Clamp(dragDistance, 0, maxPower);

            powertext.text = "Power: " + Mathf.RoundToInt(shotPower).ToString();

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                powertext.gameObject.SetActive(false);

                Vector3 dragDirection = (dragStartPos - currentMousePos).normalized;

                ApplyForce(dragDirection, shotPower);
            }
        }
    }

    void ApplyForce(Vector3 direction, float power)
    {
        float force = power * 0.1f;
        rb.AddForce(new Vector3(direction.x, 0, direction.y) * force, ForceMode.Impulse);
        Debug.Log("Apllied force with power: " + power);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Green"))
        {
            rb.drag = greenDrag;
            Debug.Log("Ball on green");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Green"))
        {
            rb.drag = normalDrag;
            Debug.Log("Ball NOT on green");
        }
    }
}

//void MoveBall()
//{
//    if (isMoving)
//    {

//            Vector3 newXZPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
//            Vector3 newPosition = Vector3.MoveTowards(transform.position, newXZPosition, moveSpeed * Time.deltaTime);

//            rb.MovePosition(newPosition);


//            if (Vector3.Distance(transform.position, newXZPosition) < stopThreshold)
//            {
//                isMoving = false;
//                return;
//            }

//            if (moveSpeed > 0)
//            {
//                moveSpeed -= slowRate * Time.deltaTime;
//                moveSpeed = Mathf.Max(moveSpeed, 0);
//            }
//        }
//    }

//private void OnCollisionEnter(Collision collision)
//{
//    if (collision.gameObject.CompareTag("Hole"))
//    {
//        //Ball has entered hole move to next one
//    }
//}
