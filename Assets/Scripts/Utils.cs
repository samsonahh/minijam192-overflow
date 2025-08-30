using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static SceneReference GetCurrentScene() => SceneReference.FromScenePath(SceneManager.GetActiveScene().path);
}
