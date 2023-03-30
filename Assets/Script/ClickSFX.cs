using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSFX : MonoBehaviour
{
    public AudioSource SFX;
    bool clicked = false;
    public float delay = 1;
    // Start is called before the first frame update
    void Start()
    {
        clicked = false;
    }
    private void OnMouseDown()
    {
        if (clicked == false)
        {
            SFX.Play();
            clicked = true;
        }
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
    }
}
