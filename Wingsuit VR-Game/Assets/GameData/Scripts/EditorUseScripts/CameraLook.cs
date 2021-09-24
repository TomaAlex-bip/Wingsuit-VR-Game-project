using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] private float sensitivity;
    [SerializeField] private float smoothFactor = 0.1f;

    bool cameralock;

    float rotX;
    float rotY;
    float rotZ;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            cameralock = !cameralock;
        }

        if (cameralock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(cameralock)
        {
            Rotate();
        }

        
    }


    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float tilt = Input.GetAxis("Horizontal");

        rotX = Mathf.Lerp(rotX, -mouseY * sensitivity, smoothFactor);
        rotY = Mathf.Lerp(rotY, mouseX * sensitivity, smoothFactor);
        rotZ = Mathf.Lerp(rotZ, -tilt * sensitivity, smoothFactor);

        transform.Rotate(new Vector3(rotX, rotY, rotZ) * Time.deltaTime);
    }

#endif
}
