using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField]
    public GameObject thisGameObj;
    
    private UnityEngine.UI.Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = thisGameObj.GetComponent<UnityEngine.UI.Slider>();
        slider.value=SoundManager.volume;
    }

    // Update is called once per frame
    void Update()
    {
        SoundManager.volume = slider.value;
        Debug.Log(SoundManager.volume);
    }
}
