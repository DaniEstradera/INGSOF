using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMode : MonoBehaviour
{

    public bool GameIsOver = false;
    public string StateOfTheGame;

    private Animator anim;
    private float StopTimeScale = 0.01f;
    private float speed = 0.2f;
    private float lastDeltaTime;
    private float restartDelay = 3;


    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        lastDeltaTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsOver) {
            GameOver(StateOfTheGame);
        }
    }

    void GameOver(string reason) {
        anim.SetTrigger(reason);
        restartDelay -= Time.deltaTime;
        if (restartDelay <= 0) {
            RestartLevel();
        }
    }

    public void RestartLevel ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
