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
    [SerializeField] private TextMeshProUGUI strokePerHoleText;
    [SerializeField] private Slider powerSlider;


    private GameObject currentBall;
    private int strokeCount = 0;
    private int currentHole = 1;
    private int totalHoles = 9;
    private int totalScore;

    private List<int> holeScores = new List<int>();

    // Start is called before the first frame update

    void Start()
    {
        if (strokeCountText == null)
        {
            Debug.Log("stroke error");
        }
        holeScores = new List<int>(new int[totalHoles]);
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

    public void UpdatePerHoleScoreboard()
    {
        Debug.Log("Scoreboard Update called");
        holeScores[currentHole - 1] = strokeCount;
        UpdateScoreboardUI();
        strokeCount = 0;
        currentHole++;
    }

    private void UpdateScoreboardUI()
    {
        Debug.Log("UpdatePerHoleScoreboardUI Called");
        string scoreText = "Scorecard: ";
        foreach (int score in holeScores)
        {
            scoreText += "|| " + score.ToString() + " ";
        }

        totalScore = 0;
        foreach(int score in holeScores)
        {
            totalScore += score;
        }

        if (totalScore < 10)
        {
            scoreText += "|| 0" + totalScore.ToString() + " ||";
        }
        else
        {
            scoreText += "|| " + totalScore.ToString() + " ||";
        }

        strokePerHoleText.text = scoreText;

        Debug.Log("Updated Scorecard: " + scoreText);
    }
}
