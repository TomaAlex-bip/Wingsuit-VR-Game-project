using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [SerializeField] private float movementSpeed;

    [SerializeField] private Vector2 minContraints;
    [SerializeField] private Vector2 maxContraints;

    [SerializeField] private bool limitRotation;
    [SerializeField] private float maxRotation;

    PlayerBehaviour playerBehaviour;

    float speed;
    Transform cam;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        cam = transform.Find("Main Camera");
        speed = movementSpeed;
        playerBehaviour = PlayerBehaviour.Instance;
    }

    private void Update()
    {
        // move the player forward
        transform.position += cam.transform.forward * speed * Time.deltaTime;


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
            if (rotX <= -maxRotation || rotX >= maxRotation || rotY <= -maxRotation || rotY >= maxRotation)
            {
                // deal with the state where you look back
                GameOverState();
            }
            else
            {
                // keep things normal
                //ContinueState();
            }


            //cam.transform.localRotation = rot;
        }
    }


    public void GameOverState()
    {
        speed = 0f;
        playerBehaviour.GameOverState();
    }

    private void ContinueState()
    {
        speed = movementSpeed;
    }


}
