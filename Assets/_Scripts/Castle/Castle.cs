using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public class Castle : MonoBehaviour
{
    public float health;
    public float goldIncrease;
    public int level;
    public Collider2D colliderLand;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        if (!IsPointerOverGameObject())
            Debug.Log("a");
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
}
