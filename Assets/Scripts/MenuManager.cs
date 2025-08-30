using Eflatun.SceneReference;
using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SceneReference gameScene;

    public void StartGame()
    {
        FadeManager.Instance.FadeToScene(gameScene);
    }
}
