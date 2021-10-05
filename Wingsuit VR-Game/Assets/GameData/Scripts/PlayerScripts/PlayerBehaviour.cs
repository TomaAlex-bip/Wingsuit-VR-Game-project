using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    public static PlayerBehaviour Instance { get; private set; }

    public bool FinishedJumpAnimation { get; private set; }


    GameManager gameManager;

    Rigidbody rb;

    bool gameOver;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance!= this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        gameManager = GameManager.Instance;
    }





    public void GameOverState()
    {
        if(!gameOver)
        {
            gameOver = true;

            rb.useGravity = true;

            rb.drag = 0.5f;
            rb.mass = 10f;

            rb.AddForce(transform.forward * 5000f);

            gameManager.RestartGame(3f);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Respawn"))
        {
            PlayerMovement.Instance.GameOverState();

        }
    }





}
