using UnityEngine;
using DG.Tweening;

public class Fade : MonoBehaviour {

    public static Fade Instance;
    private Vector3 sizeObject;

    public enum FadeState
    {
        None, FadeInDone, FadeOutDone
    }

    public FadeState state;

    void Awake()
    {
        Instance = this;
        sizeObject = new Vector3(0.015625f, 0.015625f, 0.015625f);
    }

    public void StartFade()
    {
        transform.localScale = sizeObject;
        FadeInDone();
    }

    public void EndFade()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        state = FadeState.None;
    }

    public void FadeInDone()
    {
        state = FadeState.FadeInDone;
    }

    public void FadeOutDone()
    {
        state = FadeState.FadeOutDone;
    }

}
