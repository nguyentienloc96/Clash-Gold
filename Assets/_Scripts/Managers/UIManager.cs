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

    [Header("RELEASE")]
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

    [Header("EQUIP")]
    public GameObject panelEquip;
    public Transform contentEquip;
    public List<GameObject> lsItemEquip = new List<GameObject>();
    public Sprite[] arrImgTypeEquip = new Sprite[5];
    public Sprite[] arrImgIconType = new Sprite[4];
    public Sprite[] arrImgBtnEquip = new Sprite[3];
    public Sprite spBoderBig;
    public Text txtExp;

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

    #region === HOME ===
    public void BtnPlay_Onclick()
    {
        SetActivePanel(panelGroupHome, false);
        SetActivePanel(panelChooseLevel, true);
    }

    public void BtnMode_Onclick(float mode)
    {
        if (GameManager.Instance.idMapBox != -1)
        {
            StartCoroutine(IEResetMap());
        }
        else
        {
            GameManager.Instance.GenerateMapBox();
        }
        SetActivePanel(panelChooseLevel, false);
        SetActivePanel(panelHome, false);
        GameManager.Instance.ratioBorn = mode;
        txtStory.text = GameConfig.Instance.lsStory[Random.Range(0, GameConfig.Instance.lsStory.Count)];
        ScenesManager.Instance.GoToScene(
            1,
            null,
            () =>
            {
                GameManager.Instance.GetGold(GameConfig.Instance.GoldStart);
                GameManager.Instance.GetCoin(GameConfig.Instance.CoinStart);
            }
        );
    }

    public IEnumerator IEResetMap()
    {
        GameManager.Instance.ClearMap();
        yield return new WaitUntil(() => GameManager.Instance.lsBoxManager.Count == 0
         && GameManager.Instance.enemyManager.childCount == 0);
        GameManager.Instance.GenerateMapBox();
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
                GameManager.Instance.GetGold(DataPlayer.Instance.gold);
                GameManager.Instance.GetCoin(DataPlayer.Instance.coin);
            }
        );
    }

    public void BtnBackHome_Oclick()
    {
        ScenesManager.Instance.GoToScene(1);
        SetActivePanel(panelChooseLevel, false);
        SetActivePanel(panelGroupHome, true);
    }
    #endregion

    #region === UI IN-WALL ===
    public void ShowInWall()
    {
        SetActivePanel(panelInWall, true);
    }

    public void ShowPanelBuild()
    {
        SetActivePanel(panelBuildSelect, true);
        for (int i = 0; i < lsBuildHouseUI.Count; i++)
        {
            lsBuildHouseUI[i].SetLock(GameManager.Instance.lsBuildHouse[i].isUnlock);
        }
    }

    public void ShowPanelUpgrade()
    {
        SetActivePanel(panelUpgrade, true);
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
        SetActivePanel(panelUpgrade, false);
    }

    public void Btn_NoUpgrade()
    {
        SetActivePanel(panelUpgrade, false);
    }

    public void Btn_BuildHouse(int _id)
    {
        Debug.Log(houseClick);
        GameManager.Instance.lsHousePlayer[houseClick].Build(_id);
        SetActivePanel(panelBuildSelect, false);
    }

    public void Btn_CloseWall()
    {
        SetActivePanel(panelInWall, false);
    }
    #endregion

    #region === OTHER ===

    public void HideAllPanelGame()
    {
        panelBuildHouse.SetActive(false);
        panelBuildSelect.SetActive(false);
        panelRelace.SetActive(false);
        panelRelease.SetActive(false);
        panelReleaseAttack.SetActive(false);
        panelSeting.SetActive(false);
        panelUpgrade.SetActive(false);
        panelUpgradeG.SetActive(false);
        panelWarring.SetActive(false);
        panelInWall.SetActive(false);
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

    public void UpgradeGoldMine(string nameGM, Sprite icon, int lvCurrent, int lvWillUp, int capCurrent, int capWillUp, long price, bool isOutMoney, UnityEngine.Events.UnityAction actionUp)
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
        Time.timeScale = 1;
        HideAllPanelGame();
        panelSeting.SetActive(false);
        DataPlayer.Instance.SaveDataPlayer();
        panelHome.SetActive(true);
        SetActivePanel(panelGroupHome, true);
        SetActivePanel(panelChooseLevel, false);
        txtStory.text = GameConfig.Instance.lsStory[Random.Range(0, GameConfig.Instance.lsStory.Count)];
        ScenesManager.Instance.GoToScene(1, () =>
         {
             btnContinue.interactable = true;
             GameManager.Instance.stateGame = StateGame.Home;
         });
    }

    public void Btn_ThrowHero()
    {
        GameManager.Instance.goldMineCurrent.ThrowHero();
        GameManager.Instance.goldMineCurrent = null;
        panelRelease.SetActive(false);
    }
    #endregion

    #region === EQUIP ===
    public int indexEquip = 0;
    public void BtnEquip_Onclick()
    {
        panelEquip.SetActive(true);
        for (int i = 0; i < GameManager.Instance.lsBuildHouse.Count; i++)
        {
            if (GameManager.Instance.lsBuildHouse[i].isUnlock)
            {
                int IDHouse = GameManager.Instance.lsBuildHouse[i].ID;

                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = lsSprAvatarHero[IDHouse];
                item.txtInfo.text = "Increase health " + GameConfig.Instance.lsInfoHero[IDHouse].nameHero + " 50%";
                item.txtPercent.text = "+50%";
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, GameConfig.Instance.lsRdEquip[IDHouse].eHealth));
                item.txtPrice.text = ConvertNumber(Exp);
                item.iconEquip.sprite = arrImgTypeEquip[0];
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type1(IDHouse, 0, Exp, item.btnEquip, item));

                ItemEquipmentSelect item1 = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item1.iconHero.sprite = lsSprAvatarHero[IDHouse];
                item1.txtInfo.text = "Increase attack " + GameConfig.Instance.lsInfoHero[IDHouse].nameHero + " 50%";
                item1.txtPercent.text = "+50%";
                long Exp1 = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, GameConfig.Instance.lsRdEquip[IDHouse].eAtk));
                item1.txtPrice.text = ConvertNumber(Exp1);
                item1.iconEquip.sprite = arrImgTypeEquip[1];
                if (GameManager.Instance.exp > Exp1)
                {
                    item1.btnEquip.interactable = true;
                }
                else
                {
                    item1.btnEquip.interactable = false;
                }
                item1.btnEquip.onClick.AddListener(() => Equip_Type1(IDHouse, 1, Exp1, item1.btnEquip, item1));

                ItemEquipmentSelect item2 = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item2.iconHero.sprite = lsSprAvatarHero[IDHouse];
                item2.txtInfo.text = "Decrease hit speed " + GameConfig.Instance.lsInfoHero[IDHouse].nameHero + " 30%";
                item2.txtPercent.text = "-30%";
                long Exp2 = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, GameConfig.Instance.lsRdEquip[IDHouse].eHit_Speed));
                item2.txtPrice.text = ConvertNumber(Exp2);
                item2.iconEquip.sprite = arrImgTypeEquip[2];
                if (GameManager.Instance.exp > Exp2)
                {
                    item2.btnEquip.interactable = true;
                }
                else
                {
                    item2.btnEquip.interactable = false;
                }
                item2.btnEquip.onClick.AddListener(() => Equip_Type1(IDHouse, 2, Exp2, item2.btnEquip, item2));

                ItemEquipmentSelect item3 = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item3.iconHero.sprite = lsSprAvatarHero[IDHouse];
                item3.txtInfo.text = "Decrease cost " + GameConfig.Instance.lsInfoHero[IDHouse].nameHero + " 30%";
                item3.txtPercent.text = "-30%";
                long Exp3 = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, GameConfig.Instance.lsRdEquip[IDHouse].eCost));
                item3.txtPrice.text = ConvertNumber(Exp3);
                item3.iconEquip.sprite = arrImgTypeEquip[3];
                if (GameManager.Instance.exp > Exp3)
                {
                    item3.btnEquip.interactable = true;
                }
                else
                {
                    item3.btnEquip.interactable = false;
                }
                item3.btnEquip.onClick.AddListener(() => Equip_Type1(IDHouse, 3, Exp3, item3.btnEquip, item3));

                ItemEquipmentSelect item4 = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item4.iconHero.sprite = lsSprAvatarHero[IDHouse];
                item4.txtInfo.text = "Decrease construction " + GameConfig.Instance.lsInfoHero[IDHouse].nameHero + " 30%";
                item4.txtPercent.text = "-30%";
                long Exp4 = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, GameConfig.Instance.lsRdEquip[IDHouse].eConstruction));
                item4.txtPrice.text = ConvertNumber(Exp4);
                item4.iconEquip.sprite = arrImgTypeEquip[4];
                if (GameManager.Instance.exp > Exp4)
                {
                    item4.btnEquip.interactable = true;
                }
                else
                {
                    item4.btnEquip.interactable = false;
                }
                item4.btnEquip.onClick.AddListener(() => Equip_Type1(IDHouse, 4, Exp4, item4.btnEquip, item4));
            }
        }

        for (int i = 0; i <= GameManager.Instance.atkWalk; i++)
        {
            ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
            item.iconHero.sprite = arrImgIconType[0];
            item.txtInfo.text = "Increase Atk for all Walking " + (i + 1) * GameConfig.Instance.AtkWalk + " points";
            item.txtPercent.text = "+" + (i + 1) * GameConfig.Instance.AtkWalk;
            long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, i));
            item.txtPrice.text = ConvertNumber(Exp);
            item.iconEquip.sprite = arrImgTypeEquip[1];
            if (GameManager.Instance.exp > Exp)
            {
                item.btnEquip.interactable = true;
            }
            else
            {
                item.btnEquip.interactable = false;
            }
            item.btnEquip.onClick.AddListener(() => Equip_Type2(i + 1, 0, Exp, item.btnEquip, item));
            if (i == 0)
            {
                item.boderEquipTop.sprite = spBoderBig;
            }
        }
        for (int i = 0; i <= GameManager.Instance.atkFly; i++)
        {
            ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
            item.iconHero.sprite = arrImgIconType[1];
            item.txtInfo.text = "Increase Atk for all Flying " + (i + 1) * GameConfig.Instance.AtkFly + " points";
            item.txtPercent.text = "+" + (i + 1) * GameConfig.Instance.AtkFly;
            long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, i));
            item.txtPrice.text = ConvertNumber(Exp);
            if (GameManager.Instance.exp > Exp)
            {
                item.btnEquip.interactable = true;
            }
            else
            {
                item.btnEquip.interactable = false;
            }
            item.btnEquip.onClick.AddListener(() => Equip_Type2(i + 1, 1, Exp, item.btnEquip, item));
            item.iconEquip.sprite = arrImgTypeEquip[1];
        }
        for (int i = 0; i <= GameManager.Instance.atkMele; i++)
        {
            ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
            item.iconHero.sprite = arrImgIconType[2];
            item.txtInfo.text = "Increase Atk for all Mele " + (i + 1) * GameConfig.Instance.AtkMele + " points";
            item.txtPercent.text = "+" + (i + 1) * GameConfig.Instance.AtkMele;
            long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, i));
            item.txtPrice.text = ConvertNumber(Exp);
            item.iconEquip.sprite = arrImgTypeEquip[1];
            if (GameManager.Instance.exp > Exp)
            {
                item.btnEquip.interactable = true;
            }
            else
            {
                item.btnEquip.interactable = false;
            }
            item.btnEquip.onClick.AddListener(() => Equip_Type2(i + 1, 2, Exp, item.btnEquip, item));
        }
        for (int i = 0; i <= GameManager.Instance.atkArcher; i++)
        {
            ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
            item.iconHero.sprite = arrImgIconType[3];
            item.txtInfo.text = "Increase Atk for all Archery " + (i + 1) * GameConfig.Instance.AtkArcher + " points";
            item.txtPercent.text = "+" + (i + 1) * GameConfig.Instance.AtkArcher;
            long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, i));
            item.txtPrice.text = ConvertNumber(Exp);
            item.iconEquip.sprite = arrImgTypeEquip[1];
            if (GameManager.Instance.exp > Exp)
            {
                item.btnEquip.interactable = true;
            }
            else
            {
                item.btnEquip.interactable = false;
            }
            item.btnEquip.onClick.AddListener(() => Equip_Type2(i + 1, 3, Exp, item.btnEquip, item));
        }

        for (int i = 0; i <= GameManager.Instance.hlthWalk; i++)
        {
            ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
            item.iconHero.sprite = arrImgIconType[0];
            item.txtInfo.text = "Increase Health for all Walking " + (i + 1) * GameConfig.Instance.HlthWalk + " points";
            item.txtPercent.text = "+" + (i + 1) * GameConfig.Instance.HlthWalk;
            long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, i));
            item.txtPrice.text = ConvertNumber(Exp);
            item.iconEquip.sprite = arrImgTypeEquip[0];
            if (GameManager.Instance.exp > Exp)
            {
                item.btnEquip.interactable = true;
            }
            else
            {
                item.btnEquip.interactable = false;
            }
            item.btnEquip.onClick.AddListener(() => Equip_Type2(i + 1, 4, Exp, item.btnEquip, item));
        }
        for (int i = 0; i <= GameManager.Instance.hlthFly; i++)
        {
            ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
            item.iconHero.sprite = arrImgIconType[1];
            item.txtInfo.text = "Increase Health for all Flying " + (i + 1) * GameConfig.Instance.HlthFly + " points";
            item.txtPercent.text = "+" + (i + 1) * GameConfig.Instance.HlthFly;
            long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, i));
            item.txtPrice.text = ConvertNumber(Exp);
            item.iconEquip.sprite = arrImgTypeEquip[0];
            if (GameManager.Instance.exp > Exp)
            {
                item.btnEquip.interactable = true;
            }
            else
            {
                item.btnEquip.interactable = false;
            }
            item.btnEquip.onClick.AddListener(() => Equip_Type2(i + 1, 5, Exp, item.btnEquip, item));
        }
        for (int i = 0; i <= GameManager.Instance.hlthMele; i++)
        {
            ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
            item.iconHero.sprite = arrImgIconType[2];
            item.txtInfo.text = "Increase Health for all Mele " + (i + 1) * GameConfig.Instance.HlthMele + " points";
            item.txtPercent.text = "+" + (i + 1) * GameConfig.Instance.HlthMele;
            long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, i));
            item.txtPrice.text = ConvertNumber(Exp);
            item.iconEquip.sprite = arrImgTypeEquip[0];
            if (GameManager.Instance.exp > Exp)
            {
                item.btnEquip.interactable = true;
            }
            else
            {
                item.btnEquip.interactable = false;
            }
            item.btnEquip.onClick.AddListener(() => Equip_Type2(i + 1, 6, Exp, item.btnEquip, item));
        }
        for (int i = 0; i <= GameManager.Instance.hlthArcher; i++)
        {
            ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
            item.iconHero.sprite = arrImgIconType[3];
            item.txtInfo.text = "Increase Health for all Archery " + (i + 1) * GameConfig.Instance.HlthArcher + " points";
            item.txtPercent.text = "+" + (i + 1) * GameConfig.Instance.HlthArcher;
            long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, i));
            item.txtPrice.text = ConvertNumber(Exp);
            item.iconEquip.sprite = arrImgTypeEquip[0];
            if (GameManager.Instance.exp > Exp)
            {
                item.btnEquip.interactable = true;
            }
            else
            {
                item.btnEquip.interactable = false;
            }
            item.btnEquip.onClick.AddListener(() => Equip_Type2(i + 1, 7, Exp, item.btnEquip, item));
        }
    }

    public void Equip_Type1(int IDHouse, int type, long Exp, Button btnEquip, ItemEquipmentSelect itemEquip)
    {
        if (GameManager.Instance.exp >= Exp)
        {
            GameManager.Instance.AddExp(-Exp);
            if (type == 0)
            {
                GameConfig.Instance.lsEquip[IDHouse].isHealth = true;
            }
            else if (type == 1)
            {
                GameConfig.Instance.lsEquip[IDHouse].isAtk = true;
            }
            else if (type == 2)
            {
                GameConfig.Instance.lsEquip[IDHouse].isHitSpeed = true;
            }
            else if (type == 3)
            {
                GameConfig.Instance.lsEquip[IDHouse].isCost = true;
            }
            else
            {
                GameConfig.Instance.lsEquip[IDHouse].isUpgrade = true;
            }
            if (GameManager.Instance.xSelectedEquip < GameManager.Instance.xOpenEquip)
            {
                GetItem(GameManager.Instance.xSelectedEquip, itemEquip);
                GameManager.Instance.xSelectedEquip++;
            }
            btnEquip.image.sprite = arrImgBtnEquip[1];
        }
    }

    public void Equip_Type2(int buff, int type, long ExpEquip, Button btnEquip, ItemEquipmentSelect itemEquip)
    {
        if (GameManager.Instance.exp >= ExpEquip)
        {
            string stPercent = "";
            GameManager.Instance.AddExp(-ExpEquip);
            if (type == 0)
            {
                GameManager.Instance.atkWalk = buff;
                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = arrImgIconType[0];
                stPercent = ((buff + 1) * GameConfig.Instance.AtkWalk).ToString();
                item.txtInfo.text = "Increase Atk for all Walking " + stPercent + " points";
                item.txtPercent.text = "+" + stPercent;
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, buff));
                item.txtPrice.text = ConvertNumber(Exp);
                item.iconEquip.sprite = arrImgTypeEquip[1];
                GameManager.Instance.xSelectedEquip++;
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type2(buff + 1, 0, Exp, item.btnEquip, item));
            }
            else if (type == 1)
            {
                GameManager.Instance.atkFly = buff;
                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = arrImgIconType[1];
                stPercent = ((buff + 1) * GameConfig.Instance.AtkFly).ToString();
                item.txtInfo.text = "Increase Atk for all Flying " + stPercent + " points";
                item.txtPercent.text = "+" + stPercent;
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, buff));
                item.txtPrice.text = ConvertNumber(Exp);
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type2(buff + 1, 1, Exp, item.btnEquip, item));
                item.iconEquip.sprite = arrImgTypeEquip[1];
            }
            else if (type == 2)
            {
                GameManager.Instance.atkMele = buff;
                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = arrImgIconType[2];
                stPercent = ((buff + 1) * GameConfig.Instance.AtkMele).ToString();
                item.txtInfo.text = "Increase Atk for all Mele " + stPercent + " points";
                item.txtPercent.text = "+" + stPercent;
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, buff));
                item.txtPrice.text = ConvertNumber(Exp);
                item.iconEquip.sprite = arrImgTypeEquip[1];
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type2(buff + 1, 2, Exp, item.btnEquip, item));
            }
            else if (type == 3)
            {
                GameManager.Instance.atkArcher = buff;
                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = arrImgIconType[3];
                stPercent = ((buff + 1) * GameConfig.Instance.AtkArcher).ToString();
                item.txtInfo.text = "Increase Atk for all Archery " + stPercent + " points";
                item.txtPercent.text = "+" + stPercent;
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, buff));
                item.txtPrice.text = ConvertNumber(Exp);
                item.iconEquip.sprite = arrImgTypeEquip[1];
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type2(buff + 1, 3, Exp, item.btnEquip, item));
            }
            else if (type == 4)
            {
                GameManager.Instance.hlthWalk = buff;
                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = arrImgIconType[0];
                stPercent = ((buff + 1) * GameConfig.Instance.HlthWalk).ToString();
                item.txtInfo.text = "Increase Health for all Walking " + stPercent + " points";
                item.txtPercent.text = "+" + stPercent;
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, buff));
                item.txtPrice.text = ConvertNumber(Exp);
                item.iconEquip.sprite = arrImgTypeEquip[0];
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type2(buff + 1, 4, Exp, item.btnEquip, item));
            }
            else if (type == 5)
            {
                GameManager.Instance.hlthFly = buff;
                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = arrImgIconType[1];
                stPercent = ((buff + 1) * GameConfig.Instance.HlthFly).ToString();
                item.txtInfo.text = "Increase Health for all Flying " + stPercent + " points";
                item.txtPercent.text = "+" + stPercent;
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, buff));
                item.txtPrice.text = ConvertNumber(Exp);
                item.iconEquip.sprite = arrImgTypeEquip[0];
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type2(buff + 1, 5, Exp, item.btnEquip, item));
            }
            else if (type == 6)
            {
                GameManager.Instance.hlthMele = buff;
                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = arrImgIconType[2];
                stPercent = ((buff + 1) * GameConfig.Instance.HlthMele).ToString();
                item.txtInfo.text = "Increase Health for all Mele " + stPercent + " points";
                item.txtPercent.text = "+" + stPercent;
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, buff));
                item.txtPrice.text = ConvertNumber(Exp);
                item.iconEquip.sprite = arrImgTypeEquip[0];
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type2(buff + 1, 6, Exp, item.btnEquip, item));
            }
            else
            {
                GameManager.Instance.hlthArcher = buff;
                ItemEquipmentSelect item = Instantiate(GameManager.Instance.itemEquipPrefab, contentEquip);
                item.iconHero.sprite = arrImgIconType[3];
                stPercent = ((buff + 1) * GameConfig.Instance.HlthArcher).ToString();
                item.txtInfo.text = "Increase Health for all Archery " + stPercent + " points";
                item.txtPercent.text = "+" + stPercent;
                long Exp = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, buff));
                item.txtPrice.text = ConvertNumber(Exp);
                item.iconEquip.sprite = arrImgTypeEquip[0];
                if (GameManager.Instance.exp > Exp)
                {
                    item.btnEquip.interactable = true;
                }
                else
                {
                    item.btnEquip.interactable = false;
                }
                item.btnEquip.onClick.AddListener(() => Equip_Type2(buff + 1, 7, Exp, item.btnEquip, item));
            }
            if (GameManager.Instance.xSelectedEquip <= GameManager.Instance.xOpenEquip)
            {
                GetItem(GameManager.Instance.xSelectedEquip, itemEquip);
                GameManager.Instance.xSelectedEquip++;
            }
            btnEquip.image.sprite = arrImgBtnEquip[1];
        }
    }
    
    public void GetItem(int ID, ItemEquipmentSelect itemEquip)
    {
        ItemEquipment item = GameManager.Instance.lsItemEquip[ID];
        item.objOpen.SetActive(true);
        item.iconHero.sprite = itemEquip.iconHero.sprite;
        item.iconEquip.sprite = itemEquip.iconEquip.sprite;
        item.txtPercent.text = itemEquip.txtPercent.text;   
    }
    #endregion
}
