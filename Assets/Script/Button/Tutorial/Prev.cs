using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Prev : MonoBehaviour
{
    public UnityEvent onMouseClick;
    public TutorialSceneManager TutorialSceneManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnMouseUp()
    {
        onMouseClick?.Invoke();
        TutorialSceneManager.Prev();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
