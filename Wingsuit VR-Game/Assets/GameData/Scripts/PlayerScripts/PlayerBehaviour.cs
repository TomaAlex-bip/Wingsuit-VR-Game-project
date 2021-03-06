using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    public static PlayerBehaviour Instance { get; private set; }

    [SerializeField] private GameObject scoreParticle;
    [SerializeField] private GameObject playerExplosionParticle;


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

        gameManager.RestartGame(5f);
        
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "Respawn":
                PlayerMovement.Instance.GameOverState();
                Instantiate(playerExplosionParticle, collision.GetContact(0).point, Quaternion.identity);
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
                Instantiate(scoreParticle, other.transform.position, Quaternion.identity);
                Destroy(other.transform.parent.gameObject);
                break;
        }
    }
}
