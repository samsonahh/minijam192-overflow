using Eflatun.SceneReference;
using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SceneReference gameScene;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        FadeManager.Instance.FadeToScene(gameScene);
    }
}
