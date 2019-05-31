using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGBehaviour : MonoBehaviour
{
    public string owner;

    private void Awake()
    {
        bool selfDestroy = true;
#if UNITY_EDITOR
    if (SystemInfo.deviceName == owner)
    {

    }
#else
    selfDestroy = true;
#endif
    }
}
