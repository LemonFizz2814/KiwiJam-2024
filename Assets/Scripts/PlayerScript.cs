using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [Header("Players Variables")]
    [SerializeField] private float maxDust;
    [SerializeField] private float maxPower;
    [SerializeField] private float maxSpeed;
    [Space]
    [SerializeField] private float powerDepletion;
    [SerializeField] private float satOnSpeedDeduction;
    [SerializeField] private float stickySpeedDeduction;
    [SerializeField] private float lowPowerSpeedDeduction;
    [SerializeField] private float powerCharge;
    [SerializeField] private float suctionPower;

    [Header("Camera Variables")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float cameraDistance;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform camSetPos;

    [Header("Object References")]
    [SerializeField] private Transform model;
    [SerializeField] private Transform catSitPos;
    [SerializeField] private ParticleSystem dumpVFX;

    [Header("Sounds")]
    [SerializeField] private AudioSource vacuumAudio;
    [SerializeField] private AudioClip suckSFX;
    [SerializeField] private AudioClip slimeSFX;
    [SerializeField] private AudioClip rechargeSFX;
    [SerializeField] private AudioClip dumpSFX;
    [SerializeField] private AudioClip powerDownSFX;
    [SerializeField] private AudioClip powerUpSFX;
    [SerializeField] private AudioClip hissSFX;
    [SerializeField] private AudioClip meowSFX;
    [SerializeField] private AudioClip babySFX;

    // private variables
    private float dust;
    private float power;
    private float suction;
    private bool isCharging;
    //private float speed;

    private float mouseX;
    private float mouseY;

    private bool beingSatOn;
    private bool inUpgradeStation;
    private bool inStickySubstance;
    private bool lowPower, fullPower;
    private bool gameOver = false;

    // Upgrades
    [SerializeField] private GameObject Knife;

    private ParticleSystemForceField ps;

    private Rigidbody rb;
    private AudioSource audioSource;

    private Vector3 moveDirection;

    private UIManager uiManager;
    private CatScript catScript;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        uiManager = FindObjectOfType<UIManager>();

        //Upgrade Stuff
        Knife.SetActive(false);

        ps = transform.Find("Roomba").Find("Vacuum").GetComponent<ParticleSystemForceField>();
        suction = suctionPower;
        StartGame();
    }

    public void StartGame()
    {
        power = maxPower;
        uiManager.SetPowerSliderValue(power, maxPower);
        uiManager.SetDustValue(dust, maxDust);
    }

    void Update()
    {
        if (gameOver)
            return;

        //HandleCamera();
        Move();
        PowerManagement();
        ButtonCheck();
        CameraDistanceCheck();

        ps.gameObject.SetActive(dust < maxDust); // disable forcefield if dust is full
    }
    private void FixedUpdate()
    {
        //HandleCamera();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);

        moveDirection = new Vector3(0, 0, vertical);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection.y = 0;
        moveDirection.Normalize();

        float velocity = CalculateSpeedDeduction(maxSpeed);
        rb.velocity = moveDirection * velocity;

        if (horizontal != 0)
        {
            NewRotate();
        }

        if (rb.velocity.magnitude > 0.05f)
        {
            if(!isCharging)
            {
                PowerDepletion();
            }

            if (!vacuumAudio.isPlaying)
                vacuumAudio.Play();
        }
        else
        {
            vacuumAudio.Stop();
        }
    }
    void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        model.rotation = Quaternion.Slerp(model.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void NewRotate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);
    }
    void HandleCamera()
    {
        //mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        //mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        //mouseY = Mathf.Clamp(mouseY, 15, 50);

        //cameraTransform.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
        //cameraTransform.position = transform.position - cameraTransform.forward * cameraDistance;
    }

    float CalculateSpeedDeduction(float _speed)
    {
        if(beingSatOn)
        {
            _speed *= satOnSpeedDeduction;
        }
        if(inStickySubstance)
        {
            _speed *= stickySpeedDeduction;
        }
        if(lowPower)
        {
            _speed *= lowPowerSpeedDeduction;
        }

        return _speed;
    }
    void ButtonCheck()
    {
        if (Input.GetKeyDown(KeyCode.E) && inUpgradeStation)
        {
            uiManager.ShowUpgradeScreen(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Dump();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Dump()
    {
        if (dust != 0)
        {
            dust = 0;

            uiManager.SetPowerSliderValue(power, maxPower);
            uiManager.SetDustValue(dust, maxDust);

            audioSource.PlayOneShot(dumpSFX);

            dumpVFX.Play();

            if (beingSatOn && catScript != null)
            {
                catScript.HopOffPlayer(transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dust") && dust < maxDust)
        {
            CollectDust();
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Sticky"))
        {
            audioSource.PlayOneShot(slimeSFX);
            inStickySubstance = true;
        }
        else if(other.CompareTag("Recharge Station"))
        {
            audioSource.PlayOneShot(rechargeSFX);
        }
        else if(other.CompareTag("Upgrade Station"))
        {
            uiManager.ShowUpgradeStationPrompt(true);
            inUpgradeStation = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Recharge Station"))
        {
            isCharging = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Recharge Station"))
        {
            isCharging = false;
        }
        else if (other.CompareTag("Sticky"))
        {
            inStickySubstance = false;
        }
        else if (other.CompareTag("Upgrade Station"))
        {
            uiManager.ShowUpgradeStationPrompt(false);
            inUpgradeStation = false;
        }
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public void CollectDust()
    {
        if (dust < maxDust)
        {
            dust++;
            uiManager.SetDustValue(dust, maxDust);
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(suckSFX, 0.1f);
        }
    }

    private void CameraDistanceCheck()
    {
        float distance = Vector3.Distance(transform.position, camSetPos.position);
        Vector3 direction = (camSetPos.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, ~LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
        {
            cameraTransform.position = hit.point;
        }
        else
        {
            cameraTransform.position = camSetPos.position;
        }
    }

    void PowerManagement()
    {
        if (isCharging)
        {
            PowerRecharge();
        }
    }

    void PowerDepletion()
    {
        if (power > 0)
        {
            power -= Time.deltaTime * powerDepletion;
            lowPower = false;
            fullPower = false;
        }
        else
        {
            if(!lowPower)
            {
                lowPower = true;
                audioSource.PlayOneShot(powerDownSFX);
            }
        }

        uiManager.SetPowerSliderValue(power, maxPower);
        uiManager.ShowLowBatteryText(IsLowPower());
        uiManager.ShowBatteryGoneText(power <= 0);
    }

    void PowerRecharge()
    {
        if (power < maxPower)
        {
            power += Time.deltaTime * powerCharge;
        }
        else if(!fullPower)
        {
            fullPower = true;
            audioSource.PlayOneShot(powerUpSFX);
        }

        uiManager.SetPowerSliderValue(power, maxPower);
        uiManager.ShowLowBatteryText(IsLowPower());
    }

    public bool IsLowPower()
    {
        return power < (0.25 * maxPower);
    }

    public Transform GetCatSitPosition()
    {
        return catSitPos;
    }
    public void SetBeingSatOn(bool _beingSatOn, CatScript _catScript)
    {
        if (beingSatOn)
        { audioSource.PlayOneShot(meowSFX); }
        else
        { audioSource.PlayOneShot(hissSFX); }

        beingSatOn = _beingSatOn;
        catScript = _catScript;

        uiManager.ShowCatHelpText(beingSatOn);
    }

    public void SuctionIncreased(float _suction)
    {
        suction += _suction;
        ps.endRange = suction;
    }
    public void CapacityIncreased(float _capacity)
    {
        maxDust += _capacity;
        uiManager.SetDustValue(dust, maxDust);
    }
    public void PowerIncreased(float _power)
    {
        maxPower += _power;
        uiManager.SetPowerSliderValue(power, maxPower);
    }
    public void KnifePurchased()
    {
        Knife.SetActive(true);
    }

    public float GetDust()
    {
        return dust;
    }
    public void SetDust(float _dust)
    {
        dust = _dust;
    }
}