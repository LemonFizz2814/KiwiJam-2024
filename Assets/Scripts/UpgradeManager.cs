using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private List<Upgrade> suctionUpgrades = new List<Upgrade>();
    [SerializeField] private List<Upgrade> capacityUpgrades = new List<Upgrade>();
    [SerializeField] private List<Upgrade> powerUpgrades = new List<Upgrade>();
    [Space]
    [SerializeField] private TextMeshProUGUI suctionPriceText;
    [SerializeField] private TextMeshProUGUI capacityPriceText;
    [SerializeField] private TextMeshProUGUI powerPriceText;
    [Space]
    [SerializeField] private TextMeshProUGUI suctionIncreaseText;
    [SerializeField] private TextMeshProUGUI capacityIncreaseText;
    [SerializeField] private TextMeshProUGUI powerIncreaseText;
    [Space]
    [SerializeField] private TextMeshProUGUI suctionLevelText;
    [SerializeField] private TextMeshProUGUI capacityLevelText;
    [SerializeField] private TextMeshProUGUI powerLevelText;
    [Space]
    [SerializeField] private TextMeshProUGUI dustText;
    [Space]
    [SerializeField] private GameObject suctionButton;
    [SerializeField] private GameObject capacityButton;
    [SerializeField] private GameObject powerButton;
    [Space]
    [SerializeField] private UIManager uiManager;

    private AudioSource audioSource;

    private int suctionIndex = 0;
    private int capacityIndex = 0;
    private int powerIndex = 0;

    private PlayerScript playerScript;

    [Serializable]
    public struct Upgrade
    {
        public float cost;
        public float increase;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerScript = FindObjectOfType<PlayerScript>();

        suctionButton.SetActive(true);
        capacityButton.SetActive(true);
        powerButton.SetActive(true);

        UpdateText();
    }

    public void UpdateText()
    {
        suctionPriceText.text = suctionUpgrades[suctionIndex].cost + " Dust";
        capacityPriceText.text = capacityUpgrades[capacityIndex].cost + " Dust";
        powerPriceText.text = powerUpgrades[powerIndex].cost + " Dust";

        suctionIncreaseText.text = $"+{suctionUpgrades[suctionIndex].increase} Suck";
        capacityIncreaseText.text = $"+{capacityUpgrades[capacityIndex].increase} Max Dust";
        powerIncreaseText.text = $"+{powerUpgrades[powerIndex].increase} Max Power";

        suctionLevelText.text = $"Upgrade suction\nLvl. {suctionIndex}";
        capacityLevelText.text = $"Upgrade capacity\nLvl. {capacityIndex}";
        powerLevelText.text = $"Upgrade power\nLvl. {powerIndex}";

        dustText.text = $"Dust: {playerScript.GetDust()}";
    }

    public void UpgradeSuctionPressed()
    {
        if (playerScript.GetDust() <= suctionUpgrades[suctionIndex].cost)
        {
            float newDust = playerScript.GetDust() - suctionUpgrades[suctionIndex].cost;
            playerScript.SetDust(newDust);
            playerScript.SuctionIncreased(suctionUpgrades[suctionIndex].increase);
            suctionIndex++;
            audioSource.Play();

            if (suctionIndex >= suctionUpgrades.Count)
            {
                suctionButton.SetActive(false);
            }
            else
            {
                UpdateText();
            }
        }
    }
    public void UpgradeCapacityPressed()
    {
        if (playerScript.GetDust() <= capacityUpgrades[capacityIndex].cost)
        {
            float newDust = playerScript.GetDust() - capacityUpgrades[capacityIndex].cost;
            playerScript.SetDust(newDust);
            playerScript.CapacityIncreased(capacityUpgrades[capacityIndex].increase);
            capacityIndex++;
            audioSource.Play();

            if (capacityIndex >= capacityUpgrades.Count)
            {
                capacityButton.SetActive(false);
            }
            else
            {
                UpdateText();
            }
        }
    }
    public void UpgradePowerPressed()
    {
        if (playerScript.GetDust() <= powerUpgrades[powerIndex].cost)
        {
            float newDust = playerScript.GetDust() - powerUpgrades[powerIndex].cost;
            playerScript.SetDust(newDust);
            playerScript.PowerIncreased(powerUpgrades[powerIndex].increase);
            powerIndex++;
            audioSource.Play();

            if (powerIndex >= powerUpgrades.Count)
            {
                powerButton.SetActive(false);
            }
            else
            {
                UpdateText();
            }
        }
    }

    public void ExitPressed()
    {
        uiManager.ShowUpgradeScreen(false);
    }
}