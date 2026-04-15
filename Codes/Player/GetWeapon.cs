using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GetWeapon : MonoSingleton<GetWeapon>
{
    public UnityEvent OnPickUpSword;

    [SerializeField] private GameObject _getUI;
    [SerializeField] private GameObject _pickUpObj;

    private Slider _pickUpSlider;

    private SwordDataContains _swordData;
    private Image _weaponInfoIcon;
    private GameObject _destroySword;

    public SwordDataSO _swordDataSO;
    public float dis;
    public bool _canPickUp;


    private bool _isPickUpDuration;
    private float _currentTime;

    public int swordId;
    public Sprite weaponIcon;

    private void Awake()
    {
        _weaponInfoIcon = GameObject.Find("WeaponIcon").GetComponent<Image>();
        _pickUpSlider = _pickUpObj.GetComponent<Slider>();
    }

    private void Start()
    {
        _getUI.SetActive(false);
        _pickUpObj.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_canPickUp)
        {
            if (collision.gameObject.TryGetComponent(out SwordDataContains swordData))
            {
                _getUI.SetActive(true);
                _canPickUp = true;
                _swordData = swordData;
                _destroySword = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.TryGetComponent(out SwordDataContains swordData))
            {
                _getUI.SetActive(false);
                _pickUpObj.SetActive(false);
                _canPickUp = false;
            }

        }
        catch
        {

        }
    }

    private void Update()
    {
        _pickUpSlider.value = _currentTime / _swordDataSO.pickUpDelayTime;

        if (_isPickUpDuration)
            _currentTime += Time.deltaTime;

        if (_currentTime >= _swordDataSO.pickUpDelayTime && _isPickUpDuration)
        {
            _isPickUpDuration = false;
            _currentTime = 0;
            _pickUpSlider.enabled = false;
        }

        if (_swordData != null && _canPickUp)
        {
            if (Input.GetMouseButtonDown(1) && WeaponThrow.Instance.isOwnWeapon && !Player.Instance._canDash && !Player.Instance._isDashing)
            {
                Player.Instance._canDash = true;
                _pickUpObj.SetActive(true);

                _weaponInfoIcon.enabled = true;
                WeaponThrow.Instance.isPickUp = true;
                OnPickUpSword?.Invoke();
                StartCoroutine(OwnCoolTime());
                SoundManager.Instance.PlaySound("GetSword");
            }
        }
    }

    private IEnumerator OwnCoolTime()
    {
        if (_swordData != null)
        {
            _swordDataSO = _swordData.GetSwordDataSO();
            dis = _swordData.gameObject.GetComponent<Sword>().intersection;

            _isPickUpDuration = true;

            yield return new WaitForSeconds(_swordDataSO.pickUpDelayTime);
            _canPickUp = false;
            weaponIcon = _swordData.GetSwordSprite();
            _weaponInfoIcon.sprite = _swordData.GetSwordSprite();
            WeaponThrow.Instance._sprite.sprite = weaponIcon;
            WeaponThrow.Instance._sprite.enabled = true;
            WeaponThrow.Instance._currentWeaponId = _swordData.GetSwordId();
            WeaponThrow.Instance.isOwnWeapon = false;
            WeaponThrow.Instance.isPickUp = false;
            Player.Instance._canDash = false;
            Destroy(_destroySword);
        }
    }
}
