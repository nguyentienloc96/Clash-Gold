using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Canon : Hero {

    public override void Attack()
    {
        AnimAttack();
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.right = transform.right;
        _bullet.GetComponent<Rigidbody2D>().velocity = transform.up * infoHero.speedBullet;
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame * infoHero.countHero;
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

    public float timeSurvive = 0;
    public override void SetInfoHero()
    {
        this.infoHero.ID = 9;
        this.infoHero = GameConfig.Instance.lsInfoHero[this.infoHero.ID - 1];
        this.infoHero.dame += GameConfig.Instance.AtkArcher * GameManager.Instance.atkArcher + GameConfig.Instance.AtkWalk * GameManager.Instance.atkWalk;
        this.infoHero.health += GameConfig.Instance.HlthWalk * GameManager.Instance.hlthWalk + GameConfig.Instance.HlthArcher * GameManager.Instance.hlthArcher;
        this.infoHero.dameDead += GameConfig.Instance.AtkArcher * GameManager.Instance.atkArcher + GameConfig.Instance.AtkWalk * GameManager.Instance.atkWalk;
        this.infoHero.counterDame += GameConfig.Instance.AtkArcher * GameManager.Instance.atkArcher + GameConfig.Instance.AtkWalk * GameManager.Instance.atkWalk;
        timeSurvive = GameConfig.Instance.Timecanonsurvive;
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
        nameBullet = "Canon"+gameObject.name;
    }
    public void Update()
    {
        HeroUpdate();
    }
}
