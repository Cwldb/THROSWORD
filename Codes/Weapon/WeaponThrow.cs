using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponThrow : MonoSingleton<WeaponThrow>
{
    public UnityEvent<int> OnThrowWeapon;
    public UnityEvent OnCharge;
    public UnityEvent OnMaxChargeEvent;

    public SpriteRenderer _sprite;
    private Player _player;
    private GetWeapon _currentWeaponData;
    private Image _weaponInfoIcon;
    private SettingUI _setting;

    private AudioSource _clip;

    public int _currentWeaponId;
    public bool isOwnWeapon = false;
    public bool isPickUp = false;
    public bool isCharging = false;
    public bool _isCharge = false;
    private float _chargeValue = 0;

    private bool _minThrow = false;

    private bool _isMaxCharge;

    private Sequence _sequence;


    private void Awake()
    {
        _setting = GameObject.Find("Setting").GetComponent<SettingUI>();
        _player = GetComponentInParent<Player>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _weaponInfoIcon = GameObject.Find("WeaponIcon").GetComponent<Image>();
        _currentWeaponData = GameObject.Find("CheckWeapon").GetComponent<GetWeapon>();
    }

    private void Start()
    {
        isOwnWeapon = true;
        _sprite.enabled = false;
        _weaponInfoIcon.enabled = false;
    }

    private void Update()
    {
        if (!isOwnWeapon && !_setting._isMovingPanel && !_setting._isPanelVisible)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_isCharge)
                {
                    SoundManager.Instance.PlaySound("ChargeSword");
                    _clip = GameObject.Find("ChargeSword Sound").GetComponent<AudioSource>();
                    _isCharge = true;
                }

                ChargeWeapon();
                OnCharge?.Invoke();
            }
            else if (Input.GetMouseButtonUp(0) && !_minThrow)
                ThrowWeapon();
        }
    }

    private void ChargeWeapon()
    {
        if (_chargeValue < 400)
        {
            CameraShake.Instance.ShakeCameraSmall();
            _clip.pitch = 1.4f;
            _chargeValue += Time.deltaTime * 1000;
        }
        else if (_chargeValue >= 400 && _chargeValue <= 700)
        {
            CameraShake.Instance.ShakeCameraMiddle();
            _clip.pitch = 1.6f;
            _chargeValue += Time.deltaTime * 1300;
        }
        else if (_chargeValue >= 700)
        {
            _clip.pitch = 2f;
            _chargeValue += Time.deltaTime * 2000;
            if (!_isMaxCharge)
            {
                OnMaxChargeEvent?.Invoke();
                _isMaxCharge = true;
            }
        }
        transform.localRotation = Quaternion.Euler(0, 0, _chargeValue);
        _player._moveSpeed = 5;
    }

    private void ThrowWeapon()
    {
        SoundManager.Instance.PlaySound("ThrowSword");
        if (_chargeValue < 300)
        {
            _player._moveSpeed = 5;
            isOwnWeapon = true;
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 640f), 0.6f, RotateMode.FastBeyond360));
            _sequence.OnComplete(() => DisableThrow());
        }
        else
        {
            isOwnWeapon = true;
            DisableThrow();
        }
    }
    private void DisableThrow()
    {
        _isMaxCharge = false;
        _sprite.enabled = false;
        _weaponInfoIcon.enabled = false;
        CameraShake.Instance.BigShake();
        _player._moveSpeed = 8;
        _isCharge = false;
        _chargeValue = 0;
        transform.localRotation = Quaternion.Euler(0, 0, _chargeValue);
        Destroy(GameObject.Find("ChargeSword Sound"));
        OnThrowWeapon?.Invoke(_currentWeaponId);
        StartCoroutine(CameraStopShake());
    }

    private IEnumerator CameraStopShake()
    {
        yield return new WaitForSeconds(0.2f);
        CameraShake.Instance.StopShake();
    }

    public void Test()
    {
        Debug.Log("dd");
    }
}
