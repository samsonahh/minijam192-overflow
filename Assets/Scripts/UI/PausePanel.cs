using Eflatun.SceneReference;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PausePanel : UIPanel
{
    [SerializeField] private SceneReference menuScene;
    [SerializeField] private GameObject settingsContainer;

    private List<Setting> settings = new();

    public override void Init()
    {
        settings = settingsContainer.GetComponentsInChildren<Setting>(true).ToList();
        foreach(Setting setting in settings)
            setting.Load();
    }

    public void Resume()
    {
        GameManager.Instance.ChangeState(GameState.Playing);
    }

    public void ReturnToMenu()
    {
        FadeManager.Instance.FadeToScene(menuScene);
    }
}