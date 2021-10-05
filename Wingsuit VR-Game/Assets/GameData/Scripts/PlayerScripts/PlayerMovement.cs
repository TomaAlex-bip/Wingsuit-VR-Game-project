using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [SerializeField] private float movementSpeed;

    [SerializeField] private Vector2 minContraints;
    [SerializeField] private Vector2 maxContraints;

    [SerializeField] private bool limitRotation;
    [SerializeField] private float maxRotation;

    [SerializeField] private float rotX;
    [SerializeField] private float rotY;

    PlayerBehaviour playerBehaviour;
    GameManager gameManager;

    float speed;
    Transform cam;

    bool inSettings = false;

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
        gameManager = GameManager.Instance;
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
        rotX = cam.localRotation.eulerAngles.x;
        rotY = cam.localRotation.eulerAngles.y;
        if (cam.localRotation.eulerAngles.x >= 180f)
        {
            rotX = cam.localRotation.eulerAngles.x - 360f;
        }
        if(cam.localRotation.eulerAngles.y >= 180f)
        {
            rotY = cam.localRotation.eulerAngles.y - 360f;
        }

        // useless, nu poate sa limiteze rotatia
        //float rotX = Mathf.Clamp(processedRotationX, -maxRotation, maxRotation);
        //float rotY = Mathf.Clamp(processedRotationY, -maxRotation, maxRotation);
        //Quaternion rot = Quaternion.Euler(rotX, rotY, cam.transform.localRotation.eulerAngles.z);


        if(limitRotation)
        {
            if (rotX <= -maxRotation || rotY <= -maxRotation || rotY >= maxRotation)
            {
                // deal with the state where you look back
                if(!inSettings)
                {
                    GameOverState();
                }
            }
            else if (rotX >= maxRotation)
            {
                SettingsState();
            }
            else
            {
                // keep things normal
                ContinueState();
            }


            //cam.transform.localRotation = rot;
        }
    }

    private void SettingsState()
    {
        inSettings = true;
        speed = 0f;

        var restart = gameManager.UI.restartButton;
        restart.SetActive(true);
        restart.transform.position = new Vector3(transform.position.x, transform.position.y - 0.49f, transform.position.z- 0.25f);
        var rot = Quaternion.Euler(110f, restart.transform.rotation.y, restart.transform.rotation.z);
        restart.transform.rotation = rot;
    }

    public void GameOverState()
    {
        var playerMovement = gameObject.GetComponent<PlayerMovement>();
        //speed = 0f;
        playerBehaviour.GameOverState();
        playerMovement.enabled = false;
    }

    private void ContinueState()
    {
        inSettings = false;
        var restart = gameManager.UI.restartButton;
        restart.SetActive(false);
        speed = movementSpeed;
    }


}
