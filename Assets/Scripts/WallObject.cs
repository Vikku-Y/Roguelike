using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    private Tile m_OriginalTile;
    private Vector2Int m_OriginalPosition;
    public Tile obstacleTile;
    public Tile damagedTile;
    public int health;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_OriginalPosition = cell;
        m_OriginalTile = GameManager.Instance.mapManager.GetCellTile(m_OriginalPosition);

        GameManager.Instance.mapManager.SetCellTile(cell, obstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        health--;

        if (health <= 0)
        {
            GameManager.Instance.mapManager.SetCellTile(m_OriginalPosition, m_OriginalTile);
            Destroy(this.gameObject);
            return true;
        } else if (health == 1)
        {
            GameManager.Instance.mapManager.SetCellTile(m_OriginalPosition, damagedTile);
        }
        
        return false;
    }
}
