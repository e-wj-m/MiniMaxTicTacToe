using UnityEngine;

// TicTacToe GameStats class, by Eric M.
// Tracks and persists win/loss/draw statistics using PlayerPrefs. Survives scene transitions AND full application restarts. Uses a key prefix so multiple independent stat pools can coexist.
// Encapsulates all persistence logic so the rest of the codebase never touches PlayerPrefs directly.

public class GameStats
{
    // Generic labels — each game mode interprets them in its own context.
    public int Side1Wins { get; private set; }
    public int Side2Wins { get; private set; }
    public int Draws { get; private set; }

    private readonly string keySide1;
    private readonly string keySide2;
    private readonly string keyDraws;

    // Creates a stat pool with the given prefix for PlayerPrefs keys.
    public GameStats(string prefix)
    {
        keySide1 = $"{prefix}_Side1Wins";
        keySide2 = $"{prefix}_Side2Wins";
        keyDraws = $"{prefix}_Draws";
        Load();
    }

    // Records the result of a completed game and saves immediately.
    public void RecordResult(int winner)
    {
        switch (winner)
        {
            case TicTacToeBoard.AI:
                Side1Wins++;
                break;
            case TicTacToeBoard.Player:
                Side2Wins++;
                break;
            default:
                Draws++;
                break;
        }

        Save();
    }

    // Resets all stats to zero and clears saved data.
    public void ResetAll()
    {
        Side1Wins = 0;
        Side2Wins = 0;
        Draws = 0;
        Save();
    }

    private void Load()
    {
        Side1Wins = PlayerPrefs.GetInt(keySide1, 0);
        Side2Wins = PlayerPrefs.GetInt(keySide2, 0);
        Draws = PlayerPrefs.GetInt(keyDraws, 0);
    }

    private void Save()
    {
        PlayerPrefs.SetInt(keySide1, Side1Wins);
        PlayerPrefs.SetInt(keySide2, Side2Wins);
        PlayerPrefs.SetInt(keyDraws, Draws);
        PlayerPrefs.Save();
    }
}
