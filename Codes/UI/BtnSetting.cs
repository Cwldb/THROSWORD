using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;


public class BtnSetting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _txtSize;

    private TMP_Text _text;

    private Color _oldColor;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        _oldColor = _text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DiscardText();
    }

    public void ChangeText()
    {
        _text.color = Color.red;
        _text.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f);
    }

    public void DiscardText()
    {
        _text.color = _oldColor;
        _text.transform.DOScale(Vector3.one, 0.1f);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TestGame");
    }

    public void OpenSetting()
    {
        SettingUI settingUI = GameObject.Find("Setting").GetComponent<SettingUI>();
        settingUI.OpenSetting();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("TestTitle");
    }
}
