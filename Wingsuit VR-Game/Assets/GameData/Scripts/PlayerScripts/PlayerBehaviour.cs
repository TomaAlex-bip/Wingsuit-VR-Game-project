using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    public static PlayerBehaviour Instance { get; private set; }

    


    private GameManager gameManager;

    private Rigidbody rb;

    private bool gameOver;


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
        if (gameOver)
        {
            return;
        }
        
        gameOver = true;

        rb.useGravity = true;

        rb.drag = 0.5f;
        rb.mass = 10f;

        rb.AddForce(transform.forward * 5000f);

        gameManager.RestartGame(3f);
        
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "Respawn":
                PlayerMovement.Instance.GameOverState();
                break;
            
            
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "ScorePoint":
                //print("am adaugat la scor");
                gameManager.AddScore();
                Destroy(other.transform.parent.gameObject);
                break;
        }
    }
}
