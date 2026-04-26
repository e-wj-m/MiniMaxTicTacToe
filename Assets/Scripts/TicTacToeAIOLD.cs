using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Unbeatable Tic Tac Toe AI using the Minimax algorithm.
/// 
/// SETUP:
/// 1. Create a Canvas with 9 UI Buttons in a 3x3 grid (each with a TextMeshProUGUI child for X/O).
/// 2. Create a TextMeshProUGUI element for the status message (e.g., "Your turn", "AI wins!").
/// 3. Create a "Reset" Button (optional — can also reset via status click or key).
/// 4. Attach this script to an empty GameObject.
/// 5. Drag your 9 cell buttons into the 'cellButtons' array in the Inspector (index 0=top-left, row by row).
/// 6. Drag your status text into 'statusText'.
/// 7. Drag your reset button into 'resetButton'.
/// </summary>
public class TicTacToeAI : MonoBehaviour
{
    [Header("Board Setup")]
    [Tooltip("Assign all 9 cell buttons in order: top-left to bottom-right, row by row.")]
    [SerializeField] private Button[] cellButtons = new Button[9];

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button resetButton;

    [Header("Display Settings")]
    [SerializeField] private Color xColor = new Color(0.2f, 0.6f, 1f);   // Blue
    [SerializeField] private Color oColor = new Color(1f, 0.35f, 0.35f);  // Red
    [SerializeField] private Color defaultColor = new Color(0.7f, 0.7f, 0.7f);

    // Board state: 0 = empty, 1 = AI (X), -1 = Player (O)
    private int[] board = new int[9];
    private bool gameActive = true;
    private bool playerTurn = true;

    // All possible winning lines
    private static readonly int[][] WinLines = new int[][]
    {
        new int[] {0, 1, 2}, // Top row
        new int[] {3, 4, 5}, // Middle row
        new int[] {6, 7, 8}, // Bottom row
        new int[] {0, 3, 6}, // Left column
        new int[] {1, 4, 7}, // Middle column
        new int[] {2, 5, 8}, // Right column
        new int[] {0, 4, 8}, // Diagonal top-left to bottom-right
        new int[] {2, 4, 6}  // Diagonal top-right to bottom-left
    };

    private void Start()
    {
        // Wire up each cell button with its index
        for (int i = 0; i < cellButtons.Length; i++)
        {
            int index = i; // Capture for closure
            cellButtons[i].onClick.AddListener(() => OnCellClicked(index));
        }

        if (resetButton != null)
            resetButton.onClick.AddListener(ResetGame);

        ResetGame();
    }

    /// <summary>
    /// Called when the player clicks a board cell.
    /// </summary>
    private void OnCellClicked(int index)
    {
        if (!gameActive || !playerTurn || board[index] != 0)
            return;

        // Place the player's move
        MakeMove(index, -1);

        // Check for game over after player's move
        int result = CheckWin(board);
        if (result != 0)
        {
            EndGame(result);
            return;
        }

        // Check for draw (board full)
        if (IsBoardFull())
        {
            EndGame(0);
            return;
        }

        // AI's turn
        playerTurn = false;
        statusText.text = "AI is thinking...";

        // Small delay so the player can see their move before AI responds
        Invoke(nameof(AIMove), 0.3f);
    }

    /// <summary>
    /// Executes the AI's move using Minimax.
    /// </summary>
    private void AIMove()
    {
        if (!gameActive) return;

        int bestMove = GetBestMove(board);

        if (bestMove == -1)
        {
            EndGame(0);
            return;
        }

        MakeMove(bestMove, 1);

        int result = CheckWin(board);
        if (result != 0)
        {
            EndGame(result);
            return;
        }

        if (IsBoardFull())
        {
            EndGame(0);
            return;
        }

        playerTurn = true;
        statusText.text = "Your turn!";
    }

    // --------------------------------------------------
    //  MINIMAX ALGORITHM (converted from your C++ code)
    // --------------------------------------------------

    /// <summary>
    /// Checks all win conditions. Returns 1 if AI won, -1 if player won, 0 if no winner yet.
    /// Equivalent to your C++ win() function.
    /// </summary>
    private int CheckWin(int[] b)
    {
        for (int i = 0; i < WinLines.Length; i++)
        {
            int a0 = WinLines[i][0];
            int a1 = WinLines[i][1];
            int a2 = WinLines[i][2];

            if (b[a0] != 0 && b[a0] == b[a1] && b[a1] == b[a2])
                return b[a2];
        }

        return 0;
    }

    /// <summary>
    /// Recursive Minimax evaluation.
    /// Returns a score relative to the current player: positive = good, negative = bad.
    /// Equivalent to your C++ minimax() function.
    /// </summary>
    private int Minimax(int[] b, int player)
    {
        int winner = CheckWin(b);
        if (winner != 0)
            return winner * player;

        int move = -1;
        int score = -2;

        for (int i = 0; i < 9; i++)
        {
            if (b[i] == 0)
            {
                b[i] = player;
                int thisScore = -Minimax(b, -player);
                if (thisScore > score)
                {
                    score = thisScore;
                    move = i;
                }
                b[i] = 0; // Undo move
            }
        }

        // No moves left — draw
        if (move == -1)
            return 0;

        return score;
    }

    /// <summary>
    /// Finds the best move for the AI (player = 1).
    /// Equivalent to your C++ computerMove() function.
    /// </summary>
    private int GetBestMove(int[] b)
    {
        int bestMove = -1;
        int bestScore = -2;

        for (int i = 0; i < 9; i++)
        {
            if (b[i] == 0)
            {
                b[i] = 1; // AI places its mark
                int moveScore = -Minimax(b, -1);
                b[i] = 0; // Undo

                if (moveScore > bestScore)
                {
                    bestScore = moveScore;
                    bestMove = i;
                }
            }
        }

        return bestMove;
    }

    // --------------------------------------------------
    //  BOARD & UI HELPERS
    // --------------------------------------------------

    /// <summary>
    /// Places a mark on the board and updates the UI button text.
    /// </summary>
    private void MakeMove(int index, int player)
    {
        board[index] = player;

        TextMeshProUGUI label = cellButtons[index].GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
        {
            label.text = (player == 1) ? "X" : "O";
            label.color = (player == 1) ? xColor : oColor;
        }
    }

    private bool IsBoardFull()
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == 0) return false;
        }
        return true;
    }

    private void EndGame(int result)
    {
        gameActive = false;

        switch (result)
        {
            case 0:
                statusText.text = "It's a draw!";
                break;
            case 1:
                statusText.text = "AI wins!";
                break;
            case -1:
                statusText.text = "You win! (Impressive!)";
                break;
        }
    }

    /// <summary>
    /// Resets the board for a new game.
    /// </summary>
    public void ResetGame()
    {
        board = new int[9];
        gameActive = true;
        playerTurn = true;

        for (int i = 0; i < cellButtons.Length; i++)
        {
            TextMeshProUGUI label = cellButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
            {
                label.text = "";
                label.color = defaultColor;
            }
        }

        if (statusText != null)
            statusText.text = "Your turn!";
    }
}
