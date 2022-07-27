using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PURPOSE: Rotates camera with mouse movement.
//   USAGE: Attach to camera.

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private PlayerBunny pb;
    private float xRotation = 0f;
    private float mouseX = 0f;
    private float mouseY = 0f;

    void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        pb = playerBody.GetComponent<PlayerBunny>();
    }

    void Update()
    {
        // Find mouse position and adjust by sensitivity and frame rate
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; // decreases xRotation based on mouse position
        
        // stops player from looking all the way above/below them
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // apply rotation to camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // apply rotation to player
        playerBody.Rotate(Vector3.up * mouseX); 

    }
}