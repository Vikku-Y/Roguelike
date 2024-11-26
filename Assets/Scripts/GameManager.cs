using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    //Singleton: Existe en todos lados pero solo existe uno (Se crea como instancia y se prohibe crear más de uno)
    public static GameManager Instance { get; private set; }
    public TurnManager turnManager {  get; private set; }
    public MapManager mapManager;
    public PlayerController playerController;

    private int m_food = 100;

    public UIDocument UIDoc;
    private Label m_foodLabel;

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
        turnManager.OnTick += OnTurnHappen;

        mapManager.GenerateMap();
        playerController.Spawn(mapManager, new Vector2Int(mapManager.spawnX, mapManager.spawnY));

        //Dentro de la UI en el elemento raiz busca (Q) el elemento con label ("texto")
        m_foodLabel =  UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_foodLabel.text = "Comida: " + m_food;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        m_food+=amount;
        m_foodLabel.text = "Comida: " + m_food;
    }
}
