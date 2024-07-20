using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject endingScreen;
    [SerializeField] private GameObject catHelpText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI dustText;

    private PlayerScript playerScript;

    private void Awake()
    {
        startScreen.SetActive(true);
        ShowCatHelpText(false);
        ShowEndScreen(false);

        playerScript = FindObjectOfType<PlayerScript>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetPowerSliderValue(float _value, float _max)
    {
        powerSlider.value = _value;
        powerSlider.maxValue = _max;
        powerText.text = $"Power ({Mathf.Round(_value)}/{_max})";
    }
    public void SetDustValue(float _value, float _max)
    {
        dustText.text = $"Dust ({Mathf.Round(_value)}/{_max})";
    }
    public void ShowCatHelpText(bool _show)
    {
        catHelpText.SetActive(_show);
    }
    public void ShowEndScreen(bool _show)
    {
        endingScreen.SetActive(_show);
    }
    public void ExitToMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGamePressed()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startScreen.SetActive(false);
        playerScript.StartGame();
    }
}