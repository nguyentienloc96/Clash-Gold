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
    public Vector3 posMove;
    public bool isMove;

    [Header("UI")]
    public Text txtLevel;
    public Text txtHealth;

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
    }

    void OnBuildHouseComplete(object _param)
    {
        Dictionary<string, int> keyHouse = (Dictionary<string, int>)_param;
        if (lsHouseRelease.Count < 3)
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
        if (lsHouseRelease.Count > 0)
        {
            for (int i = 0; i < lsHouseRelease.Count; i++)
            {
                lstAvatarHeroRelease[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = lsHouseRelease[i].countHero.ToString();
            }
        }
        if (GameManager.Instance.isPlay)
        {
            if (isMove)
            {
                MoveToPosition(posMove);
                Vector3 posMoveZ = posMove;
                posMoveZ.z = -2f;
                if (transform.position == posMoveZ)
                {
                    isMove = false;
                }
            }
        }
    }

    public void MoveCastle(Vector3 posMouse)
    {
        if (GameManager.Instance.isPlay)
        {
            if (!GameManager.Instance.isAttack)
            {
                Vector3 posEnd = DeadzoneCamera.Instance._camera.ScreenToWorldPoint(posMouse);
                posEnd.z = 0f;
                Vector3 posCastle = transform.position;
                posCastle.z = 0f;
                if (!Physics2D.Raycast(posCastle, posEnd - posCastle, Vector3.Distance(posCastle, posEnd), 1 << 13))
                {
                    posMove = posEnd;
                    isMove = true;
                }
            }
        }
    }

    void OnMouseUp()
    {
        if (!DeadzoneCamera.Instance.IsPointerOverGameObject())
            UIManager.Instance.ShowInWall();
    }

    public bool CheckCastle()
    {
        bool CastRays = false;
        Ray ray = DeadzoneCamera.Instance._camera.ScreenPointToRay(Input.mousePosition);
        if (Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << 10))
        {
            CastRays = true;
        }
        else if (Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << 13))
        {
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
            transform.position = Vector3.MoveTowards(transform.position, _toPos, speed / 5f * Time.deltaTime);
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
                    GameObject obj = Instantiate(itemHero, UIManager.Instance.contentRelace);
                    ItemHeroRelace item = obj.GetComponent<ItemHeroRelace>();
                    item.idLocation = idLocation;
                    item.idHouseRelace = i;
                    item.iconHero.sprite = UIManager.Instance.sprAvatarHero[GameManager.Instance.lstHousePlayer[item.idHouseRelace].idHero - 1];
                }
            }
        }
    }

    public void CloseRelace()
    {
        UIManager.Instance.panelRelace.SetActive(false);
    }
}
