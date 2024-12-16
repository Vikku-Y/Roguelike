using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private bool m_isMoving;
    public float moveSpeed = 5f;
    private Vector3 m_moveDirection;

    private MapManager m_Map;
    private Vector2Int m_CellPosition;
    private Animator m_Animator;

    private bool m_gameOver = false;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_gameOver) 
        { 
            gameObject.SetActive(false);
            return;
        }

        if (m_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_moveDirection, moveSpeed*Time.deltaTime);

            if (transform.position == m_moveDirection)
            {
                m_isMoving = false;
                m_Animator.SetBool("Moving", false);
                var cellData = m_Map.GetCellData(m_CellPosition);
                if (cellData.ContainedObject != null) cellData.ContainedObject.PlayerEntered();
            }
            return;
        }
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

            if (!GetComponent<SpriteRenderer>().flipX && GameManager.Instance.food > 0) { 
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;

            if (GetComponent<SpriteRenderer>().flipX && GameManager.Instance.food > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        if (hasMoved) { 
            MapManager.CellData cellData = m_Map.GetCellData(newCellTarget);

            if (cellData != null && cellData.passable) {
                GameManager.Instance.turnManager.TurnTick();

                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget, false);
                } else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget, false);
                }
            } 
        }
    }
    public void Spawn(MapManager mapManager, Vector2Int cell)
    {
        m_Map = mapManager;
        
        MoveTo(cell, true);
    }
    public void MoveTo(Vector2Int cell, bool teleport)
    {
        m_isMoving = true;
        m_CellPosition = cell;

        if (teleport)
        {
            m_isMoving = false;
            transform.position = m_Map.CellToWorld(m_CellPosition);
        }

        m_Animator.SetBool("Moving", m_isMoving);
        m_moveDirection = m_Map.CellToWorld(m_CellPosition);
        
    }

    public void GameOver(bool state)
    {
        m_gameOver = state;
    }
}
