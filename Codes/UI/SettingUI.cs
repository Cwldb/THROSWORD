using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Image _touchBlocker;
    [SerializeField] private GameObject _bgTouchBlocker;

    private Image _settingPanel;
    private Sequence _sequence;
    public bool _isMovingPanel { get; set; } = false;
    public bool _isPanelVisible { get; private set; } = false;
    private float _oldPositionY;

    private void Awake()
    {
        _settingPanel = GetComponent<Image>();
    }

    private void Start()
    {
        DOTween.KillAll();
        _touchBlocker.enabled = true;
        _bgTouchBlocker.SetActive(false);
        _oldPositionY = -1030f;
    }

    private void Update()
    {
        if (!_isMovingPanel && Input.GetKeyDown(KeyCode.Escape) && !Player.Instance._isDie && !WeaponThrow.Instance._isCharge)
        {
            if (_isPanelVisible)
            {
                EventSystem.current.SetSelectedGameObject(null);
                CloseSetting();
            }
            else
                OpenSetting();
        }
    }

    private void ShowPanel()
    {
        _isMovingPanel = true;
        _sequence = DOTween.Sequence()
            .SetUpdate(true)
            .Append(_settingPanel.rectTransform.DOLocalMoveY(0, 0.3f))
            .AppendCallback(() => _isPanelVisible = true)
            .OnComplete(() => _isMovingPanel = false);

        _touchBlocker.enabled = false;
        _bgTouchBlocker.SetActive(true);
        Time.timeScale = 0;
    }

    private void HidePanel()
    {
        _isMovingPanel = true;
        _sequence = DOTween.Sequence()
            .SetUpdate(true)
            .Append(_settingPanel.rectTransform.DOLocalMoveY(_oldPositionY, 0.6f).SetEase(Ease.InBack))
            .AppendCallback(() => _isPanelVisible = false)
            .AppendCallback(() => Time.timeScale = 1)
            .OnComplete(() => _isMovingPanel = false);

        _touchBlocker.enabled = true;
        _bgTouchBlocker.SetActive(false);
    }

    public void OpenSetting()
    {
        if (!_isMovingPanel && !_isPanelVisible)
            ShowPanel();
    }

    public void CloseSetting()
    {
        if (!_isMovingPanel && _isPanelVisible)
            HidePanel();
    }
}