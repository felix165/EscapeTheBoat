using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LastLevel : MonoBehaviour
{
    float delay = 3f;
    // Start is called before the first frame update
    void Start()
    {

        if (GameManager.Instance.isLastLevel())
        {
            GetComponent<FadeManager>().unFade();
        }
        else
        {
            GetComponent<FadeManager>().Fade(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isLastLevel())
        {
            delay-=Time.deltaTime;
            if(delay <= 0)
            {
                GetComponent<FadeManager>().Fade(0.12f);
            }
        }
    }
}
