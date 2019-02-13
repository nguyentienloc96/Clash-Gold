using EventDispatcher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[System.Serializable]
public class Castle : MonoBehaviour
{
    public float health;
    public float healthMax;
    public long price;
    public int level = 1;
    public bool isCanReleaseCanon = false;
    public Collider2D colliderLand;
    public List<Hero> lstHeroRelease = new List<Hero>();

    public List<Transform> lsPos;

    [Header("CHECK MOVE")]
    public Vector3 posMove;
    public bool isMove;
    public bool isChildMove;
    public Camera cameraMain;

    [Header("UI")]
    public Text txtLevel;
    public Text txtHealth;
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
        if (lstHeroRelease.Count == 0)
        {
            InstantiateHero((int)_param);
        }
        else if (lstHeroRelease.Count < 3)
        {
            for (int i = 0; i < lstHeroRelease.Count; i++)
            {
                if ((int)_param != lstHeroRelease[i].infoHero.ID)
                {
                    InstantiateHero((int)_param);
                    break;
                }
            }
        }
    }

    public void InstantiateHero(int idHero)
    {
        int numberHero = 1;

        Hero hero = Instantiate(GameManager.Instance.lsPrefabsHero[idHero-1]
            , lsPos[lstHeroRelease.Count].position
            , Quaternion.identity
            , GameManager.Instance.heroManager);
        hero.gameObject.name = "Hero";
        hero.SetInfoHero();
        hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
        hero.AddHero(numberHero);
        lstHeroRelease.Add(hero);
    }

    void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            if (CheckCastle())
                return;

            if (Input.GetMouseButtonDown(0))
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
        float deltaHelth = healthMax - health;
        UIManager.Instance.SetActivePanel(UIManager.Instance.anim_UpHealth);
        healthMax = healthMax * GameConfig.Instance.Bloodratio;
        health = health + deltaHelth;
        price = (long)(price * GameConfig.Instance.PriceBloodUp);
        SetUI();
        Invoke("HideAnim", 1.5f);
    }

    void HideAnim()
    {
        UIManager.Instance.SetDeActivePanel(UIManager.Instance.anim_UpHealth);
    }

    public void SetUI()
    {
        txtLevel.text = level.ToString();
        txtHealth.text = health.ToString() + "/" + healthMax.ToString();
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
            if (isChildMove)
            {
                for (int i = 0; i < lstHeroRelease.Count; i++)
                {
                    lstHeroRelease[i].speedMin = speedMin;
                    lstHeroRelease[i].StartMoveToPosition(lsPos[i].position + diffCurrent);
                }
                isChildMove = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, _toPos, speedMin / 10f * Time.deltaTime);
        }
    }
}
