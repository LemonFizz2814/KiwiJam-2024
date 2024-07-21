using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditList : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}