using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenSettings()
    {

        Debug.Log("Open Settings");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();

    }
}