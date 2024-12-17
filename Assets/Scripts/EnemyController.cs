using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : CellObject
{
    /*private bool m_isMoving;
    public float moveSpeed = 5f;
    private Vector3 m_moveDirection;*/

    private Vector2Int CellPosition;

    public int health;

    private void Awake()
    {
        GameManager.Instance.turnManager.OnTick += EnemyTurnHappen;
    }
    private void OnDestroy()
    {
        GameManager.Instance.turnManager.OnTick -= EnemyTurnHappen;
    }
    public override void Init(Vector2Int cell)
    {
        CellPosition = cell;
    }
    public override bool PlayerWantsToEnter()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        return false;
    }

    private void Update()
    {
        /*if (m_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_moveDirection, moveSpeed * Time.deltaTime);

            if (transform.position == m_moveDirection)
            {
                m_isMoving = false;
                var cellData = m_Map.GetCellData(CellPosition);
                if (cellData.ContainedObject != null) cellData.ContainedObject.PlayerEntered();
            }
            return;
        }*/
    }

    bool MoveTo(Vector2Int position)
    {
        var board = GameManager.Instance.mapManager;
        var targetCell = board.GetCellData(position);

        if (targetCell == null || !targetCell.passable || targetCell.ContainedObject != null)
        {
            return false;
        }

        var currentCell = board.GetCellData(CellPosition);
        currentCell.ContainedObject = null;

        targetCell.ContainedObject = this;
        CellPosition = position;
        transform.position = board.CellToWorld(position);

        return true;
    }

    void EnemyTurnHappen()
    {
        var playerCell = GameManager.Instance.playerController.CellPosition;

        int xDist = playerCell.x - CellPosition.x;
        int yDist = playerCell.y - CellPosition.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        Vector2Int newCellTarget = CellPosition;

        if ((xDist == 0 && yDist == 1) || (xDist == 1 && yDist == 0))  
        {
            GameManager.Instance.ChangeFood(-10);
        } else {
            if (absXDist > absYDist)
            {
                if (xDist > 0)
                {
                    newCellTarget.x++;
                }
                else
                {
                    newCellTarget.x--;
                }

                MoveTo(newCellTarget);
            }
            else
            {
                if (yDist > 0)
                {
                    newCellTarget.y++;
                }
                else
                {
                    newCellTarget.y--;
                }

                MoveTo(newCellTarget);
            }
        }
    }
}
