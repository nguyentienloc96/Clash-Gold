using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FlyingArcher : Hero {
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

    }

    // Use this for initialization
    void Start()
    {
        SetInfoHero();
        //Debug.Log(this.infoHero);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
