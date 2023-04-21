using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour
{
    public UnityEvent onLevelCompleted;
    public int moveLimit;
    public Text MovementLimit;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "PlayerChar")
        {
            GameManager.Instance.levelComplete(moveLimit);
            onLevelCompleted?.Invoke();
            Debug.Log("Level Complete");
        }
    }
    // Update is called once per frame
    void Update()
    {
        MovementLimit.text = moveLimit.ToString("0");
    }

    public void MoveDecrease()
    {
        if (moveLimit > 0)
        {
            moveLimit -= 1;
        }
        if (moveLimit <= 0)
        {
            GameManager.Instance.OutOfMove();
        }
    }
}
