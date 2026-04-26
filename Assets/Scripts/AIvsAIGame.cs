using UnityEngine;
using UnityEngine.UI;

// AI vs AI mode, by Eric M.
// The player watches as a spectator while two Minimax agents endlessly play against each other. No one AI will ever win, always resulting in a draw, but it's fun to watch them make their optimal moves and see the board fill up. 
// Reuses TicTacToeBoardUI for the board display.

[RequireComponent(typeof(TicTacToeBoardUI))]
public class AIvsAIGame : MonoBehaviour
{
    [Header("Playback")]
    [Tooltip("Delay between moves in seconds.")]
    [SerializeField] private float moveDelay = 0.6f;

    [Header("Spectator Controls")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button stepButton;
    [SerializeField] private Button resetButton;

    // Core objects.
    private TicTacToeBoard board;
    private MinimaxSolver solver;
    private TicTacToeBoardUI boardUI;
    private GameStats stats;

    // Playback state.
    private int currentPlayer;
    private bool gameActive;
    private bool isPlaying;     
    private float moveTimer;

    private void Awake()
    {
        boardUI = GetComponent<TicTacToeBoardUI>();

        board = new TicTacToeBoard();
        solver = new MinimaxSolver(board);
        stats = new GameStats("AIvsAI");
    }

    private void Start()
    {
        WireControls();
        ResetMatch();
    }

    private void Update()
    {
        if (!gameActive || !isPlaying) return;

        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0f)
        {
            ExecuteNextMove();
            moveTimer = moveDelay;
        }
    }

    // Control Wiring.

    private void WireControls()
    {
        if (playButton != null)
            playButton.onClick.AddListener(StartAutoPlay);

        if (stepButton != null)
            stepButton.onClick.AddListener(StepOnce);

        if (resetButton != null)
            resetButton.onClick.AddListener(ResetMatch);

        if (boardUI.MenuButton != null)
            boardUI.MenuButton.onClick.AddListener(GameSceneManager.LoadMainMenu);
    }

    // Playback Controls.

    private void StartAutoPlay()
    {
        if (!gameActive)
        {
            ResetMatch();
            return;
        }

        isPlaying = true;
        moveTimer = moveDelay;
    }

    private void StepOnce()
    {
        if (!gameActive) return;

        // Pause auto-play when stepping manually.
        isPlaying = false;
        ExecuteNextMove();
    }

    // Game Flow.

    private void ExecuteNextMove()
    {
        if (!gameActive) return;

        int bestMove = solver.FindBestMove(currentPlayer);

        if (bestMove == -1)
        {
            EndMatch(TicTacToeBoard.Empty);
            return;
        }

        board.PlaceMove(bestMove, currentPlayer);
        boardUI.UpdateCell(bestMove, currentPlayer);

        string playerName = (currentPlayer == TicTacToeBoard.AI) ? "X" : "O";
        int cellDisplay = bestMove + 1;
        boardUI.SetStatus($"{playerName} plays cell {cellDisplay}");

        // Check end conditions.
        int winner = board.EvaluateWinner();
        if (winner != TicTacToeBoard.Empty)
        {
            EndMatch(winner);
            return;
        }

        if (board.IsFull())
        {
            EndMatch(TicTacToeBoard.Empty);
            return;
        }

        // Swap turns.
        currentPlayer = -currentPlayer;
    }

    private void EndMatch(int result)
    {
        gameActive = false;
        isPlaying = false;

        stats.RecordResult(result);
        boardUI.UpdateAIvsAIScoreboard(stats);

        string message = result switch
        {
            TicTacToeBoard.AI => "X wins!",
            TicTacToeBoard.Player => "O wins!",
            _ => "It's a draw! (As expected)"
        };

        boardUI.SetStatus(message);
    }

    // Reset.

    public void ResetMatch()
    {
        board.Reset();
        boardUI.ClearBoard();
        boardUI.UpdateAIvsAIScoreboard(stats);

        currentPlayer = TicTacToeBoard.AI; // X always goes first.
        gameActive = true;
        isPlaying = false;
        moveTimer = moveDelay;

        boardUI.SetStatus("Ready — press Play or Step!");
    }

}
