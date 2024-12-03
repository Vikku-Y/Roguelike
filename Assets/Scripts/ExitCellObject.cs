using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject
{
    public Tile exitTile;
    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        GameManager.Instance.mapManager.SetCellTile(cell, exitTile);
    }

    public override void PlayerEntered()
    {
        GameManager.Instance.ChangeFood(3);
        GameManager.Instance.LevelClear();
    }
}
