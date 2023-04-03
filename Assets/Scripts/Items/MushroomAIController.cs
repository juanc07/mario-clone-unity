using UnityEngine;
using System.Collections;

public class MushroomAIController : AIController{	

	//private bool hasJump = false;


	public override void Start (){
		base.Start ();
		playerHeroController.Bounce(0.35f);
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
		
		bool hit = aiHeroController.Hit();
		if(hit){
			ShowHitParticle();
		}
	}
	
	public override void AttackPlayer ()
	{
		base.AttackPlayer ();
		playerHeroController.Hit();
	}
	
	public override void InstantDeath ()
	{
		base.InstantDeath ();
		ShowHitParticle();
		aiHeroController.Kill();
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
			ShowHitParticle();
			aiHeroController.Kill();
		}
	}
	
	public override void HitByMovingBrick ()
	{
		base.HitByMovingBrick ();
		aiHeroController.Kill();
	}

	public override void Update ()
	{
		base.Update ();
	}
	
	private void ShowHitParticle(){
		soundManager.PlaySfx(SFX.StepOnEnemy);		
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 2f;
		Vector3 scale = new Vector3(2f,2f,2f);
		particleManager.CreateParticle(ParticleEffect.Hit1,newPosition,scale);
	}
}
