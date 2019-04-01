﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_DrunkenBomber : Hero
{

    public override void Attack()
    {
        AnimAttack();
        Vector3 diff = (targetCompetitor.transform.position - posShoot.position).normalized;
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.up = diff;
        _bullet.GetComponent<Rigidbody2D>().velocity = diff * infoHero.speedBullet;
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame * infoHero.countHero;
        _bullet.GetComponent<Bullet>().isBoom = true;
        _bullet.GetComponent<Bullet>().isCanFly = true;
        _bullet.GetComponent<Bullet>().rangeBoom = infoHero.rangeBoom;
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


    public float spreadDame;
    public override void SetInfoHero()
    {
        this.infoHero.ID = 7;
        this.infoHero = GameConfig.Instance.lsInfoHero[this.infoHero.ID - 1];
        spreadDame = 4 * GameConfig.Instance.UnitRange;
    }
    string nameBullet;
    public void Start()
    {
        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = "DrunkenBom"+gameObject.name;
    }

    public void Update()
    {
        HeroUpdate();
    }
}
