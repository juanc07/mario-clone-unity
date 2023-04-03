using UnityEngine;
using System.Collections;

public class IceBossAIController : AIController{
	private float rangeToAttack = 2f;
	private float chanceToAttack =0.30f;

	private float delayToAttack1 = 5f;
	private float delayToAttack2 = 5f;

	private bool isReadyToAttack1 = true;
	private bool isReadyToAttack2 = true;

	public bool activateDamage{set;get;}

	public IceBossAttackCollider iceBossAttackCollider;
	public GameObject iceBallParticleEffect;

	private ProjectileManager projectileManager;

	private float playerHeroPositionX;
	private float iceBossPositionX;

	private float distanceToShowHp = 30f;

	public override void Start (){
		base.Start ();
		projectileManager = GameObject.FindObjectOfType(typeof(ProjectileManager)) as ProjectileManager;
		OnOffIceBallParticleEffect(false);
	}

	public override void OnGameRestart ()
	{
		base.OnGameRestart ();
		StartMovingFromForceStop();
	}


	public override void Update (){
		base.Update ();
		
		if(playerHero==null) return;
		
		
		if(this.gameObject.transform.position.x < disappearXRange){
			aiHeroController.Kill();
		}

		playerHeroPositionX = playerHero.gameObject.transform.position.x;
		iceBossPositionX = this.gameObject.transform.position.x;

		if(distance <= distanceToShowHp){
			ShowBossHp();
		}
		/*float distanceFromTarget =(playerHeroPositionX - iceBossPositionX);
		if(distanceFromTarget<0){
			distanceFromTarget*=-1;
		}*/

		if(distance >= rangeToAttack && isReadyToAttack2 && !aiHeroController.isAttacking && !gameDataManager.player.IsDead){
			float rndAttack = Random.Range(0,1f);
			aiHeroController.attackDelay = 2f;
			isReadyToAttack2 = false;
			//Invoke("RefreshDelayToAttack2",delayToAttack2);
			Invoke(Task.RefreshDelayToAttack2.ToString(),delayToAttack2);
			if(rndAttack >= chanceToAttack){
				if(playerHeroPositionX > iceBossPositionX && aiHeroController.isFacingRight){
					ActivateIceBallAttack();
				}else if(playerHeroPositionX < iceBossPositionX && aiHeroController.isFacingLeft){
					ActivateIceBallAttack();
				}
			}
		}
	}

	private void ShowBossHp(){
		if(!gameDataManager.IsShowBossHP){
			gameDataManager.IsShowBossHP =true;
		}
	}

	private void InstantActivateIceBallAttack(){
		if(!aiHeroController.isAttacking && !gameDataManager.player.IsDead){
			aiHeroController.attackDelay = 2f;
			if(playerHeroPositionX > iceBossPositionX && aiHeroController.isFacingRight){
				ActivateIceBallAttack();
			}else if(playerHeroPositionX < iceBossPositionX && aiHeroController.isFacingLeft){
				ActivateIceBallAttack();
			}else{
				CheckWhereToGo();
				Invoke(Task.AimAttack.ToString(), 0.3f);
			}
		}
	}

	private void AimAttack(){
		ActivateIceBallAttack();
	}

	private void ActivateIceBallAttack(){
		//FullStop();
		OnOffIceBallParticleEffect(true);
		ForceStop();
		aiHeroController.Attack(AttackType.Attack2);
	}

	public override void AddEventListener ()
	{
		base.AddEventListener ();
		aiHeroController.OnHeroHit += OnIceBossHit;
		aiHeroController.OnHitComplete+=OnHitComplete;
		aiHeroController.OnAttackComplete+=OnAttackComplete;
		aiHeroController.OnMidAttackComplete+=OnMidAttackComplete;
		aiHeroController.OnNearAttackComplete+=OnNearAttackComplete;
		aiHeroController.OnHeroDied+=OnIceBossDied;
		iceBossAttackCollider.InitIceBossAIController(this);
	}
	
