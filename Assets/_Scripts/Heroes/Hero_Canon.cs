﻿using System.Collections;
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
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame * infoHero.numberHero;
    }

    public override void Die()
    {
        AnimDie();
        //ObjectPoolingManager.Instance.ResetPoolForType(nameBullet);
    }

    public override void BeingAttacked(float _dame)
    {
        TakeDamage(_dame);
    }

    public float timeSurvive = 0;
    public override void SetInfoHero()
    {
        this.infoHero.ID = 11;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        timeSurvive = GameConfig.Instance.Timecanonsurvive;
    }

    string nameBullet;
    public void Start()
    {
        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = gameObject.name;
    }
    public void Update()
    {
        HeroUpdate();
    }
}
