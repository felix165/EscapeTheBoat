using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float timeLimit=600f;
    public static float timeLeft;

    public static int score = 0;
    public int completeScore = 200;
    public int moveBonusScore = 10;

    private int tempScore = 0;
    private float tempDelay = 0;
    private bool isLevelCompelete = false;

    private ArrayList loadedScene = new ArrayList();
    private int minLevelScene = 2;
    private int maxLevelScene = 10;
    private int tutorialScene = 1;
    private int gameOverScene = 11;

    public static bool isGameOver = false;



    // Start is called before the first frame update
    void Start()
    {
        NewGame();
    }

    // Update is called once per frame
    void Update()
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
            if (isLevelCompelete)
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
    //NewGame
    public void NewGame()
    {
        timeLeft = timeLimit;
        score = 0;
        tempScore = score;
        loadedScene.Clear();
        isGameOver = false;

        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == -1)
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
        }
        else
        {
            SoundManager.Instance.PlaySFX("NextLevel");
            sceneIndex = RandomLevel(minLevelScene, maxLevelScene);
            loadedScene.Add(sceneIndex);
            isLevelCompelete = false;
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
            isLevelCompelete = true;
            tempScore += completeScore;
            tempScore += (moveLeft * moveBonusScore);
        }
    }
    public void OutOfMove()
    {
        isGameOver = true; //if outOfMove not mean gameover then comment this line.
        NextLevel();

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






}
