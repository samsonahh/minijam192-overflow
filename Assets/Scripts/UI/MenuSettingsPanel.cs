using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuSettingsPanel : UIPanel
{
    [SerializeField] private GameObject settingsContainer;
    private List<Setting> settings = new();

    public override void Init()
    {
        settings = settingsContainer.GetComponentsInChildren<Setting>(true).ToList();
        foreach (Setting setting in settings)
            setting.Load();
    }
}
