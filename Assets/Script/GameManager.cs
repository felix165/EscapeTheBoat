using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float timeLimit=600f;
    public static float timeLeft = 600f;

    public static int score = 0;
    public int completeScore = 200;
    public int moveBonusScore = 10;
    public int failScore = 1;

    private int tempScore = 0;
    private float tempDelay = 0;
    private bool isCountScore = false;

    private ArrayList loadedScene = new ArrayList();
    private int minLevelScene = 2;
    private int maxLevelScene = 6;
    //private int mainMenuScene = 0;
    private int tutorialScene = 1;
    private int gameOverScene = 11;

    public static bool isGameOver = false;
    bool isGameStart = false;
    public static bool isGamePause = false;



    // Start is called before the first frame update
    void Start()
    {

        //NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStart)
        {
            if (timeLeft <= 0)
            {
                //GameOver
                isGameOver = true;
                score = tempScore;
                NextLevel();

            }
            else
            {
                //Score Animation
                if (isCountScore)
                {
                    if (tempScore != score)
                    {
                        tempDelay += Time.deltaTime;
                        if (tempDelay >= 0.01f)
                        {
                            tempDelay = 0f;
                            score += 1;
                        }
                    }
                    else
                    {
                        NextLevel();
                    }
                }
                else
                {
                    timeLeft -= Time.deltaTime;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingsWindow.Instance.setActive(!SettingsWindow.Instance.getActiveSelf());
            if (!SettingsWindow.Instance.getActiveSelf())
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }

    }
    //NewGame
    public void NewGame()
    {
        timeLeft = timeLimit;
        score = 0;
        tempScore = score;
        loadedScene.Clear();
        isGameOver = false;

        if(SceneManager.GetActiveScene().buildIndex == tutorialScene || SceneManager.GetActiveScene().buildIndex == gameOverScene)
        {
            NextLevel();
        }
        isGameStart= true;        
    }
    
    //NextLevel
    public void NextLevel()
    {
        int sceneIndex = 0;
        if (isGameOver)
        {
            sceneIndex = gameOverScene;
        }
        else
        {
            SoundManager.Instance.PlaySFX("NextLevel");
            sceneIndex = RandomLevel(minLevelScene, maxLevelScene);
            loadedScene.Add(sceneIndex);
            isCountScore = false;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadTutorialScene()
    {
        SceneManager.LoadScene(tutorialScene);
    }

    private int RandomLevel(int min, int max)
    {
        int sceneIndex = 0;
        bool flag = true;
        while(flag)
        {
            sceneIndex = (int)Random.Range(min, max);
            flag=loadedScene.Contains(sceneIndex);
        }
        return sceneIndex;
    }


    public void levelComplete(int moveLeft=0)
    {
        if (!isGameOver)
        {
            isCountScore = true;
            tempScore += completeScore;
            tempScore += (moveLeft * moveBonusScore);
        }
    }
    public void OutOfMove()
    {
        if (!isGameOver)
        {
            isCountScore = true;
            tempScore += completeScore;
            tempScore += failScore;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        isGamePause= true;
        Debug.Log("pause");
        
    }
    public void resumeGame()
    {
        Time.timeScale = 1.0f;
        isGamePause= false;
        Debug.Log("Resume");
    }






}
