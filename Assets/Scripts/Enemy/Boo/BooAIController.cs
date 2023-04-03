using UnityEngine;
using System.Collections;

public class BooAIController : AIController{	
	public override void Start (){
		base.Start ();
		airOffsetX = 2f;
		airOffsetY = 1f;
	}
	
	public override void Update (){
		base.Update ();
	}
	
	public override void HitByMario (){
		base.HitByMario ();
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			bool hit = aiHeroController.Hit();
			if(hit){
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
		if(levelObjectTagger.levelTag == LevelTag.CannonBullet){
			Invoke(Task.ActivateDeath.ToString(),0.3f);
		}else if(levelObjectTagger.levelTag == LevelTag.Enemy){
			EnemyController enemyController = levelObjectTagger.gameObject.GetComponent<EnemyController>();
			if(enemyController!=null){
				if(enemyController.enemyType == EnemyType.FallingEnemy){
					Invoke(Task.ActivateDeath.ToString(),0.3f);
				}
			}
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
