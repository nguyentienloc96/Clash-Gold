using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public bool isNextScene;

    public void GoToScene(int id,UnityAction actionLoadScenesDone = null, UnityAction actionLoadScenesLast = null)
    {
        StartCoroutine(GoToSceneHandel(id,actionLoadScenesDone, actionLoadScenesLast));
    }

    private IEnumerator GoToSceneHandel(int id,UnityAction actionLoadScenesDone = null, UnityAction actionLoadScenesLast = null)
    {
        Fade.Instance.StartFade(id);
        yield return new WaitUntil(() => Fade.Instance.state == Fade.FadeState.FadeInDone);
        Debug.Log(1);
        if (actionLoadScenesDone != null)
            actionLoadScenesDone();
        Debug.Log(2);
        yield return new WaitForSeconds(1f);
        Debug.Log(3);
        yield return new WaitUntil(() => isNextScene = true);
        Debug.Log(4);
        Fade.Instance.EndFade(id);
        if (actionLoadScenesLast != null)
            actionLoadScenesLast();
    }
}
