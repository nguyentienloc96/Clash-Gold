using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = new UIManager();

    [Header("UI HOME")]
    public GameObject panelHome;
    public GameObject panelGroupHome;
    public GameObject panelChooseLevel;
    public GameObject panelMain;
    public GameObject panelYesNoNewPlay;
    public GameObject panelGameOver;
    public GameObject panelVictory;
    public Button buttonContinue;

    [Header("UI TOP")]
    public Text txtGold;
    public Text txtCoin;
    public Text txtGoldMount;

    [Header("IN-WALL")]
    public GameObject panelInWall;
    public GameObject panelBuildSelect;
    public GameObject panelUpgrade;
    public GameObject panelBuildHouse;
    public List<BuildHouseObject> lstHouse;
    public Sprite[] sprAvatarHero;

    [Header("UPGRADE")]
    public int xUpgrade = 1;
    public Text txtLevelCurrent;
    public Text txtLevelUpgrade;
    public Text txtCapCurrent;
    public Text txtCapUpgrade;
    public Text txtPrice;
    public Image iconHero;
    public Text txtNameHero;
    public Text txtInfoHero;
    public DetailIcon detailIcon;

    [Header("UI SHOWSPEECH")]
    public GameObject panelShowSpeech;
    public Text txtShowSpeech;

    [Header("OTHERS")]
    public List<Sprite> lstSpriteHouse;
    public GameObject anim_UpLV_House;
    public GameObject anim_UpLV_GoldMine;
    public Canvas parentCanvas;
    [HideInInspector]
    public List<string> arrAlphabetNeed = new List<string>();
    private string[] arrAlphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

    [Header("MAP")]
    public Canvas cavas;
    public Canvas canvasLoading;
    public GameObject panelRelace;
    public Transform contentRelace;
    public List<Sprite> lsSpriteGround = new List<Sprite>();

    [Header("ATTACK")]
    public GameObject mapAttack;
    public GameObject panelThrowHero;
    public Transform contentThrowHero;
    public GameObject itemThrowHero;
    public GameObject panelLetGo;
    public Text txtLetGo;

    public List<Button> lsBtnIconHouse = new List<Button>();

    public GameObject panelThrowHeroAttack;
    public List<ItemHeroAttack> lsItemHeroAttack = new List<ItemHeroAttack>();

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

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

        if (PlayerPrefs.GetInt(KeyPrefs.IS_CONTINUE) == 0)
        {
            buttonContinue.interactable = false;
        }
        else
        {
            buttonContinue.interactable = true;
        }
    }

    void Update()
    {
        txtGoldMount.text = "Gold mount: " + GameManager.Instance.lstGoldMinePlayer.Count.ToString() + "/" + GameConfig.Instance.GoldMinerAmount.ToString();
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
        if (_g.name == "InWall")
            _g.GetComponent<Animator>().Play("ActivePanel");
    }

    public void SetDeActivePanel(GameObject _g)
    {
        if (_g == null)
            return;

        _g.SetActive(false);
        //_g.GetComponent<Animator>().Play("DeActivePanel");
    }

    /// <summary>
    /// Hien chuot khi click man hinh
    /// </summary>
    void ShowMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 click;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out click);
            Vector3 mousePos = parentCanvas.transform.TransformPoint(click) + new Vector3(0.2f, -0.3f, 0);
            //mouseClick.transform.position = mousePos;
        }
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
            SetActivePanel(panelChooseLevel);
        }
        SetDeActivePanel(panelGroupHome);
    }

    public void Btn_YesNewPlay()
    {
        SetDeActivePanel(panelYesNoNewPlay);
        SetActivePanel(panelGroupHome);
        GameManager.Instance.isPlay = true;
        this.PostEvent(EventID.StartGame);
        this.PostEvent(EventID.UpLevelHouse);
        panelHome.SetActive(false);
        Fade.Instance.panelLoadingStory.SetActive(true);
        ScenesManager.Instance.GoToScene(() =>
          {
              GameManager.Instance.AddGold(GameConfig.Instance.GoldStart);
              GameManager.Instance.AddCoin(GameConfig.Instance.CoinStart);
          });
    }

    public void Btn_NoNewPlay()
    {
        SetActivePanel(panelGroupHome);
        SetDeActivePanel(panelYesNoNewPlay);
    }

    public void Btn_ChooseLevel(int _level)
    {
        GameManager.Instance.ratioBorn = GameConfig.Instance.RatioBorn[_level];
        GameManager.Instance.castlePlayer.price = GameConfig.Instance.PriceBlood0;
        SetDeActivePanel(panelChooseLevel);
        SetActivePanel(panelYesNoNewPlay);
    }

    public void Btn_Continue()
    {
        DataPlayer.Instance.LoadDataPlayer();
        panelHome.SetActive(false);
        GameManager.Instance.isPlay = true;
        this.PostEvent(EventID.StartGame);
        ScenesManager.Instance.GoToScene();
    }

    public void Btn_Tutorial()
    {
        Debug.Log("Tutorial");
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
        SetActivePanel(panelBuildSelect);
        for (int i = 0; i < lstHouse.Count; i++)
        {
            lstHouse[i].SetLock(GameManager.Instance.lstBuildHouse[i].isUnlock);
        }
    }

    public void ShowPanelUpgrade()
    {
        SetActivePanel(panelUpgrade);
        Btn_x1x10_Upgrade(1);
    }

    public void Btn_x1x10_Upgrade(int _x)
    {
        xUpgrade = _x;
        int _idHeroUpgrade = GameManager.Instance.lstHousePlayer[houseClick].idHero;
        detailIcon.idHero = _idHeroUpgrade;
        detailIcon.icon.SetActive(true);
        detailIcon.info.SetActive(false);
        iconHero.sprite = sprAvatarHero[_idHeroUpgrade - 1];
        txtNameHero.text = GameConfig.Instance.lstInfoHero[_idHeroUpgrade - 1].NameHero;
        txtInfoHero.text = GameConfig.Instance.lstInfoHero[_idHeroUpgrade - 1].Info;
        GameManager.Instance.lstHousePlayer[houseClick].CheckUpgrade(xUpgrade);
        txtLevelCurrent.text = GameManager.Instance.lstHousePlayer[houseClick].level.ToString();
        txtLevelUpgrade.text = GameManager.Instance.lstHousePlayer[houseClick].levelWillupgrade.ToString();
        txtCapCurrent.text = GameManager.Instance.lstHousePlayer[houseClick].capWar.ToString();
        txtCapUpgrade.text = GameManager.Instance.lstHousePlayer[houseClick].capWillUpgrade.ToString();
        txtPrice.text = ConvertNumber(GameManager.Instance.lstHousePlayer[houseClick].priceWillUpgrade);
    }

    public void Btn_YesUpgrade()
    {
        GameManager.Instance.lstHousePlayer[houseClick].YesUpgrade(xUpgrade);
        SetDeActivePanel(panelUpgrade);
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
        SetDeActivePanel(panelBuildSelect);
    }

    public void Btn_CloseWall()
    {
        panelInWall.GetComponent<Animator>().Play("DeActivePanel");
    }
    #endregion

    public void CloseThrowHero()
    {
        panelThrowHero.SetActive(false);
    }

    public void ThrowHero()
    {
        GameManager.Instance.goldMineCurrent.ThrowHero();
        GameManager.Instance.goldMineCurrent = null;
        panelThrowHero.SetActive(false);
    }

    public void HideAllPanelGame()
    {
        panelBuildSelect.SetActive(false);
        panelUpgrade.SetActive(false);
        panelInWall.SetActive(false);
        panelRelace.SetActive(false);
        panelThrowHero.SetActive(false);
        panelThrowHeroAttack.SetActive(false);
    }
}
