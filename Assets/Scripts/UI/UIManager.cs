using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public enum PanelType
{
    Pause,
    Results,
}

public class UIManager : Singleton<UIManager>
{
    [field: SerializeField, ReadOnly] public GameObject CurrentPanel { get; private set; }

    [SerializeField, SerializedDictionary("Panel Type", "Panel Object")]
    private SerializedDictionary<PanelType, GameObject> panels = new();

    private void Start()
    {
        HideAllPanels();
    }

    public void ShowPanel(PanelType panelType)
    {
        GameObject panel = panels[panelType];

        if (CurrentPanel == panel)
            return;

        if (CurrentPanel != null && CurrentPanel != panel)
            CurrentPanel.SetActive(false);

        CurrentPanel = panel;
        CurrentPanel.SetActive(true);

        InputManager.Instance.EnableUIActions();
    }

    public void HideAllPanels()
    {
        foreach (var panel in panels)
            panel.Value.SetActive(false);

        CurrentPanel = null;
    }
}
