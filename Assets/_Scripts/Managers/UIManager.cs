using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class UIManager : MonoBehaviour
{
    [Header("UI HOME")]
    public GameObject panelHome;
    public GameObject panelGroupHome;
    public GameObject panelChooseLevel;
    public GameObject panelMain;
    public GameObject panelYesNoNewPlay;
    public Button buttonContinue;

    [Header("UI TOP")]
    public Text txtGold;
    public Text txtCoin;
    public Text txtGoldMount;

    [Header("IN-WALL")]
    public GameObject panelInWall;
    public GameObject panelBuild;
    public GameObject panelUpgrade;
    public Text txtLevelCastle;
    public Text txtBlood;
    public List<GameObject> lstHouse; //List nha de build

    [Header("UPGRADE")]
    public int xUpgrade = 1;
    public Sprite[] sprX1X10;
    public Image imgX1;
    public Image imgX10;
    public Text txtLevelCurrent;
    public Text txtLevelUpgrade;
    public Text txtCapCurrent;
    public Text txtCapUpgrade;
    public Text txtPrice;

    [Header("UI SHOWSPEECH")]
    public GameObject panelShowSpeech;
    public Text txtShowSpeech;

    public List<Sprite> lstSpriteHouse;
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
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < arrAlphabet.Length; j++)
            {
                arrAlphabetNeed.Add(arrAlphabet[i] + arrAlphabet[j]);
            }
        }

        this.RegisterListener(EventID.ShowSpeech, (param) => OnShowSpeech(param));
    }

    // Update is called once per frame
    void Update()
    {
        //txtGold.text = ConvertNumber(GameManager.Instance.gold);
        txtGold.text = GameManager.Instance.gold.ToString();
        txtCoin.text = GameManager.Instance.coin.ToString();
        txtGoldMount.text = "Gold mount: " + GameManager.Instance.lstGoldMinePlayer.Count.ToString();
    }

    #region === SUPPORT ===
    public string ConvertNumber(long number)
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

    public void SetActivePanel(GameObject _g)
    {
        if (_g == null)
            return;

        _g.SetActive(true);
        //_g.GetComponent<Animator>().Play("ActivePanel");
    }

    public void SetDeActivePanel(GameObject _g)
    {
        if (_g == null)
            return;

        _g.SetActive(false);
        //_g.GetComponent<Animator>().Play("DeActivePanel");
    }
    #endregion

    #region === EVENT ===
    void OnShowSpeech(object _param)
    {
        Debug.Log((int)_param);
        SetActivePanel(panelShowSpeech);
        txtShowSpeech.text = GameConfig.Instance.lstSpeech[(int)_param];
    }
    #endregion

    #region === UI HOME ===
    public void Btn_Play()
    {
        if (PlayerPrefs.GetInt(KeyPrefs.IS_CONTINUE) == 0)
        {
            SetActivePanel(panelChooseLevel);
        }
        else
        {
            SetActivePanel(panelYesNoNewPlay);
        }      
        SetDeActivePanel(panelGroupHome);       
    }

    public void Btn_YesNewPlay()
    {
        SetActivePanel(panelChooseLevel);
        SetDeActivePanel(panelYesNoNewPlay);
        GameManager.Instance.gold = GameConfig.Instance.GoldStart;
        GameManager.Instance.coin = GameConfig.Instance.CoinStart;
    }

    public void Btn_NoNewPlay()
    {
        SetActivePanel(panelGroupHome);
        SetDeActivePanel(panelYesNoNewPlay);
    }

    public void Btn_ChooseLevel(int _level)
    {
        GameManager.Instance.ratioBorn = GameConfig.Instance.RatioBorn[_level];
        panelHome.SetActive(false);
        GameManager.Instance.isPlay = true;
    }

    public void Btn_Continue()
    {
        if (PlayerPrefs.GetInt(KeyPrefs.IS_CONTINUE) == 0)
        {
            buttonContinue.interactable = false;
        }
        else
        {
            buttonContinue.interactable = true;
        }
        panelHome.SetActive(false);
        GameManager.Instance.isPlay = true;
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
        SetActivePanel(panelInWall);
    }

    public void ShowPanelBuild()
    {
        panelBuild.SetActive(true);
        for (int i = 0; i < lstHouse.Count; i++)
        {
            if (!GameManager.Instance.lstBuildHouse[i].isUnlock)
                lstHouse[i].transform.Find("Lock").gameObject.SetActive(true);
        }
    }

    public void ShowPanelUpgrade()
    {
        SetActivePanel(panelUpgrade);
        GameManager.Instance.lstHousePlayer[houseClick].CheckUpgrade(1);
        Btn_x1x10_Upgrade(1);
    }

    public void Btn_x1x10_Upgrade(int _x)
    {
        xUpgrade = _x;
        txtLevelCurrent.text = GameManager.Instance.lstHousePlayer[houseClick].level.ToString();
        txtLevelUpgrade.text = GameManager.Instance.lstHousePlayer[houseClick].levelWillupgrade.ToString();
        txtCapCurrent.text = GameManager.Instance.lstHousePlayer[houseClick].capWar.ToString();
        txtCapUpgrade.text = GameManager.Instance.lstHousePlayer[houseClick].capWillUpgrade.ToString();
    }

    public void Btn_YesUpgrade()
    {
        GameManager.Instance.lstHousePlayer[houseClick].YesUpgrade(xUpgrade);
    }

    public void Btn_NoUpgrade()
    {
        SetDeActivePanel(panelUpgrade);
    }

    [HideInInspector]
    public int houseClick;
    public void Btn_BuildHouse(int _id)
    {
        GameManager.Instance.lstHousePlayer[houseClick].Build(_id);
    }
    #endregion
}
