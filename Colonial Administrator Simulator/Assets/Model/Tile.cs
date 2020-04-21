using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{

    public enum TileType { Dirt, Floor };
    public int X { get => x; }
    public int Y { get => y; }
    public TileType Type
    {
        get { return type; }
        set {
            if (type != value) {
                type = value;
                if (callbackTileTypeChanged != null)
                {
                    callbackTileTypeChanged(this);
                }
            }

        }
    }

    Action<Tile> callbackTileTypeChanged;

    LooseObject looseObject;
    InstalledObject installedObject;

    World world;
    int x;
    int y;
    TileType type;



    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    public void RegisterTileTypeChangedCallback(Action<Tile> callback)
    {
        callbackTileTypeChanged = callback;
    }

}
