using EventDispatcher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public class Castle : MonoBehaviour
{
    public float health;
    public float healthMax;
    public long price;
    public int level;
    public bool isCanReleaseCanon = false;
    public Collider2D colliderLand;
    public List<Hero> lstHeroRelease;

    public List<Transform> lsPos;

    [Header("CHECK MOVE")]
    public Vector3 posMove;
    public bool isMove;
    public bool isChildMove;


    void Start()
    {
        this.RegisterListener(EventID.StartGame, (param) => OnStartGame());
    }

    void OnStartGame()
    {
        healthMax = GameConfig.Instance.Bloodlv0;
        for (int i = 0; i < 3; i++)
        {
            InstantiateHero(i);
        }
    }

    public void InstantiateHero(int i)
    {
        int typeHero;
        if (i == 0)
        {
            int randomFly = Random.Range(0, GameManager.Instance.lsHeroFly.Length);
            typeHero = GameManager.Instance.lsHeroFly[randomFly];
        }
        else
        {
            int randomCanMove = Random.Range(0, GameManager.Instance.lsHeroCanMove.Length);
            typeHero = GameManager.Instance.lsHeroCanMove[randomCanMove];
        }
        int numberHero = 1;

        Hero hero = Instantiate(GameManager.Instance.lsPrefabsHero[typeHero]
            , lsPos[i].position
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
            if (IsPointerOverGameObject())
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

    public void UpgradeCastle()
    {
        healthMax = healthMax * GameConfig.Instance.Bloodratio;
        price = (long)(price * GameConfig.Instance.PriceBloodUp);
    }

    public void MoveToPosition(Vector3 _toPos)
    {
        Vector3 diff = _toPos - transform.position;
        Vector3 diffCurrent = diff;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        float speedMin = lstHeroRelease[0].infoHero.speed;
        for (int i = 1; i < lstHeroRelease.Count; i++)
        {
            if (lstHeroRelease[i].infoHero.speed < speedMin)
            {
                speedMin = lstHeroRelease[i].infoHero.speed;
            }
        }
        if (lstHeroRelease.Count > 0)
        {
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
