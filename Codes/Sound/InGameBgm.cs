using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameBgm : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlaySound("InGameBG");
    }
}
