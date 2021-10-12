using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [SerializeField] private float movementSpeed;

    [SerializeField] private GameObject speedParticles;
    
    [SerializeField] private Vector2 minContraints;
    [SerializeField] private Vector2 maxContraints;

    [SerializeField] private bool limitRotation;
    [SerializeField] private float maxRotation;

    [SerializeField] private float rotX;
    [SerializeField] private float rotY;

    
    private PlayerBehaviour playerBehaviour;
    private GameManager gameManager;

    private float speed;
    private float speedMultiplier = 1f;
    private Transform cam;

    private Camera mainCam;
    private float initialFOV;

    private bool inSettings = false;

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
        
        mainCam = cam.GetComponent<Camera>();
        initialFOV = mainCam.fieldOfView;
        

    }

    private void Update()
    {
        // move the player forward
        var playerTransform = transform;
        playerTransform.position += cam.transform.forward * speed * Time.deltaTime;


        // limit the movement space
        var localPosition = playerTransform.localPosition;
        var x = Mathf.Clamp(localPosition.x, minContraints.x, maxContraints.x);
        var y = Mathf.Clamp(localPosition.y, minContraints.y, maxContraints.y);

        // var listOfVisibleChunks = EndlessMapGenerator.Instance.VisibleChunks.Values;
        // foreach (var chunk in listOfVisibleChunks)
        // {
        //     var wallsParent = chunk.transform.Find("Walls");
        //     //var wallsRend = new List<Renderer>();
        //     for (int i = 0; i < 4; i++)
        //     {
        //         var wallRend = wallsParent.GetChild(i).GetComponent<Renderer>();
        //         var currentColor = wallRend.material.color;
        //         currentColor.a = 1f;
        //         wallRend.sharedMaterial.color = currentColor;
        //     }
        //     
        //     
        // }

        var position = transform.localPosition;


        var xDist = maxContraints.x - Mathf.Abs(x);

        var yMed = (maxContraints.y + minContraints.y) / 2f;
        var yDist = maxContraints.y - yMed - (y-yMed);

        var distanceFromWalls = Mathf.Min(xDist, yDist);

        var alfaMat = 1f - Mathf.InverseLerp(0f, 4.25f, distanceFromWalls);
        
        EndlessMapGenerator.Instance.wallsMaterial.color = new Color(1f, 0f, 0f, alfaMat);
        
        transform.localPosition = new Vector3(x, y, localPosition.z);


        // limit the rotation
        var localRotation = cam.localRotation;
        rotX = localRotation.eulerAngles.x;
        rotY = localRotation.eulerAngles.y;
        if (cam.localRotation.eulerAngles.x >= 180f)
        {
            rotX = cam.localRotation.eulerAngles.x - 360f;
        }
        if(cam.localRotation.eulerAngles.y >= 180f)
        {
            rotY = cam.localRotation.eulerAngles.y - 360f;
        }

        // useless, nu poate sa limiteze rotatia in modul VR
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
        
        var playerTransform = transform.position;
        restart.transform.position = new Vector3(playerTransform.x, playerTransform.y - 0.49f, playerTransform.z - 0.25f);

        var restartRotation = restart.transform.rotation;
        var rot = Quaternion.Euler(110f, restartRotation.y, restartRotation.z);
        restart.transform.rotation = rot;
        
        speedParticles.SetActive(false);
    }

    public void GameOverState()
    {
        var playerMovement = gameObject.GetComponent<PlayerMovement>();
        //speed = 0f;
        playerBehaviour.GameOverState();
        playerMovement.enabled = false;
        
        speedParticles.SetActive(false);
    }

    private void ContinueState()
    {
        inSettings = false;
        var restart = gameManager.UI.restartButton;
        restart.SetActive(false);
        speedMultiplier = 1f + (rotX/1.25f) / 100f;
        speed = movementSpeed * speedMultiplier;

        mainCam.fieldOfView = initialFOV * speedMultiplier;

        if (speedMultiplier > 1f)
        {
            speedParticles.SetActive(true);
        }
        else
        {
            speedParticles.SetActive(false);
        }
        
    
        

    }


}
