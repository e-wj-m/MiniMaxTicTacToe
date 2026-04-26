using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles all visual/UI concerns for the Tic Tac Toe board, by Eric M.
// Knows nothing about game rules or AI — only how to display state.
 
public class TicTacToeBoardUI : MonoBehaviour
{
    [Header("Board Cells")]
    [Tooltip("All 9 cell buttons, top-left to bottom-right, row by row.")]
    [SerializeField] private Button[] cellButtons = new Button[9];

    [Header("Status")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button menuButton;

    [Header("Player vs AI Scoreboard")]
    [SerializeField] private TextMeshProUGUI playerWinsText;
    [SerializeField] private TextMeshProUGUI aiWinsText;
    [SerializeField] private TextMeshProUGUI drawsText;

    [Header("AI vs AI Scoreboard")]
    [SerializeField] private TextMeshProUGUI ai1WinsText;
    [SerializeField] private TextMeshProUGUI ai2WinsText;
    [SerializeField] private TextMeshProUGUI aiDrawsText;

    [Header("Colors")]
    [SerializeField] private Color xColor = new Color(0.2f, 0.6f, 1f);
    [SerializeField] private Color oColor = new Color(1f, 0.35f, 0.35f);
    [SerializeField] private Color defaultColor = new Color(0.7f, 0.7f, 0.7f);

    public Button[] CellButtons => cellButtons;
    public Button ResetButton => resetButton;
    public Button MenuButton => menuButton;

    // Updates a single cell's display to show X, O, or empty.
    public void UpdateCell(int index, int player)
    {
        TextMeshProUGUI label = cellButtons[index].GetComponentInChildren<TextMeshProUGUI>();
        if (label == null) return;

        switch (player)
        {
            case TicTacToeBoard.AI:
                label.text = "X";
                label.color = xColor;
                break;
            case TicTacToeBoard.Player:
                label.text = "O";
                label.color = oColor;
                break;
            default:
                label.text = "";
                label.color = defaultColor;
                break;
        }
    }

    // Sets the status message displayed to the player.
    public void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    // Refreshes the Player vs AI scoreboard from a GameStats instance. Side1 = AI wins, Side2 = Player wins.
    public void UpdateScoreboard(GameStats stats)
    {
        if (playerWinsText != null)
            playerWinsText.text = $"Player: {stats.Side2Wins}";

        if (aiWinsText != null)
            aiWinsText.text = $"AI: {stats.Side1Wins}";

        if (drawsText != null)
            drawsText.text = $"Draws: {stats.Draws}";
    }

    // Refreshes the AI vs AI scoreboard from a GameStats instance. Side1 = AI #1 (X) wins, Side2 = AI #2 (O) wins.
    public void UpdateAIvsAIScoreboard(GameStats stats)
    {
        if (ai1WinsText != null)
            ai1WinsText.text = $"AI #1 (X): {stats.Side1Wins}";

        if (ai2WinsText != null)
            ai2WinsText.text = $"AI #2 (O): {stats.Side2Wins}";

        if (aiDrawsText != null)
            aiDrawsText.text = $"Draws: {stats.Draws}";
    }

    // Clears all 9 cells back to their default empty state.
    public void ClearBoard()
    {
        for (int i = 0; i < cellButtons.Length; i++)
            UpdateCell(i, TicTacToeBoard.Empty);
    }
}