	public override void RemoveEventListener ()
	{
		base.RemoveEventListener ();
		if(aiHeroController!=null){
			aiHeroController.OnHeroHit -= OnIceBossHit;
			aiHeroController.OnHitComplete-=OnHitComplete;
			aiHeroController.OnAttackComplete-=OnAttackComplete;
			aiHeroController.OnMidAttackComplete-=OnMidAttackComplete;
			aiHeroController.OnNearAttackComplete-=OnNearAttackComplete;
			aiHeroController.OnHeroDied-=OnIceBossDied;
		}
	}

	private void OnIceBossHit(){
		FullStop();
	}

	private void OnHitComplete(){
		StartMovingFromFullStop();
		InstantActivateIceBallAttack();
	}

	private void OnMidAttackComplete(AttackType type){
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			if(type == AttackType.Attack1){
				activateDamage = true;
				//Debug.Log("ice boss mid attack 01 complete");
			}
		}
	}

	private void OnNearAttackComplete(AttackType type){
		if(type == AttackType.Attack2){
			OnOffIceBallParticleEffect(false);
			SummonIceBall();
		}
	}

	private void OnOffIceBallParticleEffect(bool val){
		iceBallParticleEffect.gameObject.SetActive(val);
	}

	private void OnAttackComplete(AttackType type){
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			if(type == AttackType.Attack1){
				activateDamage =false;
				EnableColliderAndMesh();
				StartMovingFromForceStop();
			}else if(type == AttackType.Attack2){
				EnableColliderAndMesh();
				StartMovingFromForceStop();
			}
		}
	}

	private void OnIceBossDied(){
		aiHeroController.EnableDisableBody(false);
		soundManager.PlaySfx(SFX.BossDied);
		//CancelInvoke("RefreshDelayToAttack1");
		CancelInvoke(Task.RefreshDelayToAttack1.ToString());
		//CancelInvoke("RefreshDelayToAttack2");
		CancelInvoke(Task.RefreshDelayToAttack2.ToString());
		StopAllCoroutines();
		gameDataManager.CurrentBossHP = 0;
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
			//FullStop();
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
			TakeDamage();
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
			bool hit = aiHeroController.Hit();
			if(hit){
				ShowHitParticle();
			}
		}else if( levelObject.levelTag == LevelTag.Projectile ){
			bool hit = aiHeroController.Hit();
			if(hit){
				ShowHitParticle();
			}
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
		if(!aiHeroController.IsDead){
			aiHeroController.EnableDisableBody(false);
			ShowHitParticle();
			aiHeroController.Kill();
		}
	}
	
	public override void HitByMovingBrick ()
	{
		base.HitByMovingBrick ();
		if(!aiHeroController.IsDead){
			aiHeroController.EnableDisableBody(false);
			ShowHitParticle();
			aiHeroController.Kill();
		}
	}
	
	private void ShowHitParticle(){
		soundManager.PlaySfx(SFX.StepOnEnemy);
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 5f;
		Vector3 scale = new Vector3(7f,7f,7f);
		particleManager.CreateParticle(ParticleEffect.Hit1,newPosition,scale);
	}

	public void Smash(){
		if(isReadyToAttack1 && !aiHeroController.isAttacking && !gameDataManager.player.IsDead){
			//float rndAttack = Random.Range(0,1f);
			aiHeroController.attackDelay = 2;
			activateDamage = false;
			isReadyToAttack1 = false;
			//Invoke("RefreshDelayToAttack1",delayToAttack1);
			Invoke(Task.RefreshDelayToAttack1.ToString(),delayToAttack1);
			//if(rndAttack >= chanceToAttack){
				//FullStop();
				ForceStop();
				aiHeroController.Attack(AttackType.Attack1);
			//}
		}
	}

	private void SummonIceBall(){
		Vector3 tempPosition;
		if(aiHeroController.isFacingRight){
			tempPosition =this.gameObject.transform.position;
			tempPosition.x += 6f;
		}else{
			tempPosition =this.gameObject.transform.position;
			tempPosition.x -= 6f;
		}
		projectileManager.CreateIceBallProjectile(ProjectileType.BossIceBall, tempPosition,this.gameObject.transform.rotation,aiHeroController,900f,aiHeroController.id);
	}

	private void RefreshDelayToAttack1(){
		isReadyToAttack1 = true;
	}

	private void RefreshDelayToAttack2(){
		isReadyToAttack2 = true;
	}
}
