using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup InfoPanel;

    public void PlayGame()
    {
        // Load the next scene in the build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void InfoMenu()
    {
        // Show the information panel
        InfoPanel.alpha = 1;
        InfoPanel.blocksRaycasts = true;
    }

    public void Back()
    {
        // Hide the information panel
        InfoPanel.alpha = 0;
        InfoPanel.blocksRaycasts = false;
    }
}
