using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveMapManager : MonoBehaviour
{
    SaveMap sv = new SaveMap();
    private GameObject[] allObjects;
    public List<GameObject> prefabsObjectMap;
    public Transform parent;
    public InputField txtName;
    public PolygonCollider2D polyNav2D;
    public void Btn_Save()
    {
        sv.lstObjMap = new List<ItemObjectMap>();
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject item in allObjects)
        {
            if (item.name != "1")
            {
                ItemObjectMap obj = new ItemObjectMap();
                obj.name = item.name;
                obj.position_x = item.transform.position.x;
                obj.position_y = item.transform.position.y;
                obj.position_z = item.transform.position.z;
                obj.rotation_x = item.transform.rotation.x;
                obj.rotation_y = item.transform.rotation.y;
                obj.rotation_z = item.transform.rotation.z;
                obj.scale_x = item.transform.localScale.x;
                obj.scale_y = item.transform.localScale.y;
                obj.scale_z = item.transform.localScale.z;
                sv.lstObjMap.Add(obj);
            }
        }
        string jsonString = JsonUtility.ToJson(sv, true);
        Debug.Log(jsonString);
        string name = "/Resources/Map" + txtName.text + ".json";
        System.IO.File.WriteAllText(Application.dataPath + name, JsonUtility.ToJson(sv, true));
    }

    public void Btn_Load()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/Map1.json");
        SaveMap _sv = JsonUtility.FromJson<SaveMap>(jsonString);
        //var objJson = SimpleJSON_DatDz.JSON.Parse(loadJson("Map1"));
        Debug.Log(_sv.lstObjMap.Count);
        for (int i = 0; i < _sv.lstObjMap.Count; i++)
        {
            for (int j = 0; j < prefabsObjectMap.Count; j++)
            {
                if (prefabsObjectMap[j].name == _sv.lstObjMap[i].name)
                {
                    GameObject g = Instantiate(prefabsObjectMap[j], parent, false);
                    g.name = _sv.lstObjMap[i].name;
                    g.transform.position = new Vector3(_sv.lstObjMap[i].position_x, _sv.lstObjMap[i].position_y, _sv.lstObjMap[i].position_z);
                    g.transform.rotation = new Quaternion(_sv.lstObjMap[i].rotation_x, _sv.lstObjMap[i].rotation_y, _sv.lstObjMap[i].rotation_z, 0);
                    g.transform.localScale = new Vector3(_sv.lstObjMap[i].scale_x, _sv.lstObjMap[i].scale_y, _sv.lstObjMap[i].scale_z);
                    if (g.name != "Map" && g.name != "bg2" && g.name != "bg3" && g.name != "bg4" && g.name != "Gold Mine")
                    {
                        g.AddComponent<PolyNavObstacle>();
                    }
                }
            }

        }
        //Debug.Log("Whole JSON String 2: " + objJson["lstObjMap"]);
    }

    string loadJson(string _nameJson)
    {
        TextAsset _text = Resources.Load(_nameJson) as TextAsset;
        return _text.text;
    }
}
