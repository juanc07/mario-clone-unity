using UnityEngine;
using System.Collections;

public class FallingEnemyAIController : AIController{

	private Vector3 startPosition;

	private bool isMoveDown = true;

	public float downForce = 15f;
	public float upForce = 5f;
	public float distanceToAttack =5f;

	public override void Start (){
		base.Start ();
		startPosition = this.gameObject.transform.position;
		rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
	}
	
	public override void HitByMario ()
	{
		base.HitByMario ();
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			bool hit = aiHeroController.Hit();
			if(hit){
				//aiHeroController.Kill();
				playerHeroController.Bounce(0.35f);
				ShowHitParticle();
			}
		}
	}
	
	public override void TakeDamage (){
		base.TakeDamage ();
		
		if(!aiHeroController.IsDead){
			bool hit = aiHeroController.Hit();
			if(hit){
				ShowHitParticle();
			}
		}
	}
	
	public override void HitByEnemy (LevelObjectTagger levelObjectTagger)
	{
		base.HitByEnemy (levelObjectTagger);
		//Debug.Log("ice boss hit by enemy " + levelObjectTagger.levelTag);
		if(levelObjectTagger.levelTag == LevelTag.CannonBullet){
			//Invoke("ActivateDeath",0.3f);
			Invoke(Task.ActivateDeath.ToString(),0.3f);
		}
	}
	
	public override void HitByWeapon (LevelObjectTagger levelObject)
	{
		base.HitByWeapon (levelObject);
		if(levelObject.levelTag == LevelTag.Spore){
			bool hit = aiHeroController.Hit();
			if(hit){
				ShowHitParticle();
			}
		}else if( levelObject.levelTag == LevelTag.Crate ){
			ActivateDeath();
		}else if( levelObject.levelTag == LevelTag.Projectile ){
			ActivateDeath();
		}
	}
	
	public override void AttackPlayer ()
	{
		base.AttackPlayer ();
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			playerHeroController.Kill();
			isMoveDown = false;
		}
	}
	
	public override void InstantDeath ()
	{
		base.InstantDeath ();
		ActivateDeath();
	}
	
	public override void HitByMovingBrick ()
	{
		base.HitByMovingBrick ();
		InstantDeath();
	}
	
	private void ActivateDeath(){
		if(!aiHeroController.IsDead){
			aiHeroController.EnableDisableBody(false);
			ShowHitParticle();
			aiHeroController.Kill();
		}
	}
	
	private void ShowHitParticle(){
		soundManager.PlaySfx(SFX.StepOnEnemy);		
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 2f;
		Vector3 scale = new Vector3(2f,2f,2f);
		particleManager.CreateParticle(ParticleEffect.Hit1,newPosition,scale);
	}

	public override void HitGround ()
	{
		base.HitGround ();
		//Debug.Log("falling enemy hit ground!");
		if(aiHeroController.isHitDownSide){
			isMoveDown = false;
		}
	}

	public override void HitUnBreakbleBrick ()
	{
		base.HitUnBreakbleBrick ();
		if(aiHeroController.isHitDownSide){
			isMoveDown = false;
		}
	}

	public override void Update ()
	{
		base.Update ();
		//Debug.Log("falling enemy check distance " + distance);
		if(distance != 0){
			if(distance <=distanceToAttack){
				if(isMoveDown){
					AirMoveDown(downForce);
				}
			}
		}

		if(!isMoveDown){
			if(this.gameObject.transform.position.y <= startPosition.y){
				AirMoveUp(upForce);
			}else{
				ForceStop();
				isMoveDown = true;
			}
		}
	}
}
