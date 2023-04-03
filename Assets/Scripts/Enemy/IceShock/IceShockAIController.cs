using UnityEngine;
using System.Collections;

public class IceShockAIController : AIController{
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
			playerHeroController.Hit();
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
}
