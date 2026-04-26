using UnityEngine;


// Game controller that orchestrates the board state, AI solver, UI, and stats, by Eric M.
// This is the only class that knows about all other four — it wires them together.

[RequireComponent(typeof(TicTacToeBoardUI))]
public class TicTacToeGame : MonoBehaviour
{
    [Header("Gameplay")]
    [Tooltip("Delay in seconds before the AI places its move.")]
    [SerializeField] private float aiMoveDelay = 0.3f;

    // Core objects.
    private TicTacToeBoard board;
    private MinimaxSolver solver;
    private TicTacToeBoardUI boardUI;
    private GameStats stats;

    private bool gameActive;
    private bool playerTurn;

    private void Awake()
    {
        boardUI = GetComponent<TicTacToeBoardUI>();

        board = new TicTacToeBoard();
        solver = new MinimaxSolver(board);
        stats = new GameStats("PvAI"); // Loads persisted Player vs AI stats.
    }

    private void Start()
    {
        WireButtonListeners();
        ResetGame();
    }

    // Input Wiring.

    private void WireButtonListeners()
    {
        for (int i = 0; i < boardUI.CellButtons.Length; i++)
        {
            int index = i; // Capture for closure.
            boardUI.CellButtons[i].onClick.AddListener(() => OnCellClicked(index));
        }

        if (boardUI.ResetButton != null)
            boardUI.ResetButton.onClick.AddListener(ResetGame);

        if (boardUI.MenuButton != null)
            boardUI.MenuButton.onClick.AddListener(GameSceneManager.LoadMainMenu);
    }

    // Game Flow.

    private void OnCellClicked(int index)
    {
        if (!gameActive || !playerTurn || !board.IsCellEmpty(index))
            return;

        ExecuteMove(index, TicTacToeBoard.Player);

        if (CheckForGameEnd()) return;

        // Hand control to the AI with a short delay.
        playerTurn = false;
        boardUI.SetStatus("AI is thinking...");
        Invoke(nameof(ExecuteAIMove), aiMoveDelay);
    }

    private void ExecuteAIMove()
    {
        if (!gameActive) return;

        int bestMove = solver.FindBestMove(TicTacToeBoard.AI);

        if (bestMove == -1)
        {
            EndGame(TicTacToeBoard.Empty);
            return;
        }

        ExecuteMove(bestMove, TicTacToeBoard.AI);

        if (CheckForGameEnd()) return;

        playerTurn = true;
        boardUI.SetStatus("Your turn!");
    }

    // Places a move on the board model and syncs the UI.
    private void ExecuteMove(int index, int player)
    {
        board.PlaceMove(index, player);
        boardUI.UpdateCell(index, player);
    }

    // End Conditions: Checks if the game has ended (win or draw). Returns true if it has.

    private bool CheckForGameEnd()
    {
        int winner = board.EvaluateWinner();

        if (winner != TicTacToeBoard.Empty)
        {
            EndGame(winner);
            return true;
        }

        if (board.IsFull())
        {
            EndGame(TicTacToeBoard.Empty);
            return true;
        }

        return false;
    }

    private void EndGame(int result)
    {
        gameActive = false;

        // Record the result and refresh the scoreboard.
        stats.RecordResult(result);
        boardUI.UpdateScoreboard(stats);

        string message = result switch
        {
            TicTacToeBoard.AI => "AI wins!",
            TicTacToeBoard.Player => "You win! (Impressive!)",
            _ => "It's a draw!"
        };

        boardUI.SetStatus(message);
    }

    // Reset Board for a new round while preserving stats.

    public void ResetGame()
    {
        board.Reset();
        boardUI.ClearBoard();
        boardUI.SetStatus("Your turn!");
        boardUI.UpdateScoreboard(stats);

        gameActive = true;
        playerTurn = true;
    }
}
