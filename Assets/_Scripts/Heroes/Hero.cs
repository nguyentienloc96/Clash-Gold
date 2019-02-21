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

    [Header("ATTACK HERO")]
    public Transform posShoot;
    public GameObject targetCompetitor;
    [HideInInspector]
    public float timeCheckAttack;

    [Header("EFFECT")]
    public TextMesh txtCountHero;
    public ParticleSystem parDie;
    public ParticleSystem parHit;

    [Header("CHECK ATTACK")]
    public int IDGold;
    [HideInInspector]
    public bool isAttack;

    [Header("CHECK MOVE")]
    [HideInInspector]
    public GoldMine goldMineAttacking;
    [HideInInspector]
    public GoldMine goldMineProtecting;
    [HideInInspector]
    public Vector3 posStart;
    [HideInInspector]
    public Vector3 posMove;
    [HideInInspector]
    public bool isMove;
    [HideInInspector]
    public float timeCheckCameBack;
    [HideInInspector]
    public float speedMin;
    [HideInInspector]
    public List<Hero> lsChild;

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
        if (goldMineAttacking != null)
        {
            goldMineAttacking.lstCompetitorGoldMine.Remove(this.gameObject);
        }
        if (goldMineProtecting != null)
        {
            goldMineProtecting.lstHeroGoldMine.Remove(this);
        }
        if (gameObject.CompareTag("Hero"))
        {
            GameManager.Instance.castlePlayer.lstHeroRelease.Remove(this);
            if (infoHero.typeHero == TypeHero.Canon)
            {
                UIManager.Instance.buttonReleaseCanon.interactable = true;
            }
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

    protected GameObject CheckCompetitorNear(List<GameObject> lsCompetitor)
    {
        if (lsCompetitor.Count > 0)
        {
            float shortestDistance = Mathf.Infinity;
            GameObject nearestCompetitor = null;
            foreach (GameObject obj in lsCompetitor)
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
        int numberRemove = (int)(_dame / infoHero.health);
        infoHero.numberHero -= numberRemove;
        if (infoHero.numberHero <= 0)
        {
            infoHero.numberHero = 0;
            Die();
        }
        txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
    }

    public void CheckEnemy()
    {
        if (isAttack)
        {
            if (targetCompetitor == null || targetCompetitor.tag == "Castle")
            {
                if (gameObject.tag == "Hero")
                {
                    if (goldMineAttacking != null)
                    {
                        List<GameObject> lsTarget = new List<GameObject>();
                        if (goldMineAttacking.lstHeroGoldMine.Count > 0)
                        {
                            foreach (Hero obj in goldMineAttacking.lstHeroGoldMine)
                            {
                                lsTarget.Add(obj.gameObject);
                            }
                            targetCompetitor = CheckCompetitorNear(lsTarget);
                        }
                    }
                }
                else
                {
                    if (goldMineProtecting != null)
                    {
                        List<GameObject> lsTarget = new List<GameObject>();
                        if (goldMineProtecting.lstCompetitorGoldMine.Count > 0)
                        {
                            targetCompetitor = CheckCompetitorNear(goldMineProtecting.lstCompetitorGoldMine);
                        }
                        else
                        {
                            targetCompetitor = goldMineProtecting.castle;
                        }
                    }
                }
            }
        }
    }

    public void MoveToPosition(Vector3 _toPos)
    {
        float speed = isMove ? speedMin : infoHero.speed;
        Vector3 diff = _toPos - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        if (!isMove)
        {
            if (Vector3.Distance(transform.position, _toPos) > ((infoHero.range / 5f) + 0.75f))
            {
                transform.position = Vector3.MoveTowards(transform.position, _toPos, speed / 10f * Time.deltaTime);
                AnimRun();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _toPos, speed / 10f * Time.deltaTime);
            AnimRun();
        }
    }

    public void RotationHero(Vector3 _toPos)
    {
        Vector3 diff = _toPos - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
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

    private float timeDeadCanon;
    public void HeroUpdate()
    {
        if (GameManager.Instance.isPlay)
        {
            AnimtionUpdate();

            if(infoHero.typeHero == TypeHero.Canon)
            {
                timeDeadCanon += Time.deltaTime;
                if(timeDeadCanon >= GameConfig.Instance.Timecanonsurvive)
                {
                    Die();
                }
            }
            if (!isMove)
            {
                if (isAttack)
                {
                    CheckEnemy();

                    if (targetCompetitor != null)
                    {
                        AutoAttack();
                        if (infoHero.typeHero != TypeHero.Canon)
                        {
                            MoveToPosition(targetCompetitor.transform.position);
                        }
                        else
                        {
                            RotationHero(targetCompetitor.transform.position);
                        }
                    }
                }
                else
                {
                    if (goldMineProtecting != null)
                    {
                        if (transform.position != posStart)
                        {
                            timeCheckCameBack += Time.deltaTime;
                            if (timeCheckCameBack >= 3f)
                            {
                                posMove = posStart;
                                timeCheckCameBack = 0f;
                                isMove = true;
                            }
                        }
                    }
                }
            }
            else
            {
                MoveToPosition(posMove);
                if (transform.position == posMove)
                {
                    isMove = false;
                }
            }
        }
    }

    public void StartMoveToPosition(Vector3 _Pos)
    {
        if (gameObject.CompareTag("Hero"))
        {
            posMove = _Pos;
            posMove.z = 0f;
            isMove = true;
        }
    }

    public void AddHero(int _numberHero)
    {
        infoHero.numberHero += _numberHero;
        txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
    }

    public void StartChild()
    {

    }

    public void ExitGoldMine()
    {
        if (lsChild.Count > 0)
        {
            foreach (Hero hero in lsChild)
            {
                hero.Die();
            }
            lsChild.Clear();
        }
    }

    public void InstantiateChild(int idHero)
    {
        Hero hero = Instantiate(
        GameManager.Instance.lsPrefabsHero[idHero], transform.position, Quaternion.identity,
        GameManager.Instance.heroManager);

        hero.IDGold = IDGold;
        hero.isAttack = true;
        hero.gameObject.name = "Hero";
        hero.SetInfoHero();
        hero.infoHero.capWar = 0;
        hero.AddHero(infoHero.numberHero);
        lsChild.Add(hero);
    }
}
