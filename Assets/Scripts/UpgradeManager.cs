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
    [SerializeField] private List<Upgrade> knifeUpgrades = new List<Upgrade>();
    [Space]
    [SerializeField] private TextMeshProUGUI suctionPriceText;
    [SerializeField] private TextMeshProUGUI capacityPriceText;
    [SerializeField] private TextMeshProUGUI powerPriceText;
    [SerializeField] private TextMeshProUGUI knifePriceText;
    [Space]
    [SerializeField] private TextMeshProUGUI suctionIncreaseText;
    [SerializeField] private TextMeshProUGUI capacityIncreaseText;
    [SerializeField] private TextMeshProUGUI powerIncreaseText;
    [SerializeField] private TextMeshProUGUI knifeIncreaseText;
    [Space]
    [SerializeField] private TextMeshProUGUI suctionLevelText;
    [SerializeField] private TextMeshProUGUI capacityLevelText;
    [SerializeField] private TextMeshProUGUI powerLevelText;
    [SerializeField] private TextMeshProUGUI knifeLevelText;
    [Space]
    [SerializeField] private TextMeshProUGUI dustText;
    [Space]
    [SerializeField] private GameObject suctionButton;
    [SerializeField] private GameObject capacityButton;
    [SerializeField] private GameObject powerButton;
    [SerializeField] private GameObject knifeButton;
    [Space]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerScript playerScript;

    private AudioSource audioSource;

    private int suctionIndex = 0;
    private int capacityIndex = 0;
    private int powerIndex = 0;
    private int knifeIndex = 0;

    [Serializable]
    public struct Upgrade
    {
        public float cost;
        public float increase;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (playerScript == null)
        {
            playerScript = FindObjectOfType<PlayerScript>();
        }

        suctionButton.SetActive(true);
        capacityButton.SetActive(true);
        powerButton.SetActive(true);
        knifeButton.SetActive(true);

        UpdateAllText();
    }

    public void UpdateAllText()
    {
        if (suctionIndex < suctionUpgrades.Count)
            UpdateSuctionText();
        if (capacityIndex < capacityUpgrades.Count)
            UpdateCapacityText();
        if (powerIndex < powerUpgrades.Count)
            UpdatePowerText();
        if (knifeIndex < knifeUpgrades.Count)
            UpdateKnifeText();
    }

    public void UpdateSuctionText()
    {
        suctionPriceText.text = suctionUpgrades[suctionIndex].cost + " Dust";
        suctionIncreaseText.text = $"+{suctionUpgrades[suctionIndex].increase} Suck";
        suctionLevelText.text = $"Upgrade suction\n\nLvl. {suctionIndex}";

        if (playerScript != null)
            dustText.text = $"Dust: {playerScript.GetDust()}";
    }
    public void UpdateCapacityText()
    {
        capacityPriceText.text = capacityUpgrades[capacityIndex].cost + " Dust";
        capacityIncreaseText.text = $"+{capacityUpgrades[capacityIndex].increase} Max Dust";
        capacityLevelText.text = $"Upgrade capacity\n\nLvl. {capacityIndex}";

        if (playerScript != null)
            dustText.text = $"Dust: {playerScript.GetDust()}";
    }
    public void UpdatePowerText()
    {
        powerPriceText.text = powerUpgrades[powerIndex].cost + " Dust";
        powerIncreaseText.text = $"+{powerUpgrades[powerIndex].increase} Max Power";
        powerLevelText.text = $"Upgrade power\n\nLvl. {powerIndex}";

        if (playerScript != null)
            dustText.text = $"Dust: {playerScript.GetDust()}";
    }
    public void UpdateKnifeText()
    {
        knifePriceText.text = knifeUpgrades[knifeIndex].cost + " Dust";
        knifeIncreaseText.text = $"+{knifeUpgrades[knifeIndex].increase} Knife";
        knifeLevelText.text = $"Buy knife\n\nLvl. {knifeIndex}";

        if (playerScript != null)
            dustText.text = $"Dust: {playerScript.GetDust()}";
    }

    public void UpgradeSuctionPressed()
    {
        if (playerScript.GetDust() >= suctionUpgrades[suctionIndex].cost)
        {
            float newDust = playerScript.GetDust() - suctionUpgrades[suctionIndex].cost;
            playerScript.SetDust(newDust);
            playerScript.SuctionIncreased(suctionUpgrades[suctionIndex].increase);
            suctionIndex++;
            audioSource.Play();

            if (suctionIndex >= suctionUpgrades.Count)
            {
                suctionButton.SetActive(false);
                dustText.text = $"Dust: {playerScript.GetDust()}";
            }
            else
            {
                UpdateSuctionText();
            }
        }
    }
    public void UpgradeCapacityPressed()
    {
        if (playerScript.GetDust() >= capacityUpgrades[capacityIndex].cost)
        {
            float newDust = playerScript.GetDust() - capacityUpgrades[capacityIndex].cost;
            playerScript.SetDust(newDust);
            playerScript.CapacityIncreased(capacityUpgrades[capacityIndex].increase);
            capacityIndex++;
            audioSource.Play();

            if (capacityIndex >= capacityUpgrades.Count)
            {
                capacityButton.SetActive(false);
                dustText.text = $"Dust: {playerScript.GetDust()}";
            }
            else
            {
                UpdateCapacityText();
            }
        }
    }
    public void UpgradePowerPressed()
    {
        if (playerScript.GetDust() >= powerUpgrades[powerIndex].cost)
        {
            float newDust = playerScript.GetDust() - powerUpgrades[powerIndex].cost;
            playerScript.SetDust(newDust);
            playerScript.PowerIncreased(powerUpgrades[powerIndex].increase);
            powerIndex++;
            audioSource.Play();

            if (powerIndex >= powerUpgrades.Count)
            {
                powerButton.SetActive(false);
                dustText.text = $"Dust: {playerScript.GetDust()}";
            }
            else
            {
                UpdatePowerText();
            }
        }
    }
    public void UpgradeKnifePressed()
    {
        if (playerScript.GetDust() >= knifeUpgrades[knifeIndex].cost)
        {
            float newDust = playerScript.GetDust() - knifeUpgrades[knifeIndex].cost;
            playerScript.SetDust(newDust);
            playerScript.KnifePurchased();
            knifeIndex++;
            audioSource.Play();

            if (knifeIndex >= knifeUpgrades.Count)
            {
                knifeButton.SetActive(false);
                dustText.text = $"Dust: {playerScript.GetDust()}";
            }
            else
            {
                UpdateKnifeText();
            }
        }
    }

    public void ExitPressed()
    {
        uiManager.ShowUpgradeScreen(false);
    }
}