using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI strokeCountText;
    //[SerializeField] private TextMeshProUGUI holeScoreText;

    private int strokeCount = 0;
    private int currentHole = 1;
    private int totalHoles = 9;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStrokeUI();
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
        //holeScoreText.text = "Hole: " + currentHole;
    }

    // Update is called once per frame
    public void NextHole()
    {
        if (currentHole < totalHoles)
        {
            currentHole++;
            SceneManager.LoadScene("Hole " + currentHole);
            UpdateStrokeUI();
        }
    }
}
