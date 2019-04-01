using EventDispatcher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Castle : MonoBehaviour
{
    [Header("INFO CASTLE")]
    public float speed;

    [Header("CHECK MOVE")]
    public Vector3 posMove;
    public bool isMove;

    [Header("RELEASE")]
    public List<House> lsHouseRelease = new List<House>();

    void Start()
    {
        this.RegisterListener(EventID.StartGame, (param) => OnStartGame());
    }

    void OnStartGame()
    {
        this.RegisterListener(EventID.BuildHouseComplete, (param) => OnBuildHouseComplete(param));
    }

    void OnBuildHouseComplete(object _param)
    {
        Dictionary<string, int> keyHouse = (Dictionary<string, int>)_param;
        if (lsHouseRelease.Count < 3)
        {
            lsHouseRelease.Add(GameManager.Instance.lsHousePlayer[keyHouse["IdHouse"]]);
            ShowAvatarHero(keyHouse["IdHero"] - 1);
        }
    }

    void ShowAvatarHero(int _id)
    {
        for (int i = 0; i < UIManager.Instance.lstAvatarHeroRelease.Length; i++)
        {
            if (!UIManager.Instance.lstAvatarHeroRelease[i].gameObject.activeSelf)
            {
                UIManager.Instance.lstAvatarHeroRelease[i].gameObject.SetActive(true);
                UIManager.Instance.lstAvatarHeroRelease[i].sprite = UIManager.Instance.lsSprAvatarHero[_id];
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
                UIManager.Instance.lstAvatarHeroRelease[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = lsHouseRelease[i].info.countHero.ToString();
            }
        }
        if (GameManager.Instance.stateGame == StateGame.Playing)
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
        if (GameManager.Instance.stateGame == StateGame.Playing)
        {
            if (!GameManager.Instance.isAttacking)
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
        {
            UIManager.Instance.ShowInWall();
        }
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

    public void RelaceHero_Onclick(int idLocation)
    {
        UIManager.Instance.panelRelace.SetActive(true);
        for (int k = 0; k < UIManager.Instance.contentRelace.childCount; k++)
        {
            Destroy(UIManager.Instance.contentRelace.GetChild(k).gameObject);
        }
        for (int i = 0; i < GameManager.Instance.lsHousePlayer.Count; i++)
        {
            if (GameManager.Instance.lsHousePlayer[i].info.typeState == TypeStateHouse.None)
            {
                bool isCheckHero = false;
                for (int j = 0; j < lsHouseRelease.Count; j++)
                {
                    if (GameManager.Instance.lsHousePlayer[i].info.idHero == lsHouseRelease[j].info.idHero)
                        isCheckHero = true;
                }
                if (!isCheckHero)
                {
                    GameObject obj = Instantiate(GameManager.Instance.itemHero, UIManager.Instance.contentRelace);
                    ItemHeroRelace item = obj.GetComponent<ItemHeroRelace>();
                    item.idLocation = idLocation;
                    item.idHouseRelace = i;
                    item.iconHero.sprite = UIManager.Instance.lsSprAvatarHero[GameManager.Instance.lsHousePlayer[item.idHouseRelace].info.idHero - 1];
                }
            }
        }
    }

}
