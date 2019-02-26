using EventDispatcher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[System.Serializable]
public class Castle : MonoBehaviour
{
    [Header("INFO CASTLE")]
    public float health;
    public float healthMax;
    public long price;
    private int level = 1;
    public float speed;
    [HideInInspector]
    public bool isCanReleaseCanon = false;

    [Header("CHECK MOVE")]
    [HideInInspector]
    public Vector3 posMove;
    [HideInInspector]
    public bool isMove;

    [Header("UI")]
    public Text txtLevel;
    public Text txtHealth;
    public Camera cameraMain;

    [Header("LIST")]
    public List<Transform> lsPos;
    public Image[] lstAvatarHeroRelease;

    public List<House> lsHouseRelease = new List<House>();

    public GameObject itemHero;

    void Start()
    {
        this.RegisterListener(EventID.StartGame, (param) => OnStartGame());
    }

    void OnStartGame()
    {
        healthMax = GameConfig.Instance.Bloodlv0;
        health = healthMax;
        this.RegisterListener(EventID.BuildHouseComplete, (param) => OnBuildHouseComplete(param));
        SetUI();
        if (isCanReleaseCanon)
        {
            UIManager.Instance.buttonReleaseCanon.interactable = true;
        }
    }

    void OnBuildHouseComplete(object _param)
    {
        Dictionary<string, int> keyHouse = (Dictionary<string, int>)_param;
        if (keyHouse["IdHero"] != 9 && lsHouseRelease.Count < 3)
        {
            lsHouseRelease.Add(GameManager.Instance.lstHousePlayer[keyHouse["IdHouse"]]);
            ShowAvatarHero(keyHouse["IdHero"] - 1);
        }
    }

    void ShowAvatarHero(int _id)
    {
        for (int i = 0; i < lstAvatarHeroRelease.Length; i++)
        {
            if (!lstAvatarHeroRelease[i].gameObject.activeSelf)
            {
                lstAvatarHeroRelease[i].gameObject.SetActive(true);
                lstAvatarHeroRelease[i].sprite = UIManager.Instance.sprAvatarHero[_id];
                break;
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            if (CheckCastle() || IsPointerOverGameObject() || UIManager.Instance.isBinoculars)
                return;

            if (Input.GetMouseButtonDown(0) && !GameManager.Instance.isAttack)
            {
                posMove = GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition);
                posMove.z = 0f;
                isMove = true;
            }
            if (isMove)
            {
                MoveToPosition(posMove);
                if (transform.position == posMove)
                {
                    isMove = false;
                }
            }
        }
    }

    void OnMouseUp()
    {
        if (!IsPointerOverGameObject())
            UIManager.Instance.ShowInWall();
    }

    public static bool IsPointerOverGameObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, list);
        return list.Count > 0;
    }

    public bool CheckCastle()
    {
        bool CastRays = false;
        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            if (hit.transform.gameObject.layer == 10)
                CastRays = true;
        }
        return CastRays;
    }

    public void UpgradeCastle()
    {
        price = (long)(price * GameConfig.Instance.PriceBloodUp);
        if (price > GameManager.Instance.gold)
            return;

        float deltaHelth = healthMax - health;
        UIManager.Instance.SetActivePanel(UIManager.Instance.anim_UpHealth);
        healthMax = healthMax * GameConfig.Instance.Bloodratio;
        health = healthMax - deltaHelth;
        GameManager.Instance.AddGold(-price);
        level++;
        SetUI();
        Invoke("HideAnim", 1f);
    }

    void HideAnim()
    {
        UIManager.Instance.SetDeActivePanel(UIManager.Instance.anim_UpHealth);
    }

    public void SetUI()
    {
        txtLevel.text = level.ToString();
        txtHealth.text = ((int)health).ToString() + "/" + ((int)healthMax).ToString();
    }

    public void MoveToPosition(Vector3 _toPos)
    {
        if (lsHouseRelease.Count > 0)
        {
            Vector3 diff = _toPos - transform.position;
            Vector3 diffCurrent = diff;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            _toPos.z = -2f;
            transform.position = Vector3.MoveTowards(transform.position, _toPos, speed / 10f * Time.deltaTime);
        }
    }

    public void RelaceHero(int idLocation)
    {
        UIManager.Instance.panelRelace.SetActive(true);
        for (int k = 0; k < UIManager.Instance.contentRelace.childCount; k++)
        {
            Destroy(UIManager.Instance.contentRelace.GetChild(k).gameObject);
        }
        for (int i = 0; i < GameManager.Instance.lstHousePlayer.Count; i++)
        {
            if (GameManager.Instance.lstHousePlayer[i].typeState == TypeStateHouse.None)
            {
                bool isCheckHero = false;
                for (int j = 0; j < lsHouseRelease.Count; j++)
                {
                    if (GameManager.Instance.lstHousePlayer[i].idHero == lsHouseRelease[j].idHero)
                        isCheckHero = true;
                }
                if (!isCheckHero)
                {
                    int m = i;
                    GameObject obj = Instantiate(itemHero, UIManager.Instance.contentRelace);
                    obj.transform.GetChild(0).gameObject.SetActive(true);
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = UIManager.Instance.sprAvatarHero[GameManager.Instance.lstHousePlayer[m].idHero - 1];
                    obj.GetComponent<Button>().onClick.AddListener(() => RelaceItemHero(idLocation, GameManager.Instance.lstHousePlayer[m].idHouse));
                }
            }
        }
    }

    public void CloseRelace()
    {
        UIManager.Instance.panelRelace.SetActive(false);
    }

    public void RelaceItemHero(int idLocation, int idHouseRelace)
    {
        int idHero = GameManager.Instance.lstHousePlayer[idHouseRelace].idHero;
        lsHouseRelease[idLocation] = GameManager.Instance.lstHousePlayer[idHouseRelace];
        lstAvatarHeroRelease[idLocation].gameObject.SetActive(true);
        lstAvatarHeroRelease[idLocation].sprite = UIManager.Instance.sprAvatarHero[idHero - 1];
        UIManager.Instance.panelRelace.SetActive(false);
    }
}
