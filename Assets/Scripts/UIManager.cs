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
    [SerializeField] private GameObject upgradeStationPromptText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI dustText;
    [Space]
    [SerializeField] private UpgradeManager upgradeManager;

    private PlayerScript playerScript;

    private void Awake()
    {
        startScreen.SetActive(true);
        ShowCatHelpText(false);
        ShowEndScreen(false);
        ShowUpgradeScreen(false);
        ShowUpgradeStationPrompt(false);

        playerScript = FindObjectOfType<PlayerScript>();

        LockCursor(false);
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
        LockCursor(!_show);
    }
    public void ShowUpgradeStationPrompt(bool _show)
    {
        upgradeStationPromptText.SetActive(_show);
    }
    public void ShowUpgradeScreen(bool _show)
    {
        LockCursor(!_show);
        upgradeManager.gameObject.SetActive(_show);
    }
    public void ExitToMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGamePressed()
    {
        LockCursor(true);

        startScreen.SetActive(false);
        playerScript.StartGame();
    }

    void LockCursor(bool _locked)
    {
        if(_locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}