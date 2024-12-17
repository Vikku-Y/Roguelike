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

    public int mapTilesX = 8;
    public int mapTilesY = 8;

    public Grid grid;

    public FoodObject[] foodPrefabs;
    public WallObject[] wallPrefabs;
    public ExitCellObject exitPrefab;
    public EnemyController enemyPrefab;

    private List<Vector2Int> m_EmptyCells;

    public void Init(Vector2Int exit, Vector2Int spawn)
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

                    //Añade casilla a lista de casillas vacias
                    m_EmptyCells.Add(new Vector2Int(x, y));
                }

            }
        }
        m_EmptyCells.Remove(spawn);

        GenerateExit(exit);
        GenerateWall(Random.Range(5, 11));
        GenerateFood(Random.Range(4, 5));
        GenerateEnemies(1);
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

    public void CleanMap()
    {
        if (m_BoardData != null)
        {
            for (int x = 0; x < mapTilesX; x++)
            {
                for (int y = 0; y < mapTilesY; y++)
                {
                    if (m_BoardData[x, y].ContainedObject)
                    {
                        Destroy(m_BoardData[x, y].ContainedObject.gameObject);
                    }
                    SetCellTile(new Vector2Int(x, y), null);
                }
            }

            m_BoardData = null;
            m_EmptyCells = null;
        }
    }

    public void GenerateFood(int foodNumber)
    {
        for (int i = 0; i < foodNumber; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int cords = m_EmptyCells[randomIndex];

            AddObject(foodPrefabs[Random.Range(0, foodPrefabs.Length)], cords);
            m_EmptyCells.RemoveAt(randomIndex);
        }
    }

    public void GenerateWall(int wallNumber)
    {
        for (int i = 0; i < wallNumber; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int cords = m_EmptyCells[randomIndex];

            AddObject(wallPrefabs[Random.Range(0, wallPrefabs.Length)], cords);
            m_EmptyCells.RemoveAt(randomIndex);
        }
    }

    public void GenerateEnemies(int enemies)
    {
        for (int i = 0;i < enemies; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int coord = m_EmptyCells[randomIndex];
            m_EmptyCells.RemoveAt(randomIndex);
            EnemyController newEnemy = Instantiate(enemyPrefab);
            AddObject(newEnemy, coord);
        }
    }

    public void GenerateExit(Vector2Int exit)
    {
        AddObject(exitPrefab, exit);
        m_EmptyCells.Remove(exit);
    }

    public void AddObject(CellObject addedObject, Vector2Int cords) {
        CellData cell = m_BoardData[cords.x, cords.y];

        addedObject.transform.position = CellToWorld(cords);
        cell.ContainedObject = addedObject;
        addedObject.Init(cords);

    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        tileMap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return tileMap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }
}
