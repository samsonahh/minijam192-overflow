using Eflatun.SceneReference;
using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SceneReference gameScene;
    [SerializeField] private MenuSettingsPanel settingsPanel;

    private void Start()
    {
        Time.timeScale = 1f;
        settingsPanel.Init();
    }

    public void StartGame()
    {
        FadeManager.Instance.FadeToScene(gameScene);
    }
}
