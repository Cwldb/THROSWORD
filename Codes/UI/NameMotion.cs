using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameMotion : MonoBehaviour
{
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        NameMove();
    }

    private void NameMove()
    {
        _rect.DOMoveY(_rect.position.y - 20f, 0.7f).SetEase(Ease.InSine).SetLoops(-1,LoopType.Yoyo);
    }
}
