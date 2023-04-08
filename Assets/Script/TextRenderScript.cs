using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextRenderScript : MonoBehaviour
{
    public enum Opsi
    {
        TimeLimit,
        Score,

    }

    public Opsi option;
    public Text textObj;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (option)
        {
            case Opsi.TimeLimit:
                textObj.text = GameManager.timeLimit.ToString("0");
                break;
            case Opsi.Score:
                textObj.text = GameManager.score.ToString("0");
                break;
            default:
                Debug.Log("Option hasn't defined yet");
                return;

        }

    }
}
