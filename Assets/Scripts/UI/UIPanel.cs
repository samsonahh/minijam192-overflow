using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    /// <summary>
    /// Use this instead of Awake because UIPanels are disabled at startup. Awake won't be called properly.
    /// </summary>
    public abstract void Init();

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
