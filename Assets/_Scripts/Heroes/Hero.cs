﻿using System.Collections;
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
    public TextMesh txtCountHero;

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
    public Hero targetCompetitor;
    public float timeCheckAttack;
    public ParticleSystem parHit;
    

    public abstract void SetInfoHero();

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
        if (gameObject.CompareTag("Enemy"))
        {
            TestManager.Instance.lsEnemy.Remove(this);
        }
        else
        {
            TestManager.Instance.lsHero.Remove(this);
        }
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

    protected Hero CheckCompetitorNear(List<Hero> lsCompetitor)
    {
        if (lsCompetitor.Count > 0)
        {
            float shortestDistance = Mathf.Infinity;
            Hero nearestCompetitor = null;
            foreach (Hero obj in lsCompetitor)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, obj.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestCompetitor = obj;
                }
            }

            return nearestCompetitor;
        }
        return null;
    }

    protected void TakeDamage(float _dame)
    {
        parHit.Play();
        infoHero.healthAll -= _dame;
        infoHero.numberHero = Mathf.CeilToInt(infoHero.healthAll / infoHero.health);
        if (infoHero.numberHero <= 0)
        {
            infoHero.numberHero = 0;
            Die();
        }
        txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero)
;
    }

    public void CheckEnemy()
    {
        if (targetCompetitor == null)
        {
            List<Hero> lsCompetitor = new List<Hero>();
            if (gameObject.CompareTag("Hero"))
            {
                lsCompetitor = TestManager.Instance.lsEnemy;
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                lsCompetitor = TestManager.Instance.lsHero;
            }
            List<Hero> lsCompetitorTarget = new List<Hero>();
            Hero targetAttack = null;
            if (infoHero.typeHero == TypeHero.ChemThuong)
            {

                foreach (Hero obj in lsCompetitor)
                {
                    if (obj.infoHero.typeHero != TypeHero.ChemBay && obj.infoHero.typeHero != TypeHero.CungBay)
                    {
                        if (obj.typeAction != TypeAction.DIE && obj.infoHero.numberHero > 0)
                        {
                            if (obj.infoHero.typeHero != TypeHero.Canon)
                            {
                                lsCompetitorTarget.Add(obj);
                            }
                            else
                            {
                                if (Vector3.Distance(transform.position, obj.transform.position) > infoHero.range / 5f)
                                {
                                    lsCompetitorTarget.Add(obj);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Hero obj in lsCompetitor)
                {
                    if (obj.typeAction != TypeAction.DIE && obj.infoHero.numberHero > 0)
                    {
                        if (obj.infoHero.typeHero != TypeHero.Canon)
                        {
                            lsCompetitorTarget.Add(obj);
                        }
                        else
                        {
                            if (Vector3.Distance(transform.position, obj.transform.position) > infoHero.range / 5f)
                            {
                                lsCompetitorTarget.Add(obj);
                            }
                        }
                    }
                }
            }
            targetAttack = CheckCompetitorNear(lsCompetitorTarget);
            if (targetAttack != null)
            {
                targetCompetitor = targetAttack;
            }
            else
            {
                targetCompetitor = null;
            }
        }
    }

    public void MoveToPosition(Vector3 _toPos)
    {
        Vector3 diff = _toPos - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        if (Vector3.Distance(transform.position, _toPos) > ((infoHero.range / 5f) + 0.75f))
        {
            transform.position = Vector3.MoveTowards(transform.position, _toPos, infoHero.speed / 10f * Time.deltaTime);
            AnimRun();
        }
    }

    public void AutoAttack()
    {
        timeCheckAttack += Time.deltaTime;

        if (Vector3.Distance(transform.position, targetCompetitor.transform.position) <= ((infoHero.range / 5f) + 0.75f))
        {
            if (timeCheckAttack >= infoHero.hitSpeed)
            {
                Attack();
                timeCheckAttack = 0;
            }
            else
            {
                AnimIdle();
            }
        }

    }

    public void HeroUpdate()
    {
        AnimtionUpdate();

        CheckEnemy();

        if (targetCompetitor != null)
        {
            AutoAttack();
            if (infoHero.typeHero != TypeHero.Canon)
            {
                MoveToPosition(targetCompetitor.transform.position);
            }
        }
    }

}
