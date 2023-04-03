using UnityEngine;
using System.Collections;

public class CannonBulletAIController : AIController{

	public bool hasMove =false;

	public override void StartMovingFromFullStop ()
	{
		base.StartMovingFromFullStop ();
		if(!hasMove){
			hasMove =true;
			soundManager.PlaySfx(SFX.CannonBullet);
			MoveLeft();
		}		
	}

	public override void Start (){
		base.Start ();
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
	
	public override void TakeDamage ()
	{
		base.TakeDamage ();

		if(!aiHeroController.IsDead){
			bool hit = aiHeroController.Hit();
			if(hit){
				aiHeroController.ApplyGravity =true;
				ShowHitParticle();
			}
		}
	}
	
	public override void AttackPlayer ()
	{
		base.AttackPlayer ();
		if(!playerHeroController.IsDead && !aiHeroController.IsDead){
			playerHeroController.Kill();
			InstantDeath();
			//Invoke("InstantDeath",0.5f);
		}
	}
	
	public override void InstantDeath ()
	{
		base.InstantDeath ();
		ActivateDeath();
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
	
	public override void HitByMovingBrick ()
	{
		base.HitByMovingBrick ();
		ActivateDeath();
	}

	public override void HitItem ()
	{
		base.HitItem ();
	}

	public override void HitByEnemy (LevelObjectTagger levelObjectTagger)
	{
		base.HitByEnemy (levelObjectTagger);
		ActivateDeath();
		//Invoke("InstantDeath",0.3f);
	}

	/*public override void StartMoving ()
	{
		base.StartMoving ();
	}*/

	public override void FullStop ()
	{
		base.FullStop ();
		//particleEffect.gameObject.SetActive(false);
	}

	private void ActivateDeath(){
		if(!aiHeroController.IsDead){
			aiHeroController.EnableDisableBody(false);
			ShowHitParticle();
			aiHeroController.Kill();
		}
	}

	private void ShowExplosionParticle(){
		Vector3 newPosition =	this.gameObject.transform.position;
		Vector3 scale = new Vector3(7f,7f,7f);
		particleManager.CreateParticle(ParticleEffect.Explosion1,newPosition,scale);
		soundManager.PlaySfx(SFX.SmallExplosion,1f);
	}
	
	private void ShowHitParticle(){
		soundManager.PlaySfx(SFX.StepOnEnemy);		
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 2f;
		Vector3 scale = new Vector3(2f,2f,2f);
		particleManager.CreateParticle(ParticleEffect.Hit1,newPosition,scale);
	}
}
