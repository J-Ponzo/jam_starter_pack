using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : Singleton<OptionsManager>
{
    protected OptionsManager() { DontDestroyOnLoad(gameObject); }

    public void DEBUGFoo()
    {

    }
}
