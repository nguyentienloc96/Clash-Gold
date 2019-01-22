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
    [Header("INFO HERO")]
    public InfoHero infoHero = new InfoHero();

    [Header("ANIM HERO")]
    public Animator animator;
    public TypeAction typeAction;
    public int numRun;
    public int numAttack;
    public bool isFly;
    public bool isRelease;
    public ParticleSystem parDie;

    [Header("ATTACK HERO")]
    public Transform posShoot;
    public Transform targetCompetitor;
    public float timeCheckAttack;

    public abstract void SetInfoHero();

    public abstract void CheckEnemy();

    public abstract void MoveToPosition(Vector2 _toPos);

    public abstract void Attack();

    public abstract void BeingAttacked(float _dame);

    public abstract void Die();

    public void AnimtionUpdate()
    {
        animator.SetBool("Run", typeAction == TypeAction.RUN ? true : false);
        animator.SetBool("Attack", typeAction == TypeAction.ATTACK ? true : false);
        animator.SetBool("Die", typeAction == TypeAction.DIE ? true : false);
    }

    public void AnimAttack()
    {
        typeAction = TypeAction.ATTACK;
    }

    public void AnimDie()
    {
        typeAction = TypeAction.DIE;
        parDie.Play();
        Destroy(gameObject, 0.5f);
    }

    public void AnimRun()
    {
        typeAction = TypeAction.RUN;
    }

    public void AnimIdle()
    {
        typeAction = TypeAction.IDLE;
    }

    protected GameObject CheckCompetitorNear(List<Hero> lsCompetitor)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestCompetitor = null;
        foreach (Hero obj in lsCompetitor)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, obj.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestCompetitor = obj.gameObject;
            }
        }

        return nearestCompetitor;
    }

    protected void TakeDamage(float _dame)
    {
        infoHero.health -= _dame;
        if(infoHero.health <= 0)
        {
            Die();
        }
    }
}
