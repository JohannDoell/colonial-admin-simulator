using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    Vector3 lastFramePosition;
    Vector3 dragStartPosition;

    public GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 curFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curFramePosition.z = 0;

        // Tile cursor
        Tile tileUnderMouse = GetTileAtWorldCoord(curFramePosition);
        if (tileUnderMouse != null) {
            cursor.SetActive(true);
            Vector3 curMousePosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            cursor.transform.position = curMousePosition;
        } else {
            cursor.SetActive(false);
        }
        
        // Start Drag
        if (Input.GetMouseButtonDown(0)) {
            dragStartPosition = curFramePosition;
        }

        // End Drag
        if (Input.GetMouseButtonUp(0)) {
            int startX = Mathf.RoundToInt(dragStartPosition.x);
            int endX = Mathf.RoundToInt(curFramePosition.x);
            
            if (endX < startX) {
                int temp = endX;
                endX = startX;
                startX = temp;
            }

            int startY = Mathf.RoundToInt(dragStartPosition.y);
            int endY = Mathf.RoundToInt(curFramePosition.y);

            if (endY < startY) {
                int temp = endY;
                endY = startY;
                startY = temp;
            }

            for (int x = startX; x <= endX; x++) {
                for (int y = startY; y <= endY; y++) {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null) {
                        t.Type = Tile.TileType.Floor;
                    }
                }
            }
        }

        // Screen Dragging - Right or Middle Mouse
        if(Input.GetMouseButton(1) || Input.GetMouseButton (2)) {
            Vector3 diff = lastFramePosition - curFramePosition;
            Camera.main.transform.Translate(diff);
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;

    }

    Tile GetTileAtWorldCoord(Vector3 coord) {
        int x = Mathf.RoundToInt(coord.x);
        int y = Mathf.RoundToInt(coord.y);

        

        return WorldController.Instance.World.GetTileAt(x, y);
    }

}
