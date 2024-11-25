using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {

    public class CellData {

        public bool passable;
        public GameObject ContainedObject;
    }

    private CellData[,] m_BoardData;

    public PlayerController playerCont;

    public Tilemap tileMap;

    public Tile[] groundTiles;
    public Tile[] wallTiles;
    public Tile[] obstacleTiles;

    public int mapTilesX = 8;
    public int mapTilesY = 8;

    public int spawnX = 1;
    public int spawnY = 1;

    public Grid grid;

    public GameObject FoodPrefab;

    public void GenerateMap()
    {
        m_BoardData = new CellData[mapTilesX, mapTilesY];
        for (int x = 0; x < mapTilesX; x++)
        {
            for (int y = 0; y < mapTilesY; y++)
            {
                m_BoardData[x, y] = new CellData();
                Vector3Int tileCords = new Vector3Int(x, y, 0);

                if (x == 0 || x == mapTilesX - 1 || y == 0 || y == mapTilesY - 1)
                {
                    m_BoardData[x, y].passable = false;
                    tileMap.SetTile(tileCords, wallTiles[Random.Range(0, wallTiles.Length)]);
                }
                else
                {
                    m_BoardData[x, y].passable = true;
                    tileMap.SetTile(tileCords, groundTiles[Random.Range(0, groundTiles.Length)]);
                }

            }
        }

        spreadFood(3);
    }

    public Vector3 CellToWorld (Vector2Int cellIndex)
    {
        return grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= mapTilesX || cellIndex.y < 0 || cellIndex.y >= mapTilesY)
        {
            return null;
        } else
        {
            return m_BoardData[cellIndex.x, cellIndex.y];
        }
    }

    //Spawn de comida
    public void spreadFood(int foodNumber)
    {
        int foods = 0;
        while (foods < foodNumber)
        {
            int x = Random.Range(0, mapTilesX-1);
            int y = Random.Range(0, mapTilesY-1);

            if (m_BoardData[x, y].ContainedObject == null && m_BoardData[x, y].passable)
            {
                m_BoardData[x, y].ContainedObject = FoodPrefab;
                Vector2Int tileCords = new Vector2Int(x, y);

                Instantiate(m_BoardData[x, y].ContainedObject);
                m_BoardData[x, y].ContainedObject.transform.position = CellToWorld(tileCords);

                foods++;
            }
        }
    }
}
