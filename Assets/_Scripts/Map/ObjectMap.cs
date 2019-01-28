using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMap : MonoBehaviour
{
    public static ObjectMap Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;

    }

    public List<ItemObjectMap> lstObjMap = new List<ItemObjectMap>();
}
