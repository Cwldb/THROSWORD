using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    public Action OnDeathEvent;

    [field: SerializeField] public InputReader inputReader { get; private set; }

    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashDuration = 1f;
    [SerializeField] private float _dashCoolTime = 1f;

    public float _moveSpeed;
    public bool _isDie;
    private bool _hit;
    public bool _canDash;
    public bool _isDashing;

    private Health _health;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private PlayerTrail _trail;

    private AudioSource _clip;

    private Vector2 _moveDir;

    private void Awake()
    {
        _trail = FindAnyObjectByType<PlayerTrail>();
        _health = GameObject.Find("HP").GetComponent<Health>();
        inputReader.OnMove += GetMoveValue;

        _health.OnDeath += Die;
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _trail.Active = false;
    }

    private void GetMoveValue(Vector2 dir)
    {
        _moveDir = dir.normalized;
    }

    private void FixedUpdate()
    {
        if (_isDashing)
            return;
        Move();
    }

    private void Update()
    {
        if (_isDashing)
            return;

        Flip();

        if(Input.GetKeyDown(KeyCode.Space) && !_canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Move()
    {
        if (!WeaponThrow.Instance.isPickUp && !_isDie)
        {
            CheckAnim();
            _rigid.velocity = _moveDir * _moveSpeed;
        }
        else
        {
            _rigid.velocity = Vector2.zero;
        }
    }

    private IEnumerator Dash()
    {
        SoundManager.Instance.PlaySound("Dash");
        _clip = GameObject.Find("Dash Sound").GetComponent<AudioSource>();

        _clip.pitch = 2.7f;

        _canDash = true;
        _isDashing = true;
        _trail.Active = true;
        _rigid.velocity = new Vector2(_moveDir.x * _dashSpeed, _moveDir.y * _dashSpeed);
        yield return new WaitForSeconds(_dashDuration);
        Destroy(GameObject.Find("Dash Sound"));
        _isDashing = false;
        _trail.Active = false;
        yield return new WaitForSeconds(_dashCoolTime);
        _canDash = false;
    }

    private void Flip()
    {
        if (_rigid.velocity.x < 0)
            transform.localScale = new Vector3(-1,1,1);
        else if (_rigid.velocity.x > 0)
            transform.localScale = Vector3.one;
    }

    private void Die()
    {
        _isDie = true;
        OnDeathEvent?.Invoke();
        _anim.SetBool("Run", false);
        _anim.SetBool("Idle", false);
        _anim.SetBool("Die", true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!_hit)
            {
                if (collision.gameObject.TryGetComponent(out CommonMob monster))
                {
                    _health.GetHit((int)monster._dmg);
                    _hit = true;
                    StartCoroutine(HitDelay());
                }
            }

        }
    }

    private IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(0.75f);
        _hit = false;
    }

    private void CheckAnim()
    {
        if (_moveDir == Vector2.zero)
            _anim.SetBool("Run", false);
        else
            _anim.SetBool("Run", true);
    }

    private void OnDisable()
    {
        inputReader.OnMove -= GetMoveValue;
    }
}
