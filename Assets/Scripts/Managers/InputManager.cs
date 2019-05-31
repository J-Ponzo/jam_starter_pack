using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    protected InputManager() { }

    public enum AKey
    {
        Submit,
        Cancel,
        Start
    }

    public enum AAxis
    {
        HMove,
        VMove
    }

    private KeyCode[] validKeyCodes = (KeyCode[]) Enum.GetValues(typeof(KeyCode));
    private string[] validAxis =
    {
        "XAxis",
        "YAxis",
        "3Axis",
        "4Axis",
        "5Axis",
        "6Axis",
        "7Axis",
        "8Axis",
        "9Axis",
        "10Axis",
        "11Axis",
        "12Axis",
        "13Axis",
        "14Axis",
        "15Axis",
        "16Axis",
        "17Axis",
        "18Axis",
        "19Axis",
        "20Axis",
        "21Axis",
        "22Axis",
        "23Axis",
        "24Axis",
        "25Axis",
        "26Axis",
        "27Axis",
        "28Axis"
    };

    private Dictionary<AKey, KeyCode> keyBindings = new Dictionary<AKey, KeyCode>();
    private Dictionary<AAxis, string> axisBindings = new Dictionary<AAxis, string>();

    private void Start()
    {
        LoadBindings();
    }

    private void LoadBindings()
    {
        keyBindings.Clear();
        axisBindings.Clear();

        foreach (string name in Enum.GetNames(typeof(AKey)))
        {
            string keyCode_str = OptionsManager.Instance.GetString(name + "_KeyBinding");
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyCode_str);
            AKey aKey = (AKey)Enum.Parse(typeof(AKey), name);
            keyBindings.Add(aKey, keyCode);
        }

        foreach (string name in Enum.GetNames(typeof(AAxis)))
        {
            string axis_str = OptionsManager.Instance.GetString(name + "_AxisBinding");
            AAxis aAxis = (AAxis)Enum.Parse(typeof(AAxis), name);
            axisBindings.Add(aAxis, axis_str);
        }
    }

    public void SaveBindings()
    {
        OptionsManager.Instance.Save();
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

    private string GetCurrentAxis()
    {

        foreach (string axis in validAxis)
        {
            if (Mathf.Abs(Input.GetAxis(axis)) > 0.75f)
            {
                return axis;
            }
        }
        return null;
    }

    private  KeyCode GetCurrentKey()
    {
        foreach (KeyCode keyCode in validKeyCodes)
        {
            if (Input.GetKey(keyCode))
            {
                return keyCode;
            }
        }
        return KeyCode.None;
    }
}
