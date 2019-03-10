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

    public void GoToScene(UnityAction actionLoadScenesDone = null)
    {
        StartCoroutine(GoToSceneHandel(actionLoadScenesDone));
    }

    private IEnumerator GoToSceneHandel(UnityAction actionLoadScenesDone = null)
    {
        Fade.Instance.StartFade();
        yield return new WaitUntil(() => Fade.Instance.state == Fade.FadeState.FadeInDone);

        if (actionLoadScenesDone != null)
            actionLoadScenesDone();
        yield return new WaitForSeconds(1f);
        //
        yield return new WaitUntil(() => isNextScene = true);
        Fade.Instance.EndFade();
    }
}
