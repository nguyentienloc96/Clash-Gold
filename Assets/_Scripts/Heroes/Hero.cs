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
    public int numIdle;
    public int numRun;
    public int numAttack;
    public bool isFly;
    public bool isRelease;

    [Header("ATTACK HERO")]
    public Transform posShoot;
    public Hero targetCompetitor;
    [HideInInspector]
    public float timeCheckAttack;

    [Header("EFFECT")]
    public TextMesh txtCountHero;
    public ParticleSystem parDie;
    public ParticleSystem parHit;

    [Header("CHECK ATTACK")]
    public int IDGold;
    public bool isAttack;

    [Header("CHECK MOVE")]
    [HideInInspector]
    public Vector3 posStart;
    [HideInInspector]
    public Vector3 posMove;
    [HideInInspector]
    public bool isMove;
    [HideInInspector]
    public List<Box> lsBoxPlayer;
    [HideInInspector]
    public bool isLsMove;
    [HideInInspector]
    public bool isPause;
    [HideInInspector]
    public float timeCheckCameBack;
    [HideInInspector]
    public float speedMin;

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
        Invoke("RemoveObj", 0.5f);
        Destroy(gameObject, 0.5f);
    }

    public void RemoveObj()
    {
        if (gameObject.CompareTag("Hero"))
        {
            GameManager.Instance.lsHero.Remove(this);
        }
        else
        {
            GameManager.Instance.lsEnemy.Remove(this);
        }
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
        int numberRemove = (int)(_dame / infoHero.health);
        int numberSub = numberRemove;
        if (numberSub > infoHero.numberHero)
        {
            numberSub = infoHero.numberHero;
        }
        if (infoHero.numberHero <= 0)
        {
            numberSub = 0;
        }
        infoHero.numberHero -= numberRemove;
        if (GameManager.Instance.isAttackGoldMineEnemy)
        {
            if (gameObject.tag == "Hero")
            {
                for (int i = 0; i < GameManager.Instance.castlePlayer.lsHouseRelease.Count; i++)
                {
                    if (infoHero.ID == GameManager.Instance.castlePlayer.lsHouseRelease[i].idHero)
                    {
                        if (GameManager.Instance.castlePlayer.lsHouseRelease[i].countHero > numberSub)
                            GameManager.Instance.castlePlayer.lsHouseRelease[i].countHero -= numberSub;
                        else
                            GameManager.Instance.castlePlayer.lsHouseRelease[i].countHero = 0;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.GolEnemyBeingAttack.lstHeroGoldMine.Count; i++)
                {
                    if (infoHero.ID == GameManager.Instance.GolEnemyBeingAttack.lstHeroGoldMine[i].infoHero.ID)
                    {
                        if (GameManager.Instance.GolEnemyBeingAttack.lstHeroGoldMine[i].infoHero.numberHero > numberSub)
                            GameManager.Instance.GolEnemyBeingAttack.lstHeroGoldMine[i].infoHero.numberHero -= numberSub;
                        else
                            GameManager.Instance.GolEnemyBeingAttack.lstHeroGoldMine[i].infoHero.numberHero = 0;
                        break;
                    }
                }
            }
        }
        else
        {
            if (gameObject.tag == "Hero")
            {
                for (int i = 0; i < GameManager.Instance.GolHeroBeingAttack.lstHeroGoldMine.Count; i++)
                {
                    if (infoHero.ID == GameManager.Instance.castlePlayer.lsHouseRelease[i].idHero)
                    {
                        if (GameManager.Instance.GolHeroBeingAttack.lstHeroGoldMine[i].infoHero.numberHero > numberSub)
                            GameManager.Instance.GolHeroBeingAttack.lstHeroGoldMine[i].infoHero.numberHero -= numberSub;
                        else
                            GameManager.Instance.GolHeroBeingAttack.lstHeroGoldMine[i].infoHero.numberHero = 0;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.lsEnemyAttackGoldMine.Count; i++)
                {
                    if (infoHero.ID == GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.ID)
                    {
                        if (GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.numberHero > numberSub)
                            GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.numberHero -= numberSub;
                        else
                            GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.numberHero = 0;
                        break;
                    }
                }
            }
        }

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
            if (targetCompetitor == null)
            {
                List<Hero> lsCompetitor = new List<Hero>();
                if (gameObject.CompareTag("Hero"))
                {
                    lsCompetitor = GameManager.Instance.lsEnemy;
                }
                else if (gameObject.CompareTag("Enemy"))
                {
                    lsCompetitor = GameManager.Instance.lsHero;
                }
                List<Hero> lsCompetitorTarget = new List<Hero>();
                Hero targetAttack = null;
                if (infoHero.typeHero == TypeHero.ChemThuong)
                {

                    foreach (Hero obj in lsCompetitor)
                    {
                        if (obj.infoHero.typeHero != TypeHero.ChemBay && obj.infoHero.typeHero != TypeHero.CungBay)
                        {
                            if (obj.typeAction != TypeAction.DIE)
                            {
                                lsCompetitorTarget.Add(obj);
                            }
                        }
                    }
                }
                else
                {
                    foreach (Hero obj in lsCompetitor)
                    {
                        if (obj.typeAction != TypeAction.DIE)
                        {
                            lsCompetitorTarget.Add(obj);
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

    public int check;

    public void MoveToLsPosition(List<Box> lsPos)
    {
        float speed = isMove ? speedMin : infoHero.speed;
        Vector3 diff = lsPos[check].transform.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        if (transform.position == lsPos[check].transform.position)
        {
            check++;
        }
        transform.position = Vector3.MoveTowards(transform.position, lsPos[check].transform.position, speed / 10f * Time.deltaTime);
        AnimRun();
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

    public void HeroUpdate()
    {
        if (GameManager.Instance.isPlay && !isPause)
        {
            AnimtionUpdate();
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
            }
            else
            {
                if (!isLsMove)
                {
                    MoveToPosition(posMove);
                    if (transform.position == posMove)
                    {
                        isMove = false;
                    }
                }
                else
                {
                    MoveToLsPosition(lsBoxPlayer);
                    if (transform.position == lsBoxPlayer[lsBoxPlayer.Count - 1].transform.position)
                    {
                        isMove = false;
                        isLsMove = false;
                    }
                }
            }
        }
    }

    public void StartMoveToPosition(Vector3 _Pos)
    {
        posMove = _Pos;
        posMove.z = 0f;
        isMove = true;
    }

    public void StartMoveToLsPosition(List<Box> lsPos)
    {
        lsBoxPlayer = lsPos;
        isLsMove = true;
        isMove = true;
    }

    public void AddHero(int _numberHero)
    {
        infoHero.numberHero += _numberHero;
        txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
    }

    public void StartChild()
    {
        animator.SetFloat("IndexIdle", numIdle);
    }

    public void InstantiateChild(int idHero, bool ishero)
    {
        if (ishero)
        {
            Hero hero = Instantiate(
            GameManager.Instance.lsPrefabsHero[idHero], transform.position, Quaternion.identity);
            hero.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            hero.IDGold = IDGold;
            hero.isAttack = true;
            hero.gameObject.name = "Hero";
            hero.SetInfoHero();
            hero.infoHero.capWar = 0;
            hero.AddHero(infoHero.numberHero);
            GameManager.Instance.lsChild.Add(hero);
            GameManager.Instance.lsHero.Add(hero);
        }
        else
        {
            Hero hero = Instantiate(
            GameManager.Instance.lsPrefabsEnemy[idHero], transform.position, Quaternion.identity);
            hero.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            hero.IDGold = IDGold;
            hero.isAttack = true;
            hero.gameObject.name = "Enemy";
            hero.SetInfoHero();
            hero.infoHero.capWar = 0;
            hero.AddHero(infoHero.numberHero);
            GameManager.Instance.lsChild.Add(hero);
            GameManager.Instance.lsEnemy.Add(hero);
        }
    }
}
