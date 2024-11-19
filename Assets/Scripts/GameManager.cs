using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    TurnManager m_turnManager = new TurnManager();
    public MapManager mapManager;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        mapManager.GenerateMap();
        playerController.Spawn(mapManager, new Vector2Int(mapManager.spawnX, mapManager.spawnY));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
