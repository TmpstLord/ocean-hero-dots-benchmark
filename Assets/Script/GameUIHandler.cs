using UnityEngine;
using TMPro;
using Unity.Entities;
using UnityEngine.SceneManagement; 

public class GameUIHandler : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ScoreText;
    public GameObject WinPanel;
    public GameObject LosePanel;

    private EntityManager _entityManager;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Update()
    {
        EntityQuery query = _entityManager.CreateEntityQuery(
            typeof(GameDataComponent), 
            typeof(ScoreComponent)
        );

        if (query.HasSingleton<GameDataComponent>())
        {
            var entity = query.GetSingletonEntity();
            var gameData = _entityManager.GetComponentData<GameDataComponent>(entity);
            var scoreData = _entityManager.GetComponentData<ScoreComponent>(entity);

            // 1. Update Teks HUD (DIBERIKAN PELINDUNG NULL CHECK)
            if (TimerText != null) 
            {
                TimerText.text = ": " + Mathf.CeilToInt(gameData.CurrentTime).ToString() + "s";
            }
            
            if (ScoreText != null) 
            {
                ScoreText.text = "" + scoreData.Value + "/" + gameData.TargetScore;
            }

            // 2. Cek Menang/Kalah (DIBERIKAN PELINDUNG NULL CHECK)
            if (gameData.IsGameOver)
            {
                if (gameData.IsWin)
                {
                    if (WinPanel != null) WinPanel.SetActive(true);
                }
                else
                {
                    if (LosePanel != null) LosePanel.SetActive(true);
                }
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}