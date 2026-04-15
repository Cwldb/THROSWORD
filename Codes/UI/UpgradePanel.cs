using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoSingleton<UpgradePanel>
{
    [SerializeField] private GameObject _upPanel;

    public bool _upPanelVisible;

    private void Start()
    {
        _upPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && _upPanelVisible)
        {
            CloseUpgrade();
            _upPanelVisible = false;
        }
    }

    public void OpenUpgrade()
    {
        _upPanel.SetActive(true);
        _upPanelVisible = true;
    }

    public void CloseUpgrade()
    {
        _upPanel.SetActive(false);
        _upPanelVisible = false;
    }
}
