using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_DrunkenBomber : Hero {
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


    public float spreadDame;
    public override void SetInfoHero()
    {
        this.infoHero.ID = 9;
        this.infoHero.health = 311;
        this.infoHero.dame = 271;
        this.infoHero.hitSpeed = 1.9f;
        this.infoHero.speed = 5;
        this.infoHero.price = 3000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Hi;
        this.infoHero.range = 5;
        spreadDame = 4 * GameConfig.Instance.UnitRange;
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
