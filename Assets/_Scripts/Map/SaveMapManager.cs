using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveMapManager : MonoBehaviour {
    SaveMap sv = new SaveMap();
    private GameObject[] allObjects;
    public List<GameObject> prefabsObjectMap;
    public Transform parent;
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
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/Map1.json", JsonUtility.ToJson(sv,true));
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
                    Instantiate(prefabsObjectMap[j], parent, false);
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
