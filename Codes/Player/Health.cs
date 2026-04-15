using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoSingleton<Health>
{
    public event Action OnHpChage;
    public event Action OnDeath;

    [SerializeField] private int _maxHealth;
    private float _currentHealth;

    [SerializeField] private Image _hpFill;

    private void Awake()
    {
        OnHpChage += SetHp;
    }

    private void Update()
    {
        if (_currentHealth >= 100)
        {
            _currentHealth = 100;
        }
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        SetHp();
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        SoundManager.Instance.PlaySound("Hit");
        OnHpChage?.Invoke();
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            OnDeath?.Invoke();
        }
    }

    private void SetHp()
    {
        _hpFill.fillAmount = _currentHealth / _maxHealth;
    }
}
