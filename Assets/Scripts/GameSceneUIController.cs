using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public SpriteRenderer player1ImageSpriteRenderer; // Reference to the GameObject's SpriteRenderer for character image
    public SpriteRenderer player1NameSpriteRenderer; // Reference to the GameObject's SpriteRenderer for character name
    public SpriteRenderer player2ImageSpriteRenderer; // Reference to the GameObject's SpriteRenderer for character image
    public SpriteRenderer player2NameSpriteRenderer; // Reference to the GameObject's SpriteRenderer for character name

    void Start()
    {
        // Set Player 1 sprites
        if (SelectedCharacters.Player1Character != null)
        {
            Debug.Log("Setting Player 1 character name sprite: " + SelectedCharacters.Player1Character.characterName.name);
            player1ImageSpriteRenderer.sprite = SelectedCharacters.Player1Character.characterImage;
            player1NameSpriteRenderer.sprite = SelectedCharacters.Player1Character.characterName;
        }
        else
        {
            Debug.LogError("Player 1 character is null");
        }

        // Set Player 2 sprites
        if (SelectedCharacters.Player2Character != null)
        {
            Debug.Log("Setting Player 2 character name sprite: " + SelectedCharacters.Player2Character.characterName.name);
            player2ImageSpriteRenderer.sprite = SelectedCharacters.Player2Character.characterImage;
            player2NameSpriteRenderer.sprite = SelectedCharacters.Player2Character.characterName;
        }
        else
        {
            Debug.LogError("Player 2 character is null");
        }
    }
}
