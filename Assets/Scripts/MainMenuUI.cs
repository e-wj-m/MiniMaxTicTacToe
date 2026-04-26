using UnityEngine;
using UnityEngine.UI;
using TMPro;

// UI controller for the Main Menu scene, by Eric M.
// Loads persisted stats for both game modes from PlayerPrefs so the player can see their record before jumping back in.

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button spectateButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resetStatsButton;

    [Header("Player vs AI Scoreboard")]
    [SerializeField] private TextMeshProUGUI playerWinsText;
    [SerializeField] private TextMeshProUGUI aiWinsText;
    [SerializeField] private TextMeshProUGUI pvaiDrawsText;

    [Header("AI vs AI Scoreboard")]
    [SerializeField] private TextMeshProUGUI ai1WinsText;
    [SerializeField] private TextMeshProUGUI ai2WinsText;
    [SerializeField] private TextMeshProUGUI aivsaiDrawsText;

    private GameStats pvaiStats;
    private GameStats aivsaiStats;

    private void Start()
    {
        pvaiStats = new GameStats("PvAI");
        aivsaiStats = new GameStats("AIvsAI");

        if (playButton != null)
            playButton.onClick.AddListener(GameSceneManager.LoadGame);

        if (spectateButton != null)
            spectateButton.onClick.AddListener(GameSceneManager.LoadAIvsAI);

        if (quitButton != null)
            quitButton.onClick.AddListener(GameSceneManager.QuitGame);

        if (resetStatsButton != null)
            resetStatsButton.onClick.AddListener(ResetAllStats);

        RefreshScoreboards();
    }

    private void RefreshScoreboards()
    {
        // Player vs AI — Side1 = AI, Side2 = Player.
        if (playerWinsText != null)
            playerWinsText.text = $"Player: {pvaiStats.Side2Wins}";

        if (aiWinsText != null)
            aiWinsText.text = $"AI: {pvaiStats.Side1Wins}";

        if (pvaiDrawsText != null)
            pvaiDrawsText.text = $"Draws: {pvaiStats.Draws}";

        // AI vs AI — Side1 = AI #1 (X), Side2 = AI #2 (O).
        if (ai1WinsText != null)
            ai1WinsText.text = $"AI #1 (X): {aivsaiStats.Side1Wins}";

        if (ai2WinsText != null)
            ai2WinsText.text = $"AI #2 (O): {aivsaiStats.Side2Wins}";

        if (aivsaiDrawsText != null)
            aivsaiDrawsText.text = $"Draws: {aivsaiStats.Draws}";
    }

    private void ResetAllStats()
    {
        pvaiStats.ResetAll();
        aivsaiStats.ResetAll();
        RefreshScoreboards();
    }
}
