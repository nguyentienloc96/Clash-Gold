using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_ShortGunner : Hero {
    public override void Attack()
    {

    }
    public override void MoveToPosition(Vector2 _toPos)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void CheckEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override void BeingAttacked(int _dame)
    {
        throw new System.NotImplementedException();
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 7;
        this.infoHero.health = 696;
        this.infoHero.dame = 620;
        this.infoHero.hitSpeed = 2.2f;
        this.infoHero.speed = 5;
        this.infoHero.price = 5000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Lo;
        this.infoHero.range = 4;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.CungThuong;

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
    }

}
