using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMode : MonoBehaviour
{

    public bool GameIsOver = false;

    private Animator anim;
    private float StopTimeScale = 0.01f;
    private float speed = 0.2f;
    private float lastDeltaTime;



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
            GameOver();
        }
    }

    void GameOver() {
        float myDeltaTime = Time.realtimeSinceStartup - lastDeltaTime;
        lastDeltaTime = Time.realtimeSinceStartup;
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, StopTimeScale, myDeltaTime * speed);
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        anim.SetTrigger("GameOver");
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene("Scene1");
    }
}
