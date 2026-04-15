using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectFollowPlayer : MonoBehaviour
{
    private RectTransform _rect;
    private Transform player;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (_rect == null || player == null)
            return;

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(player.position);

        _rect.position = new Vector2(screenPosition.x, screenPosition.y + 60f);
    }
}
