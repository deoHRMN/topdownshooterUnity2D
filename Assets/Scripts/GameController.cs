using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // UI elements for timer and game over screen
    public TextMeshPro timerText;
    public CanvasGroup gameOverScreen;
    public TextMeshProUGUI winnerText;

    // Game timer variables
    private float timeRemaining = 300f; // 5 minutes in seconds
    private bool isGameOver = false; // Flag to check if the game is over

    void Start()
    {
        UpdateTimerText(); // Update the timer text at the start
        HideGameOverScreen(); // Hide the game over screen at the start
    }

    void Update()
    {
        if (!isGameOver) // Only update the timer if the game is not over
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Decrease time remaining by elapsed time
                UpdateTimerText(); // Update the timer text UI

                if (timeRemaining <= 0)
                {
                    timeRemaining = 0; // Set time remaining to 0 if it goes below 0
                    EndGame(null); // End the game with no winner (time up)
                }
            }
        }
    }

    // Called when a player dies
    public void PlayerDied(string playerTag)
    {
        if (!isGameOver) // Only end the game if it's not already over
        {
            string winnerTag = playerTag == "Player 1" ? "Player 2" : "Player 1"; // Determine the winner based on the player who died
            EndGame(winnerTag); // End the game with the winner
        }
    }

    // Update the timer text UI
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // Calculate minutes
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // Calculate seconds
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds); // Format the timer text
    }

    // End the game and show the game over screen
    private void EndGame(string winnerTag)
    {
        isGameOver = true; // Set game over flag to true
        ShowGameOverScreen(); // Show the game over screen

        if (winnerTag == null)
        {
            winnerText.text = "Time's Up!"; // Display "Time's Up!" if no winner
        }
        else
        {
            winnerText.text = winnerTag + " Wins!"; // Display the winner
        }
    }

    // Show the game over screen
    private void ShowGameOverScreen()
    {
        gameOverScreen.alpha = 1; // Set the alpha to 1 (fully visible)
        gameOverScreen.interactable = true; // Make the screen interactable
        gameOverScreen.blocksRaycasts = true; // Enable blocking raycasts
    }

    // Hide the game over screen
    private void HideGameOverScreen()
    {
        gameOverScreen.alpha = 0; // Set the alpha to 0 (fully invisible)
        gameOverScreen.interactable = false; // Make the screen non-interactable
        gameOverScreen.blocksRaycasts = false; // Disable blocking raycasts
    }

    // Reload the scene to replay the game
    public void ReplayGame()
    {
        SceneManager.LoadScene("SelectScreen"); // Load the character selection screen
    }
}
