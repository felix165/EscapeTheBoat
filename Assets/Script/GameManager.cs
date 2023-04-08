using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnLevelCompleted; 
    public static GameManager Instance;
    public static float timeLimit=600f;
    public static float curTimeLimit=timeLimit;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (curTimeLimit <= 0)
        {
            //GameOver
            isGameOver = true;
            score = tempScore;
            NextLevel();
        }
        else
        {
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
                curTimeLimit -= Time.deltaTime;
            }
        }
    }
    //NewGame
    public void NewGame()
    {
        curTimeLimit= timeLimit;
        score = 0;
        tempScore = score;
        loadedScene.Clear();
        isGameOver = false;

        NextLevel();
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
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                sceneIndex = tutorialScene;
            }
            else
            {
                sceneIndex = RandomLevel(minLevelScene, maxLevelScene);
                loadedScene.Add(sceneIndex);
            }
            isLevelCompelete = false;
        }
        SceneManager.LoadScene(sceneIndex);

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
            //OnLevelCompleted?.Invoke();
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

    public void click()
    {
        SoundManager.Instance.PlaySFX("Click");
        print("click!!");
    }





}
