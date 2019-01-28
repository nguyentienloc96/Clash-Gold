using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveMap
{
    public List<ItemObjectMap> lstObjMap = new List<ItemObjectMap>();
}

[System.Serializable]
public class ItemObjectMap
{
    public string name;
    public float position_x;
    public float position_y;
    public float position_z;
    public float rotation_x;
    public float rotation_y;
    public float rotation_z;
    public float scale_x;
    public float scale_y;
    public float scale_z;
}

