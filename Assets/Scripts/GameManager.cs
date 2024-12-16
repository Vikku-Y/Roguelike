using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    //Singleton: Existe en todos lados pero solo existe uno (Se crea como instancia y se prohibe crear más de uno)
    public static GameManager Instance { get; private set; }
    public TurnManager turnManager {  get; private set; }
    public MapManager mapManager;
    public PlayerController playerController;

    public int food = 100;
    private int m_level = 1;

    public UIDocument UIDoc;
    private VisualElement m_gameOverPanel;
    private Label m_foodLabel;
    private Label m_levelLabel;
    private Label m_gameOverMsg;

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

        StartMap();

        //Dentro de la UI en el elemento raiz busca (Q) el elemento con label ("texto")
        m_foodLabel =  UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_foodLabel.text = "Comida: " + food;

        m_levelLabel =  UIDoc.rootVisualElement.Q<Label>("LevelLabel");
        m_levelLabel.text = "Nivel: " + m_level;

        m_gameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");

        m_gameOverMsg = m_gameOverPanel.Q<Label>("GameOverMsg");

        m_gameOverPanel.style.visibility = Visibility.Hidden;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && m_gameOverPanel.style.visibility == Visibility.Visible)
        {
            RestartGame();
        }
    }

    public void StartMap()
    {
        int spawnX = Random.Range(1, mapManager.mapTilesX - 2);
        int spawnY = Random.Range(1, mapManager.mapTilesY - 2);
        int exitX = 6;
        int exitY = 6;

        if (spawnX > 1)
        {
            exitX = 1;
            spawnX = mapManager.mapTilesX - 2;
        }

        if (spawnY > 1)
        {
            exitY = 1;
            spawnY = mapManager.mapTilesY - 2;
        }

        mapManager.Init(new Vector2Int(exitX, exitY), new Vector2Int(spawnX, spawnY));
        playerController.Spawn(mapManager, new Vector2Int(spawnX, spawnY));
    }

    public void RestartGame()
    {
        playerController.GameOver(false);
        playerController.gameObject.SetActive(true);

        m_gameOverPanel.style.visibility = Visibility.Hidden;

        food = 100;
        turnManager.turnCount = 0;
        m_level = 0;

        m_foodLabel.text = "Comida: " + food;
        m_foodLabel.style.visibility = Visibility.Visible;
        m_levelLabel.style.visibility = Visibility.Visible;

        LevelClear();
    }

    public void OnTurnHappen()
    {
        ChangeFood(-3);
    }

    public void ChangeFood(int amount)
    {
        food+=amount;
        m_foodLabel.text = "Comida: " + food;

        if (food <= 0)
        {
            playerController.GameOver(true);

            m_foodLabel.style.visibility = Visibility.Hidden;
            m_levelLabel.style.visibility = Visibility.Hidden;
            m_gameOverPanel.style.visibility = Visibility.Visible;
            m_gameOverMsg.text = "Game Over\n\nHas muerto de hambre en el nivel " + m_level + "\n\nPulsa R para reiniciar";
        }
    }

    public void LevelClear()
    {
        m_level++;
        m_levelLabel.text = "Nivel: " + m_level;

        mapManager.CleanMap();

        StartMap();
    }
}
