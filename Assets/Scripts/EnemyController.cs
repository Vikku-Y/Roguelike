using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool m_isMoving;
    public float moveSpeed = 5f;
    private Vector3 m_moveDirection;

    private MapManager m_Map;
    private Vector2Int m_CellPosition;

    public GameObject player;

    private void Update()
    {
        if (m_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_moveDirection, moveSpeed * Time.deltaTime);

            if (transform.position == m_moveDirection)
            {
                m_isMoving = false;
                var cellData = m_Map.GetCellData(m_CellPosition);
                if (cellData.ContainedObject != null) cellData.ContainedObject.PlayerEntered();
            }
            return;
        }
    }
}
