using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
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
    private Rigidbody2D rb;

    private Vector3 startPos;
    private Vector3 tempStartPos;
    public float speed = 1.2f;
 

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
        imageAdjustment();
        colliderAdjustment();
        PosAdjustment();
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
            collider.size = new Vector2(1.49f, 2.99f);
        }
        else
        {
            collider.size = new Vector2(1.49f, 4.49f);
        }
        if (moveType == Move.Horizontal)
        {
            collider.size = new Vector2(collider.size.y, collider.size.x);
        }
    }

    private void OnMouseDown()
    {
        /*tempStartPos = this.gameObject.transform.position;
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= 960;
        mousePos.y -= 480;
        startPos = mousePos/108 - this.gameObject.transform.position;
*/
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
        Vector3 mousePos = Input.mousePosition;

        if (moveType == Move.Horizontal)
        {
            mousePos.x -= 960;
            mousePos.x /= 108;
            mousePos.y = this.gameObject.transform.position.y;
            //this.gameObject.transform.localPosition = new Vector3(mousePos.x - (startPos.x), this.gameObject.transform.position.y, 0);
            rb.velocity = new Vector3((mousePos.x - (this.gameObject.transform.position.x)) * speed, 0, 0);
            Debug.Log(rb.velocity);
        }
        else
        {
            mousePos.x = this.gameObject.transform.position.x;
            mousePos.y -= 480;
            mousePos.y /= 108;
            //this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.position.x, mousePos.y - (startPos.y), 0);
            rb.velocity = new Vector3(0, (mousePos.y - (this.gameObject.transform.position.y))*speed, 0);
            Debug.Log(rb.velocity);
        }     
        
    }
    private void PosAdjustment()
    {
        Vector3Int gridPosition = grid.WorldToCell(new Vector3(12, 54, 0) + (this.gameObject.transform.position*108));

        if (moveType== Move.Horizontal)
        {
            if(gridPosition.x % 2 != 0 && charType==Char.Small) //Small
            {
/*                Debug.Log(this.gameObject.transform.position.x % 2);
*/                if(this.gameObject.transform.position.x % 2 >= 1)
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
/*                Debug.Log(this.gameObject.transform.position.x % 2);
*/                if (this.gameObject.transform.position.x % 2 >= 1)
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
/*                Debug.Log(this.gameObject.transform.position.y % 2);
*/                if (this.gameObject.transform.position.y % 2 >= 1)
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
/*                Debug.Log(this.gameObject.transform.position.y % 2);*/
                if (this.gameObject.transform.position.y % 2 >= 1)
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
        this.gameObject.transform.localPosition = goalPos;
    }

    private void OnMouseUp()
    {
        PosAdjustment();
        
        if (moveType == Move.Horizontal)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


}
