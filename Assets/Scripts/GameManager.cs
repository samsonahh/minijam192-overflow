using Eflatun.SceneReference;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Win,
    MainMenu,
    Loading,
}

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged = delegate { };

    public void ChangeState(GameState newState)
    {
        if(CurrentState == newState)
            return;

        CurrentState = newState;
        EnterState(newState);

        OnGameStateChanged?.Invoke(newState);
    }

    private void EnterState(GameState state)
    {
        switch(state)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                InputManager.Instance.EnablePlayerActions();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                InputManager.Instance.EnableUIActions();
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                InputManager.Instance.EnableUIActions();
                break;
            case GameState.Win:
                Time.timeScale = 0f;
                InputManager.Instance.EnableUIActions();
                break;
            case GameState.MainMenu:
                Time.timeScale = 0f;
                InputManager.Instance.EnableUIActions();
                break;
            case GameState.Loading:
                Time.timeScale = 1f;
                InputManager.Instance.DisableAllActions();
                break;
        }
    }

    public IEnumerator SwitchScenes(SceneReference newScene, GameState afterState)
    {
        ChangeState(GameState.Loading);

        yield return SceneManager.LoadSceneAsync(newScene.Name, LoadSceneMode.Single);

        ChangeState(afterState);
    }
}