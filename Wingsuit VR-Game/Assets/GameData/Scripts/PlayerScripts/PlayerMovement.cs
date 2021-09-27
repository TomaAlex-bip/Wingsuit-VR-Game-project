using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;

    [SerializeField] private Vector2 minContraints;
    [SerializeField] private Vector2 maxContraints;

    [SerializeField] private bool limitRotation;
    [SerializeField] private float maxRotation;


    Transform cam;

    private void Start()
    {
        cam = transform.Find("Main Camera");
    }

    private void Update()
    {
        // move the player forward
        transform.position += cam.transform.forward * movementSpeed * Time.deltaTime;


        // limit the movement space
        float x = Mathf.Clamp(transform.localPosition.x, minContraints.x, maxContraints.x);
        float y = Mathf.Clamp(transform.localPosition.y, minContraints.y, maxContraints.y);
        transform.localPosition = new Vector3(x, y, transform.localPosition.z);


        // limit the rotation

        float processedRotationX = cam.localRotation.eulerAngles.x;
        float processedRotationY = cam.localRotation.eulerAngles.y;
        if (cam.localRotation.eulerAngles.x >= 180f)
        {
            processedRotationX = cam.localRotation.eulerAngles.x - 360f;
        }
        if(cam.localRotation.eulerAngles.y >= 180f)
        {
            processedRotationY = cam.localRotation.eulerAngles.y - 360f;
        }

        float rotX = Mathf.Clamp(processedRotationX, -maxRotation, maxRotation);
        float rotY = Mathf.Clamp(processedRotationY, -maxRotation, maxRotation);
        Quaternion rot = Quaternion.Euler(rotX, rotY, cam.transform.localRotation.eulerAngles.z);

        if(limitRotation)
        {
            cam.transform.localRotation = rot;
        }
    }

}
