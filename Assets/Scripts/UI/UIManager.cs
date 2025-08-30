using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum PanelType
{
    Pause,
    Win,
    Lose,
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField, SerializedDictionary("Panel Type", "Panel Object")]
    private SerializedDictionary<PanelType, GameObject> panels = new();

    private void Start()
    {
        HideAllPanels();
    }

    public void ShowPanel(PanelType panelType)
    {
        HideAllPanels();

        if (panels.TryGetValue(panelType, out var panelToShow))
        {
            panelToShow.SetActive(true);
            return;
        }

        Debug.LogWarning($"Panel of type {panelType} not found!");
    }

    public void HideAllPanels()
    {
        foreach (var panel in panels)
            panel.Value.SetActive(false);
    }
}
