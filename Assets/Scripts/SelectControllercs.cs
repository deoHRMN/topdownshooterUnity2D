using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectControllercs : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene"); // load main menu screen
    }
}
