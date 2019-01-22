using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FlyingArcher : Hero
{
    public override void Attack()
    {
        AnimAttack();
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType("Flying Archer", posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.right = transform.right;
        _bullet.GetComponent<Rigidbody2D>().velocity = transform.up * infoHero.speedBullet;
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame;
    }
    public override void MoveToPosition(Vector2 _toPos)
    {
        Vector2 dir = _toPos - new Vector2(transform.position.x, transform.position.y);
        transform.up = dir;
        if (Vector3.Distance(transform.position, _toPos) > infoHero.range / 5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _toPos, infoHero.speed / 5f * Time.deltaTime);
            AnimRun();
        }
    }

    public override void Die()
    {
        ObjectPoolingManager.Instance.ResetPoolForType("Flying Archer");
        AnimDie();
    }

    public override void CheckEnemy()
    {
        if (targetCompetitor == null)
        {
            List<Hero> lsCompetitor = new List<Hero>();
            if (gameObject.CompareTag("Hero"))
            {
                lsCompetitor = ObjectPoolingManager.Instance.lsEnemy;
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                lsCompetitor = ObjectPoolingManager.Instance.lsHero;
            }
            List<Hero> lsCompetitorTarget = new List<Hero>();
            Hero targetAttack = null;
            if (infoHero.typeHero == TypeHero.ChemThuong)
            {
                foreach (Hero obj in lsCompetitor)
                {
                    if (obj.infoHero.typeHero != TypeHero.ChemBay && obj.infoHero.typeHero != TypeHero.CungBay)
                    {
                        if (obj.typeAction != TypeAction.DIE && obj.infoHero.health > 0)
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
                    if (obj.typeAction != TypeAction.DIE && obj.infoHero.health > 0)
                    {
                        lsCompetitorTarget.Add(obj);
                    }
                }
            }
            targetAttack = CheckCompetitorNear(lsCompetitorTarget);
            if (targetAttack != null)
            {
                targetCompetitor = targetAttack.transform;
            }
            else
            {
                targetCompetitor = null;
            }
        }
    }

    public override void BeingAttacked(float _dame)
    {
        TakeDamage(_dame);
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 15;
        this.infoHero.health = 490;
        this.infoHero.dame = 96;
        this.infoHero.hitSpeed = 1f;
        this.infoHero.speed = 5;
        this.infoHero.price = 4000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        this.infoHero.range = 11;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.CungBay;
        this.infoHero.speedBullet = 10f;
    }

    // Use this for initialization
    public void Start()
    {
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);

    }

    // Update is called once per frame
    public void Update()
    {
        AnimtionUpdate();

        CheckEnemy();

        if (targetCompetitor != null)
        {
            AutoAttack();
            MoveToPosition(targetCompetitor.position);
        }
    }

    public void AutoAttack()
    {
        timeCheckAttack += Time.deltaTime;

        if (Vector3.Distance(transform.position, targetCompetitor.position) <= infoHero.range / 5f)
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
}
