using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject _DeathUI;

    private void Awake()
    {
        Player.Instance.OnDeathEvent += ShowDeathPanel;
    }

    private void Start()
    {
        _DeathUI.SetActive(false);
    }

    private void ShowDeathPanel()
    {
        Time.timeScale = 0;
        _DeathUI.SetActive(true);
    }
}
