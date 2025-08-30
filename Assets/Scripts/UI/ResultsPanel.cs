using Eflatun.SceneReference;
using UnityEngine;

public class ResultsPanel : MonoBehaviour
{
    [SerializeField] private SceneReference menuScene;

    public void Replay()
    {
        FadeManager.Instance.FadeToScene(Utils.GetCurrentScene());
    }

    public void ReturnToMenu()
    {
        FadeManager.Instance.FadeToScene(menuScene);
    }
}