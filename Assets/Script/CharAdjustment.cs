using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Grid grid;

    private Vector3 startPos;

 

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation= true;
        rb.simulated = true;

        imageAdjustment();
        colliderAdjustment();
    }

    void imageAdjustment()
    {
        if(moveType== Move.Horizontal)
        {
            spriteRenderer.transform.Rotate(new Vector3(0, 0, -90));
        }
        spriteRenderer.sprite = sprite;
    }

    void colliderAdjustment()
    {
        var collider = GetComponent<BoxCollider2D>();
        if (charType == Char.Small)
        {
            collider.size = new Vector2(1.5f, 3f);
        }
        else
        {
            collider.size = new Vector2(1.5f, 4.5f);
        }
        if (moveType == Move.Horizontal)
        {
            collider.size = new Vector2(collider.size.y, collider.size.x);
        }
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= 960;
        mousePos.y -= 480;
        startPos = mousePos/108 - this.gameObject.transform.position;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        if (moveType== Move.Horizontal)
        {
            mousePos.x -= 960;
            mousePos.x /= 108;
            mousePos.y = this.gameObject.transform.position.y;
            this.gameObject.transform.localPosition = new Vector3(mousePos.x-(startPos.x), this.gameObject.transform.position.y,0);
        }
        else
        {
            mousePos.x = this.gameObject.transform.position.x;
            mousePos.y -= 480;
            mousePos.y /= 108;
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.position.x, mousePos.y-(startPos.y) , 0);
        }

    }
    private void PosAdjustment()
    {
        Vector3Int gridPosition = grid.WorldToCell(new Vector3(12, 54, 0) + (this.gameObject.transform.position*108));
        
        if(moveType== Move.Horizontal)
        {
            if(gridPosition.x % 2 == 0)
            {
                if(this.gameObject.transform.position.x % 216 >= 108)
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
            if(gridPosition.y % 2 != 0) 
            {
                if (this.gameObject.transform.position.y % 216 >= 108)
                {
                    gridPosition += new Vector3Int(0,1, 0);
                }
                else
                {
                    gridPosition += new Vector3Int(0,-1, 0);
                }
            }
        }

        Vector3 goalPos = grid.CellToWorld(gridPosition) / 108;
        Debug.Log(this.gameObject.transform.position + "xxx" + goalPos + "xxx" + gridPosition);
        this.gameObject.transform.localPosition = goalPos;
    }

    private void OnMouseUp()
    {
        PosAdjustment();
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
