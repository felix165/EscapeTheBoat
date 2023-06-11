using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static GameManager;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class PlayerData
    {
        public string username;
        public int score;
        public float timeLeft;
    }

    public static GameManager Instance;
    public float timeLimit=600f;
    public static float timeLeft = 600f;

    public static string username = "";
    public static int score = 0;
    public int completeScore = 200;
    public int moveBonusScore = 10;
    public int failScore = 50;
    private int incAmount = 20;

    //private int playerData.score = 0;
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

    CloudSave cloudSave;
    PlayerData playerData = new PlayerData()
    {
        username = username,
        score = 0,
        timeLeft = 600f, 
    };

    private string saveID = "";


    // Start is called before the first frame update
    void Start()
    {
        cloudSave = this.gameObject.GetComponent<CloudSave>();
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
                if (!isGameOver)
                {
                    GameOver();
                }
            }
            else
            {
                //Score Animation
                if (isCountScore)
                {
                    if (playerData.score != score)
                    {
                        tempDelay += Time.deltaTime;
                        if (tempDelay >= 0.35f)
                        {
                            tempDelay = 0f;
                            if (score + incAmount > playerData.score)
                            {
                                score = playerData.score;
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
                        isInputEnabled = true;
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

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        throw new NotImplementedException();
    }

    //NewGame
    public void NewGame()
    {
        if(cloudSave!= true)
        {
            cloudSave = this.gameObject.GetComponent<CloudSave>();
        }

        if (cloudSave == true)
        {
            generateUniqueID();
            Debug.Log(timeLimit);
            timeLeft = timeLimit;
            score = 0;
            playerData.score = score;
            loadedScene.Clear();
            isGameOver = false;
            isGameStart = true;
            isInputEnabled = true;
            playerData.score = 0;
            playerData.timeLeft = timeLimit;
            saveData();

            if (SceneManager.GetActiveScene().name == "Tutorial" || SceneManager.GetActiveScene().name == "GameOver")
            {
                NextLevel();
            }
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
        signIn();
        if(cloudSave == true)
        {
            SceneManager.LoadScene(tutorialScene);
        }
        else
        {
            Debug.Log("you haven't sign in yet");
        }
        
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
            sceneIndex = (int)UnityEngine.Random.Range(min, max+1);
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
            playerData.score += completeScore;
            playerData.score += (moveLeft * moveBonusScore);
            if (loadedScene.Count == (maxLevelScene - minLevelScene) + 1)
            {
                playerData.score += (int)timeLeft;
            }
            saveData();
        }  
    }

    public void OutOfMove()
    {
        if (!isGameOver)
        {
            isCountScore = true;
            isInputEnabled = false;
            playerData.score += failScore;
            SoundManager.Instance.PlaySFX("OutOfMove");
            Debug.Log("OutOfMove");
            saveData();
        }
    }
    public void QuitGame()
    {
        if (cloudSave != true)
        {
            cloudSave = this.gameObject.GetComponent<CloudSave>();
        }
        if (cloudSave == true)
        {
            cloudSave.OnClickSignOut();
            Application.Quit();
        }
        
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
        score = playerData.score;
        NextLevel();
    }
    public void reloadScene()
    {
        if(isInputEnabled && !isCountScore)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
    public bool isLastLevel()
    {
        if(loadedScene.Count >= (maxLevelScene - minLevelScene) + 1)
        {
            return true;
        }
        return false;
    }

    public void updateName(TMP_InputField text)
    {
        if(cloudSave == null)
        {
            cloudSave = this.gameObject.GetComponent<CloudSave>();
        }
        username = text.text;
        cloudSave.OnClickSwitchProfile();
    }

    public void signIn()
    {
        if (cloudSave == null)
        {
            cloudSave = this.gameObject.GetComponent<CloudSave>();
        }
        cloudSave.OnClickSignIn();
    }
    public async void saveData()
    {
        playerData.timeLeft = timeLeft;
        playerData.username = username;
        await cloudSave.ForceSaveObjectData($"Save_{saveID}", playerData);
    }
    public void generateUniqueID()
    {
        string newBackstageItemID = System.Guid.NewGuid().ToString();
        saveID = newBackstageItemID;
    }






}
