using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] Transform ballSpawnerPos;
    [SerializeField] GameObject golfBallPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI powertext;
    [SerializeField] private TextMeshProUGUI strokeCountText;
    [SerializeField] private TextMeshProUGUI strokePerHoleText;
    [SerializeField] private Slider powerSlider;
    [SerializeField] private TextMeshProUGUI holeNumberText;


    private GameObject currentBall;
    private int strokeCount = 0;
    private int currentHole = 1;
    private int totalHoles = 9;
    private int totalScore;

    private List<int> holeScores = new List<int>();

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    Debug.Log("GameManager not found");
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update

    void Start()
    {
        if (strokeCountText == null)
        {
            Debug.Log("stroke error");
        }

        GameObject[] objects = GameObject.FindGameObjectsWithTag("GameManager");

        if (objects.Length > 1)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(mainCamera);
        holeScores = new List<int>(new int[totalHoles]);
        SpawnGolfBall();
        UpdateStrokeUI();

        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnGolfBall()
    {

        Transform newBallSpawnerPos = GameObject.Find("Ball Spawn Position").transform;

        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(golfBallPrefab, newBallSpawnerPos.position, Quaternion.identity);
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

        string holeText = "Hole: " + (currentHole + 1);
        holeNumberText.text = holeText;

        Debug.Log(holeText);
    }

    public void NextHole()
    {

        Debug.Log("Loaded next hole");
        SceneManager.LoadScene(currentHole - 1);
    }

    public void ActiveSceneChanged(Scene scene, Scene nextScene)
    {
        SpawnGolfBall();
    }
}
