using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private MapManager m_Map;
    private Vector2Int m_CellPosition;

    public void Spawn(MapManager mapManager, Vector2Int cell)
    {
        m_Map = mapManager;
        m_CellPosition = cell;

        transform.position = m_Map.CellToWorld(cell);
    }
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        } else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }

        if (hasMoved) { 
            MapManager.CellData cellData = m_Map.GetCellData(newCellTarget);

            if (cellData != null && cellData.passable) {
                m_CellPosition = newCellTarget;
                transform.position = m_Map.CellToWorld(m_CellPosition);
            }
        }
    }
}
