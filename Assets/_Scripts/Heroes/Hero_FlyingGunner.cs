using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FlyingGunner : Hero
{

    public override void Attack()
    {
        AnimAttack();
        Vector3 diff = (targetCompetitor.transform.position - posShoot.position).normalized;
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.up = diff;
        _bullet.GetComponent<Rigidbody2D>().velocity = diff * infoHero.speedBullet;
        float pX = ((infoHero.range / 5f) + 0.75f) / 5f;
        int xExp = (int)(Vector3.Distance(transform.position, targetCompetitor.transform.position) / pX);
        if (xExp <= 0)
            xExp = 1;
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame * infoHero.countHero * xExp;
        _bullet.GetComponent<Bullet>().isExplosion = false;
    }

    public override void Die()
    {
        AnimDie();
    }

    public override void BeingAttacked(float _dame)
    {
        TakeDamage(_dame);
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 6;
        this.infoHero = GameConfig.Instance.lsInfoHero[this.infoHero.ID - 1];
        if (GameConfig.Instance.lsEquip[infoHero.ID - 1].isHealth)
        {
            this.infoHero.health += infoHero.health * 0.5f;
        }
        if (GameConfig.Instance.lsEquip[infoHero.ID - 1].isAtk)
        {
            this.infoHero.dame += infoHero.dame * 0.5f;
            this.infoHero.dameDead += infoHero.dameDead * 0.5f;
            this.infoHero.counterDame += infoHero.counterDame * 0.5f;
        }
        if (GameConfig.Instance.lsEquip[infoHero.ID - 1].isHitSpeed)
        {
            this.infoHero.hitSpeed += infoHero.hitSpeed * 0.5f;
        }


    }

    string nameBullet;
    public void Start()
    {
        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = "ShortGunner" + gameObject.name;
    }

    public void Update()
    {
        HeroUpdate();
    }
}
