
// Minimax solver for Tic Tac Toe, by Eric M. 
// Pure C# — no Unity dependencies. Operates on a TicTacToeBoard reference. Again, just for fun! 
// The recursion works by alternating perspectives:
// - Each level tries all empty cells for the current player.
// - Negates the child score (what's good for my opponent is bad for me).
// - Propagates the best score back up the tree.
public class MinimaxSolver
{
    private readonly TicTacToeBoard board;

    public MinimaxSolver(TicTacToeBoard board)
    {
        this.board = board;
    }

    // Returns the index of the best move for the given player, or -1 if no moves are available. 
    public int FindBestMove(int player)
    {
        int bestMove = -1;
        int bestScore = int.MinValue;

        for (int i = 0; i < TicTacToeBoard.Size; i++)
        {
            if (!board.IsCellEmpty(i)) continue;

            board.PlaceMove(i, player);
            int score = -Evaluate(-player); // Negate: opponent's best is our worst.
            board.UndoMove(i);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = i;
            }
        }

        return bestMove;
    }

    // Recursively evaluates the board from the perspective of 'player'. Returns +1 if the current player can force a win, -1 if they lose, 0 for a draw.
    private int Evaluate(int player)
    {
        // Base case: someone already won!
        int winner = board.EvaluateWinner();
        if (winner != TicTacToeBoard.Empty)
            return winner * player; // +1 if winner is current player, -1 if opponent.

        int bestScore = int.MinValue;
        bool hasMove = false;

        for (int i = 0; i < TicTacToeBoard.Size; i++)
        {
            if (!board.IsCellEmpty(i)) continue;

            hasMove = true;

            board.PlaceMove(i, player);
            int score = -Evaluate(-player); // Recurse from opponent's perspective.
            board.UndoMove(i);

            if (score > bestScore)
                bestScore = score;
        }

        // Base case: no moves left — it's a draw.
        return hasMove ? bestScore : 0;
    }
}
