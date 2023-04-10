using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Control : MonoBehaviour
{
   // [SerializeField]
    //private GameObject cellIndicator;

    [SerializeField]
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*        Debug.Log(Input.mousePosition);
                Vector3 mousePosition = inputManager.GetSelectedMapPosition();*/
        Debug.Log(Input.mousePosition + "mouse");
        Vector3Int gridPosition = grid.WorldToCell(Input.mousePosition);
        Debug.Log(gridPosition+"grid -->"+ grid.CellToWorld(gridPosition));
        
     //   cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
