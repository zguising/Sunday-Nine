using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    private Camera mainCamera;
    private TextMeshProUGUI powertext;

    [SerializeField] float stopThreshold = 0.1f;
    [SerializeField] float slowRate = 0.05f;
    [SerializeField] float normalDrag = 0.02f;
    [SerializeField] float greenDrag = 2f;

    private bool isOnGreen = false;

    private Vector3 targetPosition;
    private Vector3 dragStartPos;
    private bool isDragging = false;
    private Rigidbody rb;
    private float shotPower = 0f;
    private float maxPower = 100f;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.drag = normalDrag;

        gameManager = FindObjectOfType<GameManager>();

    }

    public void InitializeBall(Camera camera, TextMeshProUGUI powertextUI)
    {
        mainCamera = camera;
        powertext = powertextUI;

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
            rb.drag = Mathf.Lerp(rb.drag, slowRate, Time.deltaTime * 0.5f);
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

            int layerMask = LayerMask.GetMask("BallLayer");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
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
        float force = power * 0.05f;
        float variabilityFactor = power * 0.001f;
        float offset = Random.Range(-variabilityFactor, variabilityFactor);

        Vector3 adjustedDirection = new Vector3(direction.x, 0, direction.y + offset);

        rb.AddForce(adjustedDirection * force, ForceMode.Impulse);
        Debug.Log("Apllied force with power: " + power + " and variablility: " + offset);

        StartCoroutine(HeightEffect(power));
    }

    private IEnumerator HeightEffect(float power)
    {
        Vector3 ogScale = transform.localScale;

        float minHeight = 1.05f;
        float maxHieght = 1.6f;
        float heigthMultiplier = Mathf.Lerp(minHeight, maxHieght, power / maxPower);

        Vector3 targetScale = ogScale * 1.5f;

        float minDuration = 0.5f;
        float maxDuration = 2.5f;
        float duration = Mathf.Lerp(minDuration, maxDuration, power / maxPower);
        //float durationMultiplier = 1.5f;
        //duration *= durationMultiplier;

        float elaspedTime = 0f;

        while (elaspedTime < duration / 2)
        {
            transform.localScale = Vector3.Lerp(ogScale, targetScale, elaspedTime / (duration / 2));
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        elaspedTime = 0f;

        while (elaspedTime < duration / 2)
        {
            transform.localScale = Vector3.Lerp(targetScale, ogScale, elaspedTime / (duration / 2));
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = ogScale;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Green"))
        {
            isOnGreen = true;
            rb.drag = greenDrag;
            Debug.Log("Ball on green");
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            Debug.Log("Ball is in water");
            gameManager.RespawnGolfBall();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Green"))
        {
            isOnGreen = false;
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
