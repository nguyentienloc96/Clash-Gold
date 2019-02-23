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
    [HideInInspector]
    public bool isChildMove;

    [Header("UI")]
    public Text txtLevel;
    public Text txtHealth;
    public Camera cameraMain;

    [Header("LIST")]
    public List<Transform> lsPos;
    public List<Hero> lstHeroRelease;
    public Image[] lstAvatarHeroRelease;

    void Start()
    {
        this.RegisterListener(EventID.StartGame, (param) => OnStartGame());
        speed = 0;
        lstHeroRelease = new List<Hero>();
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
        if ((int)_param != 9)
        {
            if (lstHeroRelease.Count == 0)
            {
                InstantiateHero((int)_param, lsPos[lstHeroRelease.Count].position);
            }
            else if (lstHeroRelease.Count < 3)
            {
                for (int i = 0; i < lstHeroRelease.Count; i++)
                {
                    if ((int)_param != lstHeroRelease[i].infoHero.ID)
                    {
                        InstantiateHero((int)_param, lsPos[lstHeroRelease.Count].position);
                        break;
                    }
                }
            }
        }
    }

    public void InstantiateHero(int idHero, Vector3 posIns, int number = 1)
    {
        Hero hero = Instantiate(
        GameManager.Instance.lsPrefabsHero[idHero - 1], posIns, Quaternion.identity,
        GameManager.Instance.heroManager);

        hero.IDGold = -1;
        hero.gameObject.name = "Hero";
        hero.SetInfoHero();
        hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
        hero.AddHero(number);
        lstHeroRelease.Add(hero);
        ShowAvatarHero(idHero - 1);
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

            if (Input.GetMouseButtonDown(0) && !UIManager.Instance.isBuildCanon)
            {
                posMove = GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition);
                posMove.z = 0f;
                isMove = true;
                isChildMove = true;
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
        Vector3 diff = _toPos - transform.position;
        Vector3 diffCurrent = diff;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        if (lstHeroRelease.Count > 0)
        {
            float speedMin = lstHeroRelease[0].infoHero.speed;

            for (int i = 1; i < lstHeroRelease.Count; i++)
            {
                if (lstHeroRelease[i].infoHero.speed < speedMin)
                {
                    speedMin = lstHeroRelease[i].infoHero.speed;
                }
            }
            speed = speedMin;
            if (isChildMove)
            {
                for (int i = 0; i < lstHeroRelease.Count; i++)
                {
                    lstHeroRelease[i].speedMin = speedMin * 2f;
                    Vector3 posIns = i < 3 ? lsPos[i].position : lsPos[2].position;
                    lstHeroRelease[i].StartMoveToPosition(posIns + diffCurrent);
                }
                isChildMove = false;
            }

        }
        _toPos.z = -2f;
        transform.position = Vector3.MoveTowards(transform.position, _toPos, speed / 10f * Time.deltaTime);

    }

    public void BeingAttacked(float _dame)
    {
        health -= _dame;
        if (health <= 0)
        {
            UIManager.Instance.panelGameOver.SetActive(true);
        }
    }
}
