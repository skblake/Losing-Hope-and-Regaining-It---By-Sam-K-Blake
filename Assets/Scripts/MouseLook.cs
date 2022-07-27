using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PURPOSE: Rotates camera with mouse movement.
//   USAGE: Attach to camera with a parent Pivot object. 

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public bool camHasTwoAxes = false;
    public bool tempGabeCamera = false;
    public Transform playerBody;
    public Transform playerGraphic;
    public PlayerBunny bunnyScript;
    public Transform pivot;
    private float xRotation = 0f;
    private float yRotation = 0f; 
    private float mouseX = 0f;
    private float mouseY = 0f;

    void Start() 
    {
        pivot = transform.parent.gameObject.transform;
        Cursor.lockState = CursorLockMode.Locked;
        // bunnyScript = playerBody.GetComponent<PlayerBunny>();
    }

    void Update()
    {
        if (camHasTwoAxes) {

            TwoAxisCamera();
            
        } else if (tempGabeCamera) {

            TempGabeCamera();

        } else {
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

    void TwoAxisCamera() 
    {
        // Find mouse position and adjust by sensitivity and frame rate
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; // decreases xRotation based on mouse position
        yRotation += mouseX / 2; 
        
        // stops player from looking all the way above/below them
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // apply rotation to camera
        bunnyScript.GetComponent<Transform>().localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        // bunnyScript.GetComponent<Transform>().Rotate = (Vector3.up * mouseX);
        // bunnyScript.GetComponent<Transform>().localRotation = Quaternion.Euler(0f, yRotation, 0f);

        // apply rotation to player
        bunnyScript.GetComponent<Transform>().Rotate(Vector3.up * mouseX);
        // bunnyScript.GetComponent<Transform>().Rotate(Vector3.right * mouseY); 

        // float z = pivot.eulerAngles.z;
        

        if (bunnyScript.isMoving) {
            // apply rotation to player graphic
            playerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        } else {
            // pivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // apply rotation to camera
        // transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // pivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // apply rotation to player
        // playerBody.Rotate(Vector3.up * mouseX); 




        /*
                // apply rotation to camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        bunnyScript.GetComponent<Transform>().rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        pivot.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        */
    }

    public Transform m_GameManager;
    
    public float xSpeed = 20.0f;
    public float ySpeed = 80.0f;

    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    private float distance = 5f; //needs to be based off of game size (how many cubes), not random number.
    private float distanceMin = 4f;
    private float distanceMax = 15f;

    private new Rigidbody rigidbody;
    private Quaternion prevRot;
    private float prevDistance;

    float x = 0.0f;
    float y = 0.0f;

    void TempGabeCamera() 
    {
        if (Vector3.Distance(this.transform.position, m_GameManager.position) <= distance)
        {
            CameraTranslateOut(); //while the camera is inside the game area, move it backwards until it reaches the default distance. 
        }        

        Quaternion rotation = this.transform.rotation;

        if (m_GameManager && (Input.GetButton("Fire2")))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            rotation = Quaternion.Euler(y, x, 0);

        }
        
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        if(distance != prevDistance || rotation != prevRot) //if distance or rotation has changed, then run set pos/rot
        {
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + m_GameManager.position;

            transform.rotation = rotation;
            transform.position = position;

            prevRot = rotation;
            prevDistance = distance;
        }
    }

    public static float ClampAngle(float angle, float min, float max) //restricts an angle based off of a minimum and maximum. prevents angle from going over 360 or under 0.
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    private void CameraTranslateOut()
    {
        for (int i = 0; i < 300; i++)
        {
            this.transform.Translate((Vector3.forward * -1) * (Time.deltaTime * 0.02f)); //Slowly pans backwards
        }        
    }
}