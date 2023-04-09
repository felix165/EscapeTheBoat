using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharAdjustment : MonoBehaviour
{
    public enum Move
    {
        Horizontal,
        Vertical,
    }

    public enum Char
    {
        Small,
        Large,
    }
    public GameObject gameObj;
    public SpriteRenderer spriteRenderer;
    public Sprite sprite;
    public Move moveType;
    public Char charType;
    public Control control;

    // Start is called before the first frame update
    void Start()
    {

        imageAdjustment();
    }

    void imageAdjustment()
    {
        if(moveType== Move.Horizontal)
        {
            spriteRenderer.transform.Rotate(new Vector3(0, 0, -90));
        }
        spriteRenderer.sprite = sprite;
    }
    void posAdjustment()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
