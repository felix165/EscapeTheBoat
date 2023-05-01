using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Color = UnityEngine.Color;

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
    public Grid grid;
    private Rigidbody2D rb;

    private Vector3 startPos;
    private Vector3 tempStartPos;
    public float speed = 1.2f;

    public GameObject moveLimitObj;
    /*public UnityEvent onMoveSuccess;*/
    public UnityEvent onMoveFail;
    private Color transparentColor;
    private GameObject shadow;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation= true;
        rb.simulated = true;
        if (moveType == Move.Horizontal)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        //transparentColor
        Color color = spriteRenderer.material.GetColor("_Color");
        transparentColor = new Color(color.r, color.g, color.b, 0.45f);

        imageAdjustment();
        colliderAdjustment();
        this.gameObject.transform.position = PosAdjustment();


    }

    void imageAdjustment()
    {
        shadow = gameObject.transform.GetChild(0).gameObject;
        shadow.transform.Rotate(new Vector3(0, 0, 0));
        if (moveType== Move.Horizontal)
        {
            spriteRenderer.transform.Rotate(new Vector3(0, 0, -90));
            shadow.transform.Rotate(new Vector3(0, 0, -90));
        }
        spriteRenderer.sprite = sprite;

        //shadow
        shadow.transform.localScale = spriteRenderer.transform.localScale;
        shadow.GetComponent<SpriteRenderer>().sprite = sprite;
        shadow.GetComponent<SpriteRenderer>().material.color = transparentColor;

    }

    void colliderAdjustment()
    {
        var collider = GetComponent<BoxCollider2D>();
        if (charType == Char.Small)
        {
            collider.size = new Vector2(1.45f, 2.95f);
        }
        else
        {
            collider.size = new Vector2(1.45f, 4.45f);
        }
        if (moveType == Move.Horizontal)
        {
            collider.size = new Vector2(collider.size.y, collider.size.x);
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.isInputEnabled == false) { return; }
        if (moveLimitObj.GetComponent<ExitDoor>().IsOutOfMove()) { return; }
        startPos = this.transform.position;
        if (moveType == Move.Horizontal)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }

    private void OnMouseDrag()
    {
        if (GameManager.isInputEnabled == false) { return; }
        Vector3 mousePos = Input.mousePosition;

        if (moveType == Move.Horizontal)
        {
            mousePos.x -= 960;
            mousePos.x /= 108;
            mousePos.y = this.gameObject.transform.position.y;
            rb.velocity = new Vector3((mousePos.x - (this.gameObject.transform.position.x)) * speed, 0, 0);
        }
        else
        {
            mousePos.x = this.gameObject.transform.position.x;
            mousePos.y -= 480;
            mousePos.y /= 108;
            rb.velocity = new Vector3(0, (mousePos.y - (this.gameObject.transform.position.y))*speed, 0);
        }
        //Debug.Log(rb.velocity); 
        shadow.transform.position = PosAdjustment();


    }
    private Vector3 PosAdjustment()
    {
        Vector3Int gridPosition = grid.WorldToCell(new Vector3(12, 54, 0) + (this.gameObject.transform.position*108));

        if (moveType== Move.Horizontal)
        {
            if (gridPosition.x % 2 != 0 && charType==Char.Small) //Small
            {
                
                if(Math.Abs(this.gameObject.transform.position.x) % 2 >= 1)
                {
                    gridPosition += new Vector3Int(1, 0, 0);
                }
                else
                {
                    gridPosition += new Vector3Int(-1, 0, 0);
                }
            }
            if (gridPosition.x % 2 == 0 && charType == Char.Large) //Large
            {
                if (Math.Abs(this.gameObject.transform.position.x) % 2 >= 1)
                {
                    gridPosition += new Vector3Int(1, 0, 0);
                }
                else
                {
                    gridPosition += new Vector3Int(-1, 0, 0);
                }
            }
        }
        else
        {
            if(gridPosition.y % 2 != 0 && charType == Char.Small) //Small
            {
                if (Math.Abs(this.gameObject.transform.position.y) % 2 >= 1)
                {
                    gridPosition += new Vector3Int(0,1, 0);
                }
                else
                {
                    gridPosition += new Vector3Int(0,-1, 0);
                }
            }
            if (gridPosition.y % 2 == 0 && charType == Char.Large) //Large
            {
                if (Math.Abs(this.gameObject.transform.position.y) % 2 >= 1)
                {
                    gridPosition += new Vector3Int(0, 1, 0);
                }
                else
                {
                    gridPosition += new Vector3Int(0, -1, 0);
                }
            }
        }

        Vector3 goalPos = grid.CellToWorld(gridPosition) / 108;
        //Debug.Log(this.gameObject.transform.position + "xxx" + goalPos + "xxx" + gridPosition);
        return goalPos;
    }

    private void OnMouseUp()
    {
        if (GameManager.isInputEnabled == false) { return; }
        if (moveLimitObj.GetComponent<ExitDoor>().IsOutOfMove()) { onMoveFail?.Invoke(); return; }
        this.gameObject.transform.position = PosAdjustment();
        shadow.transform.position= PosAdjustment();
        
        if (moveType == Move.Horizontal)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if(startPos != this.transform.position)
        {
            moveLimitObj.GetComponent<ExitDoor>().MoveDecrease();
            /*onMoveSuccess?.Invoke();*/
        }
        else
        {
            onMoveFail?.Invoke();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


}
