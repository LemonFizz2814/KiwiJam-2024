using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Players Variables")]
    [SerializeField] private float maxDust;
    [SerializeField] private float maxPower;
    [SerializeField] private float maxSpeed;
    [Space]
    [SerializeField] private float powerDepletion;
    [SerializeField] private float powerDumpLoss;
    [SerializeField] private float satOnSpeedDeduction;
    [SerializeField] private float stickySpeedDeduction;
    [SerializeField] private float lowPowerSpeedDeduction;
    [SerializeField] private float powerCharge;

    [Header("Camera Variables")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float cameraDistance;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform cameraTransform;

    [Header("Object References")]
    [SerializeField] private Transform model;
    [SerializeField] private Transform catSitPos;

    // private variables
    private float dust;
    private float power;
    private bool isCharging;
    //private float speed;

    private float mouseX;
    private float mouseY;

    private bool beingSatOn;
    private bool inStickySubstance;
    private bool lowPower;

    private Rigidbody rb;

    private Vector3 moveDirection;

    private UIManager uiManager;
    private CatScript catScript;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        uiManager = FindObjectOfType<UIManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        power = maxPower;
        uiManager.SetPowerSliderValue(power, maxPower);
        uiManager.SetDustValue(dust, maxDust);
    }

    void Update()
    {
        //HandleCamera();
        Move();
        PowerManagement();
        PowerDepletion();
        EjectButton();
    }
    private void FixedUpdate()
    {
        //HandleCamera();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection.y = 0;
        moveDirection.Normalize();

        float velocity = CalculateSpeedDeduction(maxSpeed);
        rb.velocity = moveDirection * velocity;
        Rotate();
    }
    void Rotate()
    {
        if (rb.velocity.magnitude > 0.05f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            model.rotation = Quaternion.Slerp(model.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
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

    void EjectButton()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            dust = 0;
            power -= powerDumpLoss;

            uiManager.SetPowerSliderValue(power, maxPower);
            uiManager.SetDustValue(dust, maxDust);

            if (beingSatOn && catScript != null)
            {
                catScript.HopOffPlayer(transform);
            }
            // TODO: spawn in dust particle cloud
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
            inStickySubstance = true;
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
    }

    void CollectDust()
    {
        dust++;
        uiManager.SetDustValue(dust, maxDust);
    }

    void PowerManagement()
    {
        if (!isCharging)
        {
            PowerDepletion();
        }
        else
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
        }
        else
        {
            lowPower = true;
        }

        uiManager.SetPowerSliderValue(power, maxPower);
    }

    void PowerRecharge()
    {
        if (power < maxPower)
        {
            power += Time.deltaTime * powerCharge;
        }

        uiManager.SetPowerSliderValue(power, maxPower);
    }

    public Transform GetCatSitPosition()
    {
        return catSitPos;
    }
    public void SetBeingSatOn(bool _beingSatOn, CatScript _catScript)
    {
        beingSatOn = _beingSatOn;
        catScript = _catScript;
    }
}