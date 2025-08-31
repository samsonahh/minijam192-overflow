using Eflatun.SceneReference;
using UnityEngine;

public class ResultsPanel : UIPanel
{
    [SerializeField] private SceneReference menuScene;

    public override void Init()
    {
        
    }

    public void Replay()
    {
        FadeManager.Instance.FadeToScene(Utils.GetCurrentScene());
    }

    public void ReturnToMenu()
    {
        FadeManager.Instance.FadeToScene(menuScene);
    }
}