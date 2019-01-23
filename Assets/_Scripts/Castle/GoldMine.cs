using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;


[System.Serializable]
public class GoldMine : MonoBehaviour {
    public float health;
    public float goldIncrease;
    public int level;
    public Collider2D colliderLand;
	// Use this for initialization
	void Start () {
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnNextDay()
    {

    }

    void SpawmGold()
    {

    }
}
