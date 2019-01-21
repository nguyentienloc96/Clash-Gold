using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Canon : Hero {
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

    public float timeSurvive = 0;
    public override void SetInfoHero()
    {
        this.infoHero.ID = 11;
        this.infoHero.health = 742;
        this.infoHero.dame = 127;
        this.infoHero.hitSpeed = 0.8f;
        this.infoHero.speed = 0;
        this.infoHero.price = 3000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        this.infoHero.range = 5.5f;
        timeSurvive = GameConfig.Instance.Timecanonsurvive;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.Canon;

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
