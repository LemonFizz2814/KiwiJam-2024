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
    [SerializeField] private GameObject lowBatteryText;
    [SerializeField] private GameObject batteryGoneObj;
    [Space]
    [SerializeField] private UpgradeManager upgradeManager;
    [Space]
    [SerializeField] private ParticleSystem dustVFX;
    [SerializeField] private float percentageToWin;

    private int startParticleCount;
    private int particlesToWin;

    private bool gameOver = false;

    private PlayerScript playerScript;

    private AudioSource audioSource;

    private void Awake()
    {
        startScreen.SetActive(true);
        ShowCatHelpText(false);
        ShowEndScreen(false);
        upgradeManager.gameObject.SetActive(false);
        ShowUpgradeStationPrompt(false);

        playerScript = FindObjectOfType<PlayerScript>();
        audioSource = GetComponent<AudioSource>();

        var mainModule = dustVFX.main;
        int maxParticles = mainModule.maxParticles;
        startParticleCount = maxParticles;

        particlesToWin = (int)(startParticleCount - (startParticleCount * (percentageToWin/100)));

        LockCursor(false);
    }

    private void Update()
    {
        if (gameOver)
            return;

        var mainModule = dustVFX.main;
        int maxParticles = mainModule.maxParticles;

        if (maxParticles <= particlesToWin)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOver = true;
        ShowEndScreen(true);
        playerScript.GameOver();
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
    public void ShowLowBatteryText(bool show)
    {
        lowBatteryText.SetActive(show);
    }
    public void ShowBatteryGoneText(bool show)
    {
        batteryGoneObj.SetActive(show);
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
        upgradeManager.UpdateAllText();

        audioSource.Play();
    }
    public void ExitToMenuPressed()
    {
        audioSource.Play();
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGamePressed()
    {
        LockCursor(true);
        audioSource.Play();

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