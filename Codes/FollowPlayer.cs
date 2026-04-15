using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _playerTrm;

    private void Awake()
    {
        _playerTrm = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (_playerTrm != null)
        {
            transform.position = new Vector2(_playerTrm.position.x, _playerTrm.position.y - 1.3f);
        }
    }
}
