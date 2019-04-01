using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = new UIManager();

    public void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    [Header("UI HOME")]
    public GameObject panelHome;
    public GameObject panelGroupHome;
    public GameObject panelChooseLevel;
    public GameObject panelGameOver;
    public GameObject panelVictory;
    public Button btnContinue;

    [Header("UI TOP")]
    public Text txtGold;
    public Text txtCoin;
    public Text txtProductGold;

    [Header("HERO")]
    public List<Sprite> lsSprAvatarHero = new List<Sprite>();
    public Image[] lstAvatarHeroRelease;

    [Header("SUPPORT")]
    public List<string> arrAlphabetNeed = new List<string>();
    private string[] arrAlphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

    [Header("IN-WALL")]
    public GameObject panelInWall;
    public GameObject panelBuildSelect;
    public GameObject panelUpgrade;
    public GameObject panelBuildHouse;
    public GameObject panelRelace;

    [Header("WARRING")]
    public GameObject warringBeingAttack;
    public GameObject panelWarring;
    public DetailWarring detailWarring;

    [Header("HOUSE")]
    public List<BuildHouseObject> lsBuildHouseUI = new List<BuildHouseObject>();
    public List<Button> lsBtnIconHouse = new List<Button>();

    [Header("ATTACK")]
    public GameObject panelReleaseAttack;
    public List<ItemHeroAttack> lsItemHeroAttack = new List<ItemHeroAttack>();
    public GameObject panelLetGo;
    public Text txtLetGo;
    public Canvas cavas;
    public Canvas canvasLoading;
    public Camera cameraAttack;
    public GameObject mapAttack;
    public Transform contentRelace;

    public GameObject panelRelease;
    public Transform contentThrowHero;
    public GameObject itemThrowHero;

    [Header("UPGRADE")]
    public Text txtLevelCurrent;
    public Text txtLevelUpgrade;
    public Text txtCapCurrent;
    public Text txtCapUpgrade;
    public Text txtPrice;
    public Image iconHero;
    public Text txtNameHero;
    public Text txtInfoHero;
    public DetailIcon detailIcon;
    public Button btnUpgradeHouse;
    public int houseClick;

    [Header("UPGRADE GOLD MINE")]
    public GameObject panelUpgradeG;
    public Text txtLevelCurrentUpG;
    public Text txtLevelUpgradeUpG;
    public Text txtCapCurrentUpG;
    public Text txtCapUpgradeUpG;
    public Text txtPriceUpG;
    public Image iconHeroUpG;
    public Text txtNameHeroUpG;
    public Button btnUpG;

    [Header("STORY")]
    public Text txtStory;

    [Header("SETTING")]
    public GameObject panelSeting;

    void Start()
    {
        for (int j = 0; j < arrAlphabet.Length; j++)
        {
            arrAlphabetNeed.Add(arrAlphabet[0] + arrAlphabet[j]);
        }
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

    public void SetActivePanel(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
    }
    #endregion

    public void BtnPlay_Onclick()
    {
        SetActivePanel(panelGroupHome, false);
        SetActivePanel(panelChooseLevel, true);
    }

    public void BtnMode_Onclick(float mode)
    {
        GameManager.Instance.GenerateMapBox();
        SetActivePanel(panelChooseLevel, false);
        SetActivePanel(panelHome, false);
        GameManager.Instance.ratioBorn = mode;
        txtStory.text = GameConfig.Instance.lsStory[Random.Range(0, GameConfig.Instance.lsStory.Count)];
        ScenesManager.Instance.GoToScene(
            1,
            null,
            () =>
            {
                GameManager.Instance.AddGold(GameConfig.Instance.GoldStart);
                GameManager.Instance.AddCoin(GameConfig.Instance.CoinStart);
            }
        );
    }

    public void BtnContinue_Onclick()
    {
        GameManager.Instance.ratioBorn = DataPlayer.Instance.ratioBorn;
        GameManager.Instance.GenerateMapBoxJson();
        SetActivePanel(panelChooseLevel, false);
        SetActivePanel(panelHome, false);
        txtStory.text = GameConfig.Instance.lsStory[Random.Range(0, GameConfig.Instance.lsStory.Count)];
        ScenesManager.Instance.GoToScene(
            1,
            null,
            () =>
            {
                GameManager.Instance.AddGold(DataPlayer.Instance.gold);
                GameManager.Instance.AddCoin(DataPlayer.Instance.coin);
            }
        );
    }

    public void BtnBackHome_Oclick()
    {
        ScenesManager.Instance.GoToScene(1);
        SetActivePanel(panelChooseLevel, false);
        SetActivePanel(panelGroupHome, true);
    }

    #region === UI IN-WALL ===
    public void ShowInWall()
    {
        SetActivePanel(panelInWall,true);
    }

    public void ShowPanelBuild()
    {
        SetActivePanel(panelBuildSelect,true);
        for (int i = 0; i < lsBuildHouseUI.Count; i++)
        {
            lsBuildHouseUI[i].SetLock(GameManager.Instance.lsBuildHouse[i].isUnlock);
        }
    }

    public void ShowPanelUpgrade()
    {
        SetActivePanel(panelUpgrade,true);
        UpgradeHouse();
    }

    public void UpgradeHouse()
    {
        int _idHeroUpgrade = GameManager.Instance.lsHousePlayer[houseClick].info.idHero;
        detailIcon.idHero = _idHeroUpgrade;
        detailIcon.icon.SetActive(true);
        detailIcon.info.SetActive(false);
        iconHero.sprite = lsSprAvatarHero[_idHeroUpgrade - 1];
        txtNameHero.text = GameConfig.Instance.lsInfoHero[_idHeroUpgrade - 1].nameHero;
        txtInfoHero.text = GameConfig.Instance.lsInfoHero[_idHeroUpgrade - 1].info;
        GameManager.Instance.lsHousePlayer[houseClick].CheckUpgrade(1);
        txtLevelCurrent.text = GameManager.Instance.lsHousePlayer[houseClick].info.level.ToString();
        txtLevelUpgrade.text = GameManager.Instance.lsHousePlayer[houseClick].levelWillupgrade.ToString();
        txtCapCurrent.text = GameManager.Instance.lsHousePlayer[houseClick].info.capWar.ToString();
        txtCapUpgrade.text = GameManager.Instance.lsHousePlayer[houseClick].capWillUpgrade.ToString();
        txtPrice.text = ConvertNumber(GameManager.Instance.lsHousePlayer[houseClick].priceWillUpgrade);
        if (GameManager.Instance.gold < GameManager.Instance.lsHousePlayer[houseClick].priceWillUpgrade)
        {
            btnUpgradeHouse.interactable = false;
        }
        else
        {
            btnUpgradeHouse.interactable = true;
        }
    }

    public void Btn_YesUpgrade()
    {
        GameManager.Instance.lsHousePlayer[houseClick].YesUpgrade(1);
        UpgradeHouse();
        //SetActivePanel(panelUpgrade,false);
    }

    public void Btn_NoUpgrade()
    {
        SetActivePanel(panelUpgrade,false);
    }

    public void Btn_BuildHouse(int _id)
    {
        Debug.Log(houseClick);
        GameManager.Instance.lsHousePlayer[houseClick].Build(_id);
        SetActivePanel(panelBuildSelect,false);
    }

    public void Btn_CloseWall()
    {
        SetActivePanel(panelInWall, false);
    }
    #endregion

    public void HideAllPanelGame()
    {

    }

    public void GetUIAttack()
    {
        cavas.worldCamera = cameraAttack;
        canvasLoading.worldCamera = cameraAttack;
        mapAttack.SetActive(true);
    }

    public void GetUIMove()
    {
        cavas.worldCamera = cameraAttack;
        canvasLoading.worldCamera = cameraAttack;
        mapAttack.SetActive(true);
    }

    public void BtnCloseReleaseHero_Onclick()
    {
        panelRelace.SetActive(false);
    }

    public void UpgradeGoldMine(string nameGM,Sprite icon,int lvCurrent,int lvWillUp,int capCurrent,int capWillUp,long price,bool isOutMoney,UnityEngine.Events.UnityAction actionUp)
    {
        panelUpgradeG.SetActive(true);
        txtNameHeroUpG.text = nameGM;
        iconHeroUpG.sprite = icon;
        txtLevelCurrentUpG.text = lvCurrent.ToString();
        txtLevelUpgradeUpG.text = lvWillUp.ToString();
        txtCapCurrentUpG.text = capCurrent.ToString();
        txtCapUpgradeUpG.text = capWillUp.ToString();
        txtPriceUpG.text = ConvertNumber(price);
        if (isOutMoney)
        {
            btnUpG.interactable = false;
        }
        else
        {
            btnUpG.interactable = true;
        }
        btnUpG.onClick.RemoveAllListeners();
        btnUpG.onClick.AddListener(actionUp);
    }

    public void Btn_OpenSetting()
    {
        Time.timeScale = 0;
        panelSeting.SetActive(true);
    }

    public void Btn_CloseSetting()
    {
        Time.timeScale = 1;
        panelSeting.SetActive(false);
    }

    public void Btn_SaveExit()
    {
        DataPlayer.Instance.SaveDataPlayer();
        panelHome.SetActive(true);
        SetActivePanel(panelGroupHome, true);
        SetActivePanel(panelChooseLevel,false);
        txtStory.text = GameConfig.Instance.lsStory[Random.Range(0, GameConfig.Instance.lsStory.Count)];
        ScenesManager.Instance.GoToScene(1,() =>
        {
            GameManager.Instance.stateGame = StateGame.Home;
        });
    }

    public void Btn_ThrowHero()
    {
        GameManager.Instance.goldMineCurrent.ThrowHero();
        GameManager.Instance.goldMineCurrent = null;
        panelRelease.SetActive(false);
    }
}
