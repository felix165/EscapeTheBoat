using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float timeLimit=600f;
    public static float timeLeft = 600f;

    public static string playerName = "Anonymous";
    public static int score = 0;
    public int completeScore = 200;
    public int moveBonusScore = 10;
    public int failScore = 50;
    private int incAmount = 20;

    private int tempScore = 0;
    private float tempDelay = 0;
    private bool isCountScore = false;

    private ArrayList loadedScene = new ArrayList();
    private int minLevelScene = 3;
    private int maxLevelScene = 32;
    //private int mainMenuScene = 0;
    private int tutorialScene = 1;
    private int gameOverScene = 2;

    public static bool isGameOver = false;
    bool isGameStart = false;
    public static bool isGamePause = false;

    public static bool isInputEnabled = true;



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
                GameOver();

            }
            else
            {
                //Score Animation
                if (isCountScore)
                {
                    if (tempScore != score)
                    {
                        tempDelay += Time.deltaTime;
                        if (tempDelay >= 0.35f)
                        {
                            tempDelay = 0f;
                            if (score + incAmount > tempScore)
                            {
                                score = tempScore;
                            }
                            else
                            {
                                score += incAmount;
                            }
                            SoundManager.Instance.PlaySFX("Score");
                            
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
            updateSettingWindow();
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
        isGameStart = true;
        isInputEnabled= true;
        if (SceneManager.GetActiveScene().name == "Tutorial" || SceneManager.GetActiveScene().name == "GameOver")
        {
            NextLevel();
        }
             
    }
    
    //NextLevel
    public void NextLevel()
    {
        int sceneIndex = 0;
        if (isGameOver)
        {
            sceneIndex = gameOverScene;
            SoundManager.Instance.PlaySFX("GameOver");
        }
        else
        {
            //SoundManager.Instance.PlaySFX("NextLevel");
            sceneIndex = RandomLevel(minLevelScene, maxLevelScene);
            if(sceneIndex == -1)
            {
                GameOver();
            }
            else
            {
                loadedScene.Add(sceneIndex);
                
                isCountScore = false;
            }          
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
        if(loadedScene.Count == (max - min) + 1)
        {
            return -1;
        }
        bool flag = true;
        while(flag)
        {
            sceneIndex = (int)Random.Range(min, max+1);
            flag=loadedScene.Contains(sceneIndex);
        }
        return sceneIndex;
    }


    public void levelComplete(int moveLeft=0)
    {
        if (!isGameOver)
        {
            isCountScore = true;
            isInputEnabled = false;
            tempScore += completeScore;
            tempScore += (moveLeft * moveBonusScore);
            if (loadedScene.Count == (maxLevelScene - minLevelScene) + 1)
            {
                tempScore += (int)timeLeft;
            }
        }  
    }

    public void OutOfMove()
    {
        if (!isGameOver)
        {
            isCountScore = true;
            isInputEnabled = false;
            tempScore += failScore;
            SoundManager.Instance.PlaySFX("OutOfMove");
            Debug.Log("OutOfMove");
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
        isInputEnabled= false;
        Debug.Log("pause");
        
    }
    public void resumeGame()
    {
        Time.timeScale = 1.0f;
        isGamePause= false;
        isInputEnabled = true;
        Debug.Log("Resume");
    }

    public void updateSettingWindow()
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
    public void GameOver()
    {
        isGameOver = true;
        score = tempScore;
        NextLevel();
    }
    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public bool isLastLevel()
    {
        if(loadedScene.Count >= (minLevelScene - maxLevelScene) + 1)
        {
            return true;
        }
        return false;
    }

    public void updateName(TMP_InputField text)
    {
        playerName = text.text;
        Debug.Log(playerName);
    }







}
