using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

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

    public void CreditList()
    {
        SceneManager.LoadScene("Credits");
    }
}