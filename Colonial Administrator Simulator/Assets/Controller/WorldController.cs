using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

    public static WorldController Instance { get; protected set; }

    public Sprite floorSprite;

    public World World { get; protected set; }

    // Start is called before the first frame update
    void Start() {

        void initializeSingleton() {
            if (Instance != null) {
                Debug.LogError("Tried to make a second world controller instance.");
            } else {
                Instance = this;
            }
        }

        void initializeWorld() {
            World = new World();

            for (int x = 0; x < World.Width; x++) {
                for (int y = 0; y < World.Height; y++) {
                    Tile tile_data = World.GetTileAt(x, y);

                    GameObject tile_go = new GameObject();
                    tile_go.name = "Tile_" + x + "_" + y;
                    tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                    tile_go.transform.parent = this.transform;

                    SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer>();

                    tile_data.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tile_go); });

                }
            }
            World.RandomizeTiles();
        }

        initializeSingleton();
        initializeWorld();


    }

    // Update is called once per frame
    void Update() {

    }

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go) {
        if (tile_data.Type == Tile.TileType.Floor) {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        } else if (tile_data.Type == Tile.TileType.Dirt) {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        } else {
            Debug.LogError("OnTileTypeChanged - Unrecognized tile type.");
        }
    }

    public Tile GetTileAtWorldCoord(Vector3 coord) {
        int x = Mathf.RoundToInt(coord.x);
        int y = Mathf.RoundToInt(coord.y);

        return World.GetTileAt(x, y);
    }

}
