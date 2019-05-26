using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavigationManager : Singleton<NavigationManager>
{
    protected NavigationManager() {}

    private static object m_Lock = new object();

    [Header("ReadOnly")]
    public bool isLoading;
    public string oldScene;
    public string newScene;
    public float timeElapsed;
    public Image blackScreen;

    [Header("Switch Params.")]
    /// <summary>
    /// fading speed parameter
    /// </summary>
    public float fadeSpeed;
    /// <summary>
    /// The loading screen is displayed only if the new scene loading last more than this time
    /// </summary>
    public float loadingTimeForDisplay;
    /// <summary>
    /// If the login screen is displayed, it stay at least this time
    /// </summary>
    public float loadingStayTime;

    private IEnumerator FadeToBlack()
    {
        Color color;
        while (blackScreen.color.a < 1f)
        {
            color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + Time.deltaTime * fadeSpeed);
            blackScreen.color = color;
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
    }

    private IEnumerator FadeFromBlack()
    {
        Color color;
        while (blackScreen.color.a > 0f)
        {
            color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - Time.deltaTime * fadeSpeed);
            blackScreen.color = color;
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0f);
    }

    private IEnumerator CountTime()
    {
        while (timeElapsed < loadingTimeForDisplay)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = loadingTimeForDisplay;
    }

    private IEnumerator AsyncLoadScene()
    {
        //Reset time counter
        timeElapsed = 0;
        StartCoroutine("CountTime");

        //Fade to black
        yield return StartCoroutine("FadeToBlack");

        //Load Loading
        AsyncOperation asyncLoadLoading = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        while (!asyncLoadLoading.isDone)
        {
            yield return null;
        }

        //Load/Unload Scenes
        AsyncOperation asyncUnloadOld = SceneManager.UnloadSceneAsync(oldScene);
        AsyncOperation asyncLoadNew = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        Scene scene;
        scene = SceneManager.GetSceneByName(oldScene + "_Env");
        bool oldSceneEnvExists = scene.buildIndex > -1;
        AsyncOperation asyncUnloadOld_Env = null;
        if (oldSceneEnvExists)
            asyncUnloadOld_Env = SceneManager.UnloadSceneAsync(scene);

        scene = SceneManager.GetSceneByName(oldScene + "_UI");
        bool oldSceneUIExists = scene.buildIndex > -1;
        AsyncOperation asyncUnloadOld_UI = null;
        if (oldSceneUIExists)
            asyncUnloadOld_UI = SceneManager.UnloadSceneAsync(oldScene + "_UI");

        scene = SceneManager.GetSceneByName(newScene + "_Env");
        bool newSceneEnvExists = scene.buildIndex > -1;
        AsyncOperation asyncLoadNew_Env = null;
        if (newSceneEnvExists)
            asyncLoadNew_Env = SceneManager.LoadSceneAsync(newScene + "_Env", LoadSceneMode.Additive);

        scene = SceneManager.GetSceneByName(newScene + "_UI");
        bool newSceneUIExists = scene.buildIndex > -1;
        AsyncOperation asyncLoadNew_UI = null;
        if (newSceneUIExists)
            asyncLoadNew_UI = SceneManager.LoadSceneAsync(newScene + "_UI", LoadSceneMode.Additive);

        while (!(asyncUnloadOld.isDone && asyncLoadNew.isDone
            && (!oldSceneEnvExists || asyncUnloadOld_Env.isDone)
            && (!oldSceneUIExists || asyncUnloadOld_UI.isDone)
            && (!newSceneEnvExists || asyncLoadNew_Env.isDone)
            && (!newSceneUIExists || asyncLoadNew_UI.isDone)))
        {
            if (timeElapsed == loadingTimeForDisplay)
            {
                yield return StartCoroutine("FadeFromBlack");
                yield return new WaitForSeconds(loadingStayTime);
                yield return StartCoroutine("FadeToBlack");
            }
            yield return null;
        }

        //Unload Loading
        AsyncOperation asyncUnloadLoading = SceneManager.UnloadSceneAsync("Loading");
        while (!asyncUnloadLoading.isDone)
        {
            yield return null;
        }

        //Fade from black
        yield return StartCoroutine("FadeFromBlack");

        //Setup thing for next call
        oldScene = newScene;
        isLoading = false;
    }

    public void LoadScene(string nextSceneName)
    {
        lock (m_Lock)
        {
            if (isLoading)
            {
                Debug.LogWarning("LoadScene(" + nextSceneName + ") ignored because invoked while " + newScene + " is already loading.");
                return;
            }

            newScene = nextSceneName;
            isLoading = true;
        }

        StartCoroutine("AsyncLoadScene");
    }
}
