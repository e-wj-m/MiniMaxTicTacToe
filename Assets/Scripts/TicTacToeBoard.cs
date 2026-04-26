// Pure C# representation of a Tic Tac Toe board, by Eric M. No Unity dependencies — testable and reusable anywhere, just for fun really :)

public class TicTacToeBoard
{
    public const int Size = 9;
    public const int Empty = 0;
    public const int AI = 1;
    public const int Player = -1;

    private int[] cells = new int[Size];

    // All eight winning lines (rows, columns, diagonals).
    private static readonly int[][] WinLines =
    {
        new[] {0, 1, 2},  // Rows.
        new[] {3, 4, 5},
        new[] {6, 7, 8},
        new[] {0, 3, 6},  // Columns.
        new[] {1, 4, 7},
        new[] {2, 5, 8},
        new[] {0, 4, 8},  // Diagonals.
        new[] {2, 4, 6}
    };

    // State Queries.

    public int GetCell(int index) => cells[index];

    public bool IsCellEmpty(int index) => cells[index] == Empty;

    public bool IsFull()
    {
        for (int i = 0; i < Size; i++)
        {
            if (cells[i] == Empty) return false;
        }
        return true;
    }

    // Returns the winner (AI or Player), or Empty if no winner yet.
    public int EvaluateWinner()
    {
        foreach (int[] line in WinLines)
        {
            if (cells[line[0]] != Empty &&
                cells[line[0]] == cells[line[1]] &&
                cells[line[1]] == cells[line[2]])
            {
                return cells[line[2]];
            }
        }

        return Empty;
    }

    // Returns true if the game is over (someone won or the board is full).
    public bool IsGameOver() => EvaluateWinner() != Empty || IsFull();

    // State Mutations.

    public void PlaceMove(int index, int player) => cells[index] = player;

    public void UndoMove(int index) => cells[index] = Empty;

    public void Reset() => cells = new int[Size];
}
