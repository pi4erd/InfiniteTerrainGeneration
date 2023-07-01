using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CameraController : MonoBehaviour
{
    [Header("Axis settings")]
    [SerializeField] string horizontalAxis = "Horizontal";
    [SerializeField] string verticalAxis = "Vertical";
    [SerializeField] string mouseXAxis = "Mouse X";
    [SerializeField] string mouseYAxis = "Mouse Y";
    [SerializeField] string mouseWheelAxis = "Mouse ScrollWheel";

    [Header("Control parameters")]
    [SerializeField] float speed = 1;
    [SerializeField] float accelerationSpeed = 1.1f;
    [SerializeField] float sensitivity = 1;

    public bool controlsEnabled = true;

    [Header("UI")]
    [SerializeField] TMP_Text speedText;

    private CharacterController characterController;

    private float mouseX, mouseY, mw;
    private Vector3 movement;
    private float angleX, angleY;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void GetInput() 
    {
        mouseX = Input.GetAxis(mouseXAxis);
        mouseY = Input.GetAxis(mouseYAxis);
        mw = Input.GetAxis(mouseWheelAxis);

        float hi = Input.GetAxis(horizontalAxis);
        float vi = Input.GetAxis(verticalAxis);

        if(controlsEnabled)
        {
            movement = transform.forward * vi + transform.right * hi;

            angleX -= mouseY * sensitivity;
            angleY += mouseX * sensitivity;

            angleX = Mathf.Clamp(angleX, -89, 89);
        } else
        {
            movement = Vector3.zero;

            mw = 0;
        }
    }

    private void Update()
    {
        GetInput();

        speed *= 1 + accelerationSpeed * mw; // This formula surprisingly works!

        transform.rotation = Quaternion.Euler(angleX, angleY, 0);
        characterController.Move(speed * Time.deltaTime * movement);

        speedText.text = $"Camera speed: {speed}ups";
    }
}
