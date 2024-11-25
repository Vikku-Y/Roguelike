using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton: Existe en todos lados pero solo existe uno (Se crea como instancia y se prohibe crear más de uno)
    public static GameManager Instance { get; private set; }
    public TurnManager turnManager {  get; private set; }
    public MapManager mapManager;
    public PlayerController playerController;

    private int m_food = 100;

    //Singleton logic: Si no hay instancia se crea, si no, me mato (:
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        turnManager = new TurnManager();
        turnManager.OnTick += onTurnHappen;

        mapManager.GenerateMap();
        playerController.Spawn(mapManager, new Vector2Int(mapManager.spawnX, mapManager.spawnY));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTurnHappen()
    {
        m_food--;
        Debug.Log(m_food);
    }
}
