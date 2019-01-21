using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_BalloonKing : Hero {
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
        this.infoHero.ID = 5;
        this.infoHero.health = 750;
        this.infoHero.dame = 260;
        this.infoHero.hitSpeed = 1.5f;
        this.infoHero.speed = 5;
        this.infoHero.price = 4000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        this.infoHero.range = 1.7f;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = true;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 6;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.CungBay;

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
