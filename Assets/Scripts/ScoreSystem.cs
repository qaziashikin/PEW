using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required for working with UI elements
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    // public TextMeshProUGUI scoreText; // Assign this in the inspector with your UI Text element
    public int score = 0; // Initial score
    public Text scoreBoardText;
    private int time = 3;
    public Text timeText; // Change this line
    public Text finalMessageText; // Change this line

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText(); // Initial score update
        StartCoroutine(UpdateTimeText());
    }

    // Method to add points and update the score display
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Method to update the score text UI
    private void UpdateScoreText()
    {
        scoreBoardText.text = "Score: " + score.ToString();
        // Debug.Log("Current Score: " + score);
    }

    // Update is called once per frame
    IEnumerator UpdateTimeText()
    {
        while (time >= 0)
        {
            timeText.text = "Time: " + time.ToString();
            yield return new WaitForSeconds(1);
            time--; // Decrease time each second

            if (time <= 0)
            {
                // Stop the game when time is 0
                Time.timeScale = 0;
                DisplayFinalMessage();
            }
        }
    }

    void DisplayFinalMessage()
    {
        finalMessageText.text = "Game over. Your final score was: " + score.ToString();
        Application.Quit();
    }
}
