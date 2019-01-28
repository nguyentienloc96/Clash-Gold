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
        healthMax = GameConfig.Instance.Bloodlv0;
        for (int i = 0; i < 3; i++)
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
            StartCoroutine(IEInstantiate(
                GameManager.Instance.lsPrefabsHero[typeHero],
                lsPos[i],
                numberHero,
                "Hero",
                1));
        }
    }

    public IEnumerator IEInstantiate(Hero prafabs, Transform posIns, int countHero, string name, int level)
    {
        Hero hero = Instantiate<Hero>(prafabs, posIns.position,Quaternion.identity);
        hero.gameObject.name = name;
        lstHeroRelease.Add(hero);
        yield return new WaitForEndOfFrame();
        hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
        hero.infoHero.numberHero = countHero;
        hero.txtCountHero.text = UIManager.Instance.ConvertNumber(hero.infoHero.numberHero);
        hero.infoHero.healthAll = hero.infoHero.health * hero.infoHero.numberHero;
    }

    void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    posMove = GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition);
            //    posMove.z = 0f;
            //    isMove = true;
            //    isChildMove = true;
            //}
            //if (isMove)
            //{
            //    MoveToPosition(posMove);
            //    if (transform.position == posMove)
            //    {
            //        isMove = false;
            //    }
            //}
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

        if (lstHeroRelease.Count > 0)
        {
            if (isChildMove)
            {
                for (int i = 0; i < lstHeroRelease.Count; i++)
                {
                    lstHeroRelease[i].StartMoveToPosition(lsPos[i].position + diffCurrent);
                }
                isChildMove = false;
            }
            float speedMin = lstHeroRelease[0].infoHero.speed;
            for (int i = 1; i < lstHeroRelease.Count; i++)
            {
                if(lstHeroRelease[i].infoHero.speed < speedMin)
                {
                    speedMin = lstHeroRelease[i].infoHero.speed;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, _toPos, speedMin / 10f * Time.deltaTime);
        }
    }
}
