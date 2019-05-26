using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    protected InputManager() { }

    public enum AKey
    {
        Select,
        Back,
        Start
    }

    public enum AAxis
    {
        HMove,
        VMove
    }

    private Dictionary<AKey, KeyCode> keyBindings;
    private Dictionary<AAxis, string> axisBindings;

    private void Start()
    {
        foreach (string name in Enum.GetNames(typeof(AKey)))
        {
            string keyCode_str = OptionsManager.Instance.GetString(name + "_KeyBinging");
            KeyCode keyCode = (KeyCode) Enum.Parse(typeof(KeyCode), keyCode_str);
            AKey aKey = (AKey)Enum.Parse(typeof(AKey), name);
            keyBindings.Add(aKey, keyCode);
        }

        foreach (string name in Enum.GetNames(typeof(AAxis)))
        {
            string axis_str = OptionsManager.Instance.GetString(name + "_AxisBinging");
            AAxis aAxis = (AAxis)Enum.Parse(typeof(AAxis), name);
            axisBindings.Add(aAxis, axis_str);
        }
    }

    public float GetAxis(AAxis aAxis)
    {
        return Input.GetAxis(axisBindings[aAxis]);
    }

    public bool GetKeyDown(AKey aKey)
    {
        return Input.GetKeyDown(keyBindings[aKey]);
    }

    public bool GetKeyUp(AKey aKey)
    {
        return Input.GetKeyUp(keyBindings[aKey]);
    }

    public bool GetKey(AKey aKey)
    {
        return Input.GetKey(keyBindings[aKey]);
    }
}
