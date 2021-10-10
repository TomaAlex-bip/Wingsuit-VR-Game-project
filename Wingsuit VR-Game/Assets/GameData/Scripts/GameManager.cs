using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UIButtons UI { get => ui; }

    [SerializeField] private UIButtons ui;

    [SerializeField] private bool gameHasStarted;

    [SerializeField] private GameObject player;

    [SerializeField] private Text scoreText;


    private int score = 0;

    private Animator playerAnim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {
        playerAnim = player.GetComponentInParent<Animator>();
    }

    private void Update()
    {
        
    }


    public void StartGame()
    {
        if(!gameHasStarted)
        {
            gameHasStarted = true;
            playerAnim.SetTrigger("jump");
            StartCoroutine(StartGameCoroutine());
            Destroy(ui.playButton);
        }
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void RestartGame() => SceneManager.LoadScene(0);

    public void RestartGame(float waitTime) => StartCoroutine(RestartGameCoroutine(waitTime));

    private IEnumerator RestartGameCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        RestartGame();
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(2f);
        var playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        
    }


}


[System.Serializable]
public class UIButtons
{
    public GameObject playButton;
    public GameObject restartButton;
    public GameObject Button2;
    public GameObject Button3;
}
