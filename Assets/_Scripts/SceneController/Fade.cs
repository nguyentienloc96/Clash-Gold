using UnityEngine;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    public static Fade Instance;
    private GameObject[] arrChild;

    public enum FadeState
    {
        None, FadeInDone, FadeOutDone
    }

    public FadeState state;

    void Awake()
    {
        Instance = this;
        arrChild = new GameObject[transform.GetChild(0).childCount];
        for(int i = 0; i < arrChild.Length; i++)
        {
            arrChild[i] = transform.GetChild(0).GetChild(i).gameObject;
        }
    }

    public void StartFade(int id)
    {
        arrChild[id].SetActive(true);
        transform.localScale = new Vector3(1f, 1f, 1f);
        FadeInDone();
    }

    public void EndFade(int id)
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        arrChild[id].SetActive(false);
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
