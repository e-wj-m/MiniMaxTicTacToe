using UnityEngine.SceneManagement;

// Static helper for scene navigation. -Eric M.

public static class GameSceneManager
{
    private const string MainMenuScene = "MainMenu";
    private const string GameScene = "TicTacToeGame";
    private const string AIvsAIScene = "AIVsAIGame";

    public static void LoadMainMenu() => SceneManager.LoadScene(MainMenuScene);

    public static void LoadGame() => SceneManager.LoadScene(GameScene);

    public static void LoadAIvsAI() => SceneManager.LoadScene(AIvsAIScene);

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }
}

