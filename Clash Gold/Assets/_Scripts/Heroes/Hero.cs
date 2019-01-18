using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero
{
    public float health;
    public float dame;
    public float hitSpeed;
    public float speed;
    public float price;
    public float capWar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void MoveToPosition(Vector2 _toPos);

    public abstract void Attack();

    public abstract void Die();
}
