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
    [Header("Info Hero")]
    public InfoHero infoHero = new InfoHero();

    [Header("Anim Hero")]
    public Animator animator;
    public TypeAction typeAction;
    public int numRun;
    public int numAttack;
    public bool isFly;
    public bool isRelease;

    public abstract void SetInfoHero();

    public abstract void CheckEnemy();

    public abstract void MoveToPosition(Vector2 _toPos);

    public abstract void Attack();

    public abstract void BeingAttacked(int _dame);

    public abstract void Die();
}
