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
}

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged = delegate { };

    private void Start()
    {
        ChangeState(GameState.Playing, true);
    }

    public void ChangeState(GameState newState, bool force = false)
    {
        if(CurrentState == newState && !force)
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
                UIManager.Instance.HideAllPanels();
                InputManager.Instance.EnablePlayerActions();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                UIManager.Instance.ShowPanel(PanelType.Pause);
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                UIManager.Instance.ShowPanel(PanelType.Results);
                break;
        }
    }
}