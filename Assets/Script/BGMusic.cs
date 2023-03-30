using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    private static BGMusic music = null; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static BGMusic Music
    {
        get { return music; }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        if (music != null && music != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            music = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    
}

