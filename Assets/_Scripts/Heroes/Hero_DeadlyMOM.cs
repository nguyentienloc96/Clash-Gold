using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_DeadlyMOM : Hero {
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
        this.infoHero.ID = 3;
        this.infoHero.health = 787;
        this.infoHero.dame = 69;
        this.infoHero.hitSpeed = 1f;
        this.infoHero.speed = 5;
        this.infoHero.price = 5000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Lo;
        this.infoHero.range = 5;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = true;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 4;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.CungThuong;

    }
    // Use this for initialization
    void Start()
    {
        SetInfoHero();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
