using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectionController : MonoBehaviour
{
    // Configuration for Player 1
    [Header("Player 1 Configuration")]
    public Image[] player1Characters; // Array of character images for Player 1
    public Character[] player1CharacterSprites; // Array of character data for Player 1
    public RectTransform player1Indicator; // Indicator to show Player 1's selection

    // Configuration for Player 2
    [Header("Player 2 Configuration")]
    public Image[] player2Characters; // Array of character images for Player 2
    public Character[] player2CharacterSprites; // Array of character data for Player 2
    public RectTransform player2Indicator; // Indicator to show Player 2's selection

    // Timer Configuration
    [Header("Timer Configuration")]
    public TextMeshProUGUI timerText; // UI text for displaying the timer
    private float timer = 30f; // Initial timer value in seconds
    private bool timerActive = true; // Flag to check if the timer is active

    // Selection state
    private int player1Index = 0; // Index of the selected character for Player 1
    private int player2Index = 1; // Index of the selected character for Player 2
    private bool player1Selected = false; // Flag to check if Player 1 has selected a character
    private bool player2Selected = false; // Flag to check if Player 2 has selected a character

    // Colors for indicating selection
    private Color defaultColor = Color.white; // Default color for unselected characters
    private Color player1Color = Color.red; // Color for Player 1's selection
    private Color player2Color = Color.blue; // Color for Player 2's selection

    void Start()
    {
        // Update the character selection visuals at the start
        UpdateSelection();
    }

    void Update()
    {
        // Update the timer if it's active
        if (timerActive)
        {
            timer -= Time.deltaTime; // Decrease timer value by the elapsed time
            timerText.text = Mathf.Ceil(timer).ToString(); // Update the timer text UI

            // Check if the timer has expired
            if (timer <= 0)
            {
                timerActive = false; // Stop the timer
                AutoSelectCharacters(); // Auto-select characters if the timer expires
            }
        }

        // Handle Player 1's character selection
        if (!player1Selected)
        {
            if (Input.GetKeyDown(KeyCode.W)) // Move selection up
            {
                player1Index = (player1Index > 0) ? player1Index - 1 : player1Characters.Length - 1;
                UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.S)) // Move selection down
            {
                player1Index = (player1Index < player1Characters.Length - 1) ? player1Index + 1 : 0;
                UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Space)) // Confirm selection
            {
                SelectCharacter(1);
            }
        }

        // Handle Player 2's character selection
        if (!player2Selected)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // Move selection up
            {
                player2Index = (player2Index > 0) ? player2Index - 1 : player2Characters.Length - 1;
                UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) // Move selection down
            {
                player2Index = (player2Index < player2Characters.Length - 1) ? player2Index + 1 : 0;
                UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift)) // Confirm selection
            {
                SelectCharacter(2);
            }
        }
    }

    // Update the visual indicators for character selection
    void UpdateSelection()
    {
        // Clear previous selection colors
        foreach (var image in player1Characters)
        {
            image.color = defaultColor;
        }

        foreach (var image in player2Characters)
        {
            image.color = defaultColor;
        }

        // Apply new selection colors
        if (!player1Selected)
        {
            player1Characters[player1Index].color = player1Color;
        }

        if (!player2Selected)
        {
            player2Characters[player2Index].color = player2Color;
        }

        // Update the position of the indicators
        player1Indicator.position = new Vector3(player1Indicator.position.x, player1Characters[player1Index].transform.position.y, player1Indicator.position.z);
        player2Indicator.position = new Vector3(player2Indicator.position.x, player2Characters[player2Index].transform.position.y, player2Indicator.position.z);
    }

    // Select the character for the specified player
    void SelectCharacter(int player)
    {
        if (player == 1)
        {
            if (player1Index == player2Index && player2Selected)
            {
                Debug.Log("Player 2 has already selected this character!");
            }
            else
            {
                player1Selected = true;
                Debug.Log("Player 1 selected: " + player1Characters[player1Index].name);
                player1Indicator.gameObject.SetActive(false);
                SelectedCharacters.Player1Character = player1CharacterSprites[player1Index];
                SelectedCharacters.Player1Character.powerUpType = player1CharacterSprites[player1Index].powerUpType;
            }
        }
        else if (player == 2)
        {
            if (player2Index == player1Index && player1Selected)
            {
                Debug.Log("Player 1 has already selected this character!");
            }
            else
            {
                player2Selected = true;
                Debug.Log("Player 2 selected: " + player2Characters[player2Index].name);
                player2Indicator.gameObject.SetActive(false);
                SelectedCharacters.Player2Character = player2CharacterSprites[player2Index];
                SelectedCharacters.Player2Character.powerUpType = player2CharacterSprites[player2Index].powerUpType;
            }
        }

        // Check if both players have selected their characters
        if (player1Selected && player2Selected)
        {
            Debug.Log("Both players have selected their characters.");
            timerActive = false; // Stop the timer
            timerText.text = "0"; // Set the timer text to 0
            SceneManager.LoadScene("GameScene"); // Load the game scene
        }
    }

    // Auto-select characters if the timer expires
    void AutoSelectCharacters()
    {
        if (!player1Selected)
        {
            SelectCharacter(1);
        }
        if (!player2Selected)
        {
            SelectCharacter(2);
        }
        Debug.Log("Timer expired. Characters auto-selected.");
    }
}
