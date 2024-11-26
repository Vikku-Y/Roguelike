using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {

    public class CellData {

        public bool passable;
        public CellObject ContainedObject;
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

    public FoodObject[] foodPrefabs;

    private List<Vector2Int> m_EmptyCells;

    public void GenerateMap()
    {
        m_BoardData = new CellData[mapTilesX, mapTilesY];
        m_EmptyCells = new List<Vector2Int>();

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

                    //A�ade casilla a lista de casillas vacias
                    m_EmptyCells.Add(new Vector2Int(x, y));
                }

            }
        }
        m_EmptyCells.Remove(new Vector2Int(1,1));

        generateFood(5);
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

    public void generateFood(int foodNumber)
    {
        for (int i = 0; i < foodNumber; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int cords = m_EmptyCells[randomIndex];
            CellData cell = m_BoardData[cords.x, cords.y];

            m_EmptyCells.RemoveAt(randomIndex);

            FoodObject newFood = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)]);
            cell.ContainedObject = newFood;
            cell.ContainedObject.transform.position = CellToWorld(cords);
        }
    }
}
