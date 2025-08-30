using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.ChangeState(GameState.Playing);
    }
}