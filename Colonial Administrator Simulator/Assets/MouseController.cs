using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    Vector3 lastFramePosition;
    Vector3 curFramePosition;
    Vector3 dragStartPosition;
    List<GameObject> dragPreviewObjects;

    public GameObject circleCursorPrefab;

    // Start is called before the first frame update
    void Start() {
        dragPreviewObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {

        void UpdateDragging() {
            // Start Drag
            if (Input.GetMouseButtonDown(0)) {
                dragStartPosition = curFramePosition;
            }

            // Assess drag positions.
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

            // Clean up old drag previews
            while (dragPreviewObjects.Count > 0) {
                GameObject go = dragPreviewObjects[0];
                dragPreviewObjects.RemoveAt(0);
                SimplePool.Despawn(go);
            }

            if (Input.GetMouseButton(0)) {
                // Display a preview of the dragged area.
                for (int x = startX; x <= endX; x++) {
                    for (int y = startY; y <= endY; y++) {
                        Tile t = WorldController.Instance.World.GetTileAt(x, y);
                        if (t != null) {
                            // Display preview on tile.
                            GameObject go = SimplePool.Spawn(circleCursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                            go.transform.SetParent(this.transform, true);
                            dragPreviewObjects.Add(go);
                        }
                    }
                }
            }

            // End Drag
            if (Input.GetMouseButtonUp(0)) {
                for (int x = startX; x <= endX; x++) {
                    for (int y = startY; y <= endY; y++) {
                        Tile t = WorldController.Instance.World.GetTileAt(x, y);
                        if (t != null) {
                            t.Type = Tile.TileType.Floor;
                        }
                    }
                }
            }

        }

        void UpdateCameraMovement() {
            // Screen Dragging - Right or Middle Mouse
            if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) {
                Vector3 diff = lastFramePosition - curFramePosition;
                Camera.main.transform.Translate(diff);
            }

            Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
        }

        /*
        void UpdateCursor() {
            // Tile cursor
            Tile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(curFramePosition);
            if (tileUnderMouse != null) {
                cursor.SetActive(true);
                Vector3 curMousePosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
                cursor.transform.position = curMousePosition;
            } else {
                cursor.SetActive(false);
            }
        }
        */

        curFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curFramePosition.z = 0;

        UpdateDragging();
        UpdateCameraMovement();
        //UpdateCursor();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }



}
