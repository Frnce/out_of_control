using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates
{
    GAME,
    PAUSE,
    GAMEOVER
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            return null;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    [HideInInspector]
    public GameStates gameStates;

    public GameObject gameOverCanvas;
    public GameObject timeCanvas;
    public GameObject pauseCanvas;
    public GameObject faderCanvas;
    private GameObject gameoverCanvasinstance;
    private GameObject timeCanvasInstance;
    private GameObject pauseCanvasInstance;

    private bool timeStarted;
    private float timeSurvived;
    // Start is called before the first frame update
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    void Start()
    {
        //Temp
        gameStates = GameStates.GAME;

        Instantiate(faderCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameStates)
        {
            case GameStates.GAME:
                OnGameBehavior();
                break;
            case GameStates.PAUSE:
                PauseBehavior();
                break;
            case GameStates.GAMEOVER:
                GameOverBehavior();
                //Show UI
                break;
            default:
                break;
        }
    }
    public void ChangeState(GameStates state)
    {
        gameStates = state;
    }
    public void StartTime()
    {
        timeStarted = true;
        timeCanvasInstance.SetActive(true);
    }
    public string TimeSurvived()
    {
        string minutes = Mathf.Floor(timeSurvived / 60).ToString("00");
        string seconds = (timeSurvived % 60).ToString("00");
        return string.Format("{0}:{1}", minutes, seconds);
    }

    private void OnGameBehavior()
    {
        if (timeStarted)
        {
            timeSurvived += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameStates = GameStates.PAUSE;
            pauseCanvasInstance.SetActive(true);
        }
    }
    private void PauseBehavior()
    {
        //show UI
        Time.timeScale = 0f;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameStates = GameStates.GAME;
            pauseCanvasInstance.SetActive(false);
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
    private void GameOverBehavior()
    {
        timeStarted = false;
        gameoverCanvasinstance.SetActive(true);
    }
    public void RestartGame()
    {
        timeSurvived = 0f;
        gameoverCanvasinstance.SetActive(false);
        ChangeState(GameStates.GAME); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart");
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //IsGameOver = false;
        //IsPaused = false;
        //checkGameOverFunctionWasCalled = false;

        Scene currentScene = SceneManager.GetActiveScene();

        int sceneBuildIndex = currentScene.buildIndex;
        if (sceneBuildIndex == 1)
        {
            Time.timeScale = 1f;

            gameoverCanvasinstance = Instantiate(gameOverCanvas);
            gameoverCanvasinstance.SetActive(false);

            timeCanvasInstance = Instantiate(timeCanvas);
            timeCanvasInstance.SetActive(false);

            pauseCanvasInstance = Instantiate(pauseCanvas);
            pauseCanvasInstance.SetActive(false);
        }
    }
}
