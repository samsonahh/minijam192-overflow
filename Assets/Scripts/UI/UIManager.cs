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
    [field: SerializeField, ReadOnly] public UIPanel CurrentPanel { get; private set; }

    [SerializeField, SerializedDictionary("Panel Type", "Panel Object")]
    private SerializedDictionary<PanelType, UIPanel> panels = new();

    private protected override void Awake()
    {
        base.Awake();
        foreach (UIPanel panel in panels.Values)
            panel.Init();
    }

    private void Start()
    {
        HideAllPanels();
    }

    public void ShowPanel(PanelType panelType)
    {
        UIPanel panel = panels[panelType];

        if (CurrentPanel == panel)
            return;

        if (CurrentPanel != null && CurrentPanel != panel)
            CurrentPanel.Hide();

        CurrentPanel = panel;
        CurrentPanel.Show();

        InputManager.Instance.EnableUIActions();
    }

    public void HideAllPanels()
    {
        foreach (UIPanel panel in panels.Values)
            panel.Hide();

        CurrentPanel = null;
    }
}
