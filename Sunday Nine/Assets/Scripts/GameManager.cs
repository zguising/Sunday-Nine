using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] Transform ballSpawnerPos;
    [SerializeField] GameObject golfBallPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI powertext;
    [SerializeField] private TextMeshProUGUI strokeCountText;
    [SerializeField] private Slider powerSlider;

    private GameObject currentBall;
    private int strokeCount = 0;

    // Start is called before the first frame update

    void Start()
    {
        if (strokeCountText == null)
        {
            Debug.Log("stroke error");
        }
        SpawnGolfBall();
        UpdateStrokeUI();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnGolfBall()
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(golfBallPrefab, ballSpawnerPos.position, Quaternion.identity);
        Ball ballScript = currentBall.GetComponent<Ball>();
        ballScript.InitializeBall(mainCamera, powertext, powerSlider);
    }

    public void RespawnGolfBall()
    {
        SpawnGolfBall();
    }

    public void AddStroke()
    {
        strokeCount++;
        Debug.Log("Stroke added " + strokeCount);
        UpdateStrokeUI();
    }

    private void UpdateStrokeUI()
    {
        strokeCountText.text = "Strokes: " + strokeCount;
        Debug.Log("Stroke upadated to " + strokeCountText.text);
    }
}
