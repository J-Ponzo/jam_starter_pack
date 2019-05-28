using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : Singleton<OptionsManager>
{
    protected OptionsManager() { }

    public StringOption[] stringPrefs;

    [Serializable]
    public struct StringOption
    {
        public string key;
        public string defaultValue;
    }

    public string GetDefaultString(string key)
    {
        string defaultValue = null;
        foreach (StringOption strOption in stringPrefs)
        {
            if (strOption.key == key) defaultValue = strOption.defaultValue;
        }

        return defaultValue;
    }

    public string GetString(string key)
    {
        string defaultValue = GetDefaultString(key);
        if (defaultValue == null)
        {
            Debug.LogError("Try retrieving string " + key + " from preferences but this key is not registered");
            return null;
        }

        return PlayerPrefs.GetString(key, defaultValue);
    }

    public void SetString(string key, string value)
    {
        string defaultValue = GetDefaultString(key);
        if (defaultValue == null)
        {
            Debug.LogError("Try setting string " + key + " in preferences but this key is not registered");
            return;
        }

        PlayerPrefs.SetString(key, value);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
}
