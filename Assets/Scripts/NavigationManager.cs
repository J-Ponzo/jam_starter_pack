using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : Singleton<NavigationManager>
{
    protected NavigationManager() { DontDestroyOnLoad(gameObject); }

    public void DEBUGFoo()
    {

    }
}
