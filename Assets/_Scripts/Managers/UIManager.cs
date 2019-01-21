using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Btn_Play()
    {

    }

    public void Btn_Continue()
    {

    }

    public void Btn_Tutorial()
    {

    }

    public void Btn_Share()
    {
        //AudioManager.Instance.Play("Click");
        ShareManager.Instance.ShareScreenshotWithText(GameConfig.Instance.string_Share);
    }

    public void Btn_Rate()
    {
        //AudioManager.Instance.Play("Click");
#if UNITY_ANDROID
        if (GameConfig.Instance.link_android != null)
        {
            Application.OpenURL(GameConfig.Instance.link_android);
        }
#elif UNITY_IOS
        if (GameConfig.Instance.link_ios != null)
        {
            Application.OpenURL(GameConfig.Instance.link_ios);
        }
#endif
    }
}
