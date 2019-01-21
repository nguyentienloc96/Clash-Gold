using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [Header("UI HOME")]
    public GameObject panelHome;
    public GameObject panelMain;

    [Header("UI TOP")]
    public Text txtGold;
    public Text txtCoin;
    public Text txtGoldMount;

    [Header("IN-WALL")]
    public GameObject panelInWall;
    public GameObject panelBuild;
    public List<GameObject> lstHouse;

    public List<string> arrAlphabetNeed = new List<string>();
    private string[] arrAlphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

    public static UIManager Instance = new UIManager();
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < arrAlphabet.Length; j++)
            {
                arrAlphabetNeed.Add(arrAlphabet[i] + arrAlphabet[j]);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        txtGold.text = ConvertNumber(GameManager.Instance.gold);
        txtCoin.text = GameManager.Instance.coin.ToString();
        txtGoldMount.text = "Gold mount: " + GameManager.Instance.goldMount.ToString();
	}

    public string ConvertNumber(double number)
    {
        string smoney = string.Format("{0:#,##0}", number);
        for (int i = 0; i < arrAlphabetNeed.Count; i++)
        {
            if (smoney.Length >= 5 + i * 4 && smoney.Length < 9 + i * 4)
            {
                if (smoney[smoney.Length - (3 + i * 4)] != '0')
                {
                    smoney = smoney.Substring(0, smoney.Length - (5 + i * 4 - 3));
                    smoney = smoney + arrAlphabetNeed[i];
                    if (i < 4)
                    {
                        smoney = smoney.Remove(smoney.Length - 3, 1);
                        smoney = smoney.Insert(smoney.Length - 2, ".");
                    }
                    else
                    {
                        smoney = smoney.Remove(smoney.Length - 4, 1);
                        smoney = smoney.Insert(smoney.Length - 3, ".");
                    }
                }
                else
                {
                    smoney = smoney.Substring(0, smoney.Length - (5 + i * 4 - 1));
                    smoney = smoney + arrAlphabetNeed[i];
                }
                return smoney;
            }
        }
        return smoney;
    }

    #region === UI HOME ===
    public void Btn_Play()
    {
        panelHome.SetActive(false);
        panelMain.SetActive(true);
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
    #endregion

    #region === UI IN-WALL ===
    public void ShowInWall()
    {
        panelInWall.SetActive(true);
    }

    public void ShowPanelBuild()
    {
        panelBuild.SetActive(true);
        //for (int i = 0; i < lstHouse.Count; i++)
        //{
            
        //}
    }
    #endregion
}
