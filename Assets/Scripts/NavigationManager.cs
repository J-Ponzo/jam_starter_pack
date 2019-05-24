using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : Singleton<NavigationManager>
{
    protected NavigationManager() {}

    public string sceneToLoad;
    public bool isLoading = false;
    public bool isOldSceneFadingOut = false;
    public bool isNewSceneFadingIn = false;
    public bool isLoadingSceneFadingIn = false;
    public bool isLoadingSceneFadingOut = false;

    public float fadingSpeed;
    public float oldSceneFadeVal;
    public float newSceneFadeVal;
    public float loadSceneFadeVal;

    private void Update()
    {
        if (!isLoading) return;
        if ()
    }

    public void LoadScene(string nextSceneName)
    {
        if (isLoading)
        {
            Debug.LogWarning("LoadScene(" + nextSceneName + ") ignored because invoked while " + sceneToLoad + " is already loading.");
        }

        sceneToLoad = nextSceneName;
        
    }
}
