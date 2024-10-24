using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] Transform ballSpawnerPos;
    [SerializeField] GameObject golfBallPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI powertext;

    private GameObject currentBall;

    // Start is called before the first frame update
    void Start()
    {
        SpawnGolfBall();
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
        ballScript.InitializeBall(mainCamera, powertext);
    }

    public void RespawnGolfBall()
    {
        SpawnGolfBall();
    }
}
