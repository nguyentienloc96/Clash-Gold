using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_DeadlyBABY : Hero {
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
        this.infoHero.ID = 4;
        this.infoHero.health = 32;
        this.infoHero.dame = 32;
        this.infoHero.hitSpeed = 1f;
        this.infoHero.speed = 10;
        this.infoHero.price = 0;
        this.infoHero.capWar = 0;
        this.infoHero.range = 0;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = true;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 3;
        this.infoHero.typeHero = TypeHero.ChemThuong;

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
