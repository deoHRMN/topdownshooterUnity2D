using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public PlayerPowerUp player1PowerUp;
    public PlayerPowerUp player2PowerUp;

    void Start()
    {
        // Set power-up type for Player 1
        if (SelectedCharacters.Player1Character != null)
        {
            player1PowerUp.powerUpType = SelectedCharacters.Player1Character.powerUpType;
        }

        // Set power-up type for Player 2
        if (SelectedCharacters.Player2Character != null)
        {
            player2PowerUp.powerUpType = SelectedCharacters.Player2Character.powerUpType;
        }
    }
}
