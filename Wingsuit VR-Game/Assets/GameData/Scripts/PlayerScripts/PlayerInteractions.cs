using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private LayerMask layer;

    [SerializeField] private float timeToActivate = 2f;
    [SerializeField] private Transform gazeCrosshair;
    [SerializeField] private float size;
    [SerializeField] private Vector3 size2;

    GameManager gameManager;

    float initialSize;
    bool gazeStatus;
    float gazeTimer;

    private void Start()
    {
        gameManager = GameManager.Instance;
        initialSize = gazeCrosshair.localScale.x;
    }

    private void Update()
    {
        UpdateGazeTimer();
        OnButtonLook();
    }

    private void UpdateGazeTimer()
    {
        if(OnRayHit())
        {
            GazeOn();
        }
        else
        {
            GazeOff();
        }

        if(gazeStatus)
        {
            gazeTimer += Time.deltaTime;
            size = gazeTimer / timeToActivate;
        }

        if(size > 1f)
        {
            size = 1f;
        }

        gazeCrosshair.localScale = Vector3.one * size * initialSize;
        size2 = Vector3.one * size;
    }

    public void GazeOn()
    {
        gazeStatus = true;
    }

    public void GazeOff()
    {
        gazeStatus = false;
        gazeTimer = 0f;
        size = 0f;
    }

    private bool OnRayHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnButtonLook()
    {
        RaycastHit hit;
        if(size >= 1f && Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layer))
        {
            //print(hit);
            switch(hit.collider.tag)
            {
                case "Play":
                    gameManager.StartGame();
                    break;

                case "Restart":
                    gameManager.RestartGame();
                    break;

                default:
                    break;
            }
        }
    }

}
