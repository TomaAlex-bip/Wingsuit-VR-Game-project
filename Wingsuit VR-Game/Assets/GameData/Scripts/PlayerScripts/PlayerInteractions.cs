using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private LayerMask layer;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        OnButtonLook();
    }

    private void OnButtonLook()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layer))
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
