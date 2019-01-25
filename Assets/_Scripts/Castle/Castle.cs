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
    // Use this for initialization
    void Start()
    {
        healthMax = GameConfig.Instance.Bloodlv0;
    }

    // Update is called once per frame
    void Update()
    {

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

    public void Move()
    {

    }
}
