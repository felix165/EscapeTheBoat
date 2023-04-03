using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
/*    private static BGMusic music = null;
    public AudioSource SFX;*/

    bool clicked = false;
    public float delay = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }
/*    public static BGMusic Music
    {
        get { return music; }
    }
    // Update is called once per frame
    void Update()
    {
        if (clicked == true)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {

                clicked = false;
            }
        }
    }*/
    public void Play()
    {

    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
