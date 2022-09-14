using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneLoader
{
    public const string MenuSceneName = "Menu";
    public const string LevelOneSceneName = "Level 1";
    public const string LevelTwoSceneName = "Level 2";
    public const string LevelThreeSceneName = "Level 3";
    public const string EndSceneName = "End";

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
