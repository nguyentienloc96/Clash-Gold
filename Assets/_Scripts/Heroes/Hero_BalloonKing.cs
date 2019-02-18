﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_BalloonKing : Hero {

    public override void Attack()
    {
        AnimAttack();
        Vector3 diff = (targetCompetitor.transform.position - posShoot.position).normalized;
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.up = diff;
        _bullet.GetComponent<Rigidbody2D>().velocity = diff * infoHero.speedBullet;
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

    public override void SetInfoHero()
    {
        this.infoHero.ID = 4;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];       
    }

    string nameBullet;
    public void Start()
    {

        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = gameObject.name;

    }

    private float timeInstanceChild;
    public void Update()
    {
        HeroUpdate();
        if (targetCompetitor != null)
        {
            timeInstanceChild += Time.deltaTime;
            if (timeInstanceChild >= 1f)
            {
                GameManager.Instance.castlePlayer.InstantiateHero(19);
                timeInstanceChild = 0;
            }
        }
    }
}
