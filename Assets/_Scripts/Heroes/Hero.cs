using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeAction
{
    IDLE,
    RUN,
    ATTACK,
    DIE
}

[System.Serializable]
public abstract class Hero : MonoBehaviour
{
    public InfoHero infoHero = new InfoHero();
    public Animator animator;
    public TypeAction typeAction;
    public bool isRelease;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void SetInfoHero();

    public abstract void CheckEnemy();

    public abstract void MoveToPosition(Vector2 _toPos);

    public abstract void Attack();

    public abstract void BeingAttacked(int _dame);

    public abstract void Die();
}
