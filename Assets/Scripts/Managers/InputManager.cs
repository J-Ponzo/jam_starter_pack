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

    private Dictionary<AKey, KeyCode> keyBindings = new Dictionary<AKey, KeyCode>();
    private Dictionary<AAxis, string> axisBindings = new Dictionary<AAxis, string>();

    private void Start()
    {

        LoadBindings();
    }

    private void Update()
    {
        KeyCode code = GetCurrentKey();
        if (code != KeyCode.None)
        {
            Debug.Log(code);
        }
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
