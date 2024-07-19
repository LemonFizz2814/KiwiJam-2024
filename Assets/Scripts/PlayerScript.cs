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
    [Header("Camera Variables")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float cameraDistance;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform model;

    // private variables
    private float dust;
    private float power;
    //private float speed;

    private Rigidbody rb;

    private float mouseX;
    private float mouseY;

    private Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCamera();
    }
    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection.y = 0;
        moveDirection.Normalize();

        rb.velocity = moveDirection * maxSpeed;
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
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseY = Mathf.Clamp(mouseY, 15, 50);

        cameraTransform.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
        cameraTransform.position = transform.position - cameraTransform.forward * cameraDistance;
    }
}