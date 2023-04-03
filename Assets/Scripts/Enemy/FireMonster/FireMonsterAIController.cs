using UnityEngine;
using System.Collections;

public class FireMonsterAIController : AIController{	

	private ProjectileManager projectileManager;
	private Vector3 projectileTempPosition;

	public float attackDelay = 2f;
	public bool isReadyToAttack = true;
	public float chanceToAttack = 0.65f;
	
	public float distanceToAttack = 10f;

	public override void Start (){
		base.Start ();
		projectileManager = GameObject.FindObjectOfType(typeof(ProjectileManager)) as ProjectileManager;
	}

	public override void AddEventListener ()
	{
		base.AddEventListener ();
		aiHeroController.OnAttackComplete+=OnAttackComplete;
		aiHeroController.OnMidAttackComplete += OnMidAttackComplete;
	}
	
	public override void RemoveEventListener ()
	{
		base.RemoveEventListener ();
		aiHeroController.OnAttackComplete-=OnAttackComplete;		
		aiHeroController.OnMidAttackComplete -= OnMidAttackComplete;
	}
	
	private void OnMidAttackComplete(AttackType type){
		if(type == AttackType.Attack1){
			//ForceStop();
			SummonFireball();
			Invoke("SummonFireball",0.5f);
		}
	}
	
	private void OnAttackComplete(AttackType type){
		if(type == AttackType.Attack1){
			StartMovingFromFullStop();
		}
	}

	public override void Update ()
	{
		base.Update ();
		if(playerHeroController!=null){
			if(playerHeroController.isIdle && distance <= distanceToAttack  && !gameDataManager.IsLevelComplete){
				if(playerPositionX > enemyPositionX && aiHeroController.isFacingRight){
					if(distanceY >= 3f || distanceY <=3f){
						if((enemyPositionY + 1.5f) >= playerPositionY){
							//Debug.Log("in range y fire now!");
							ThrowFireball();
						}
					}
				}else if(playerPositionX < enemyPositionX && aiHeroController.isFacingLeft){
					if(distanceY >= 3f || distanceY <=3f){
						if((enemyPositionY + 1.5f) >= playerPositionY){
							//Debug.Log("in range y fire now!");
							ThrowFireball();
						}
					}
				}else if(playerPositionX > enemyPositionX && aiHeroController.isFacingLeft){
					ForceMoveRight();
				}else if(playerPositionX < enemyPositionX && aiHeroController.isFacingRight){
					ForceMoveLeft();
				}
			}
		}
	}

	private void ThrowFireball(){
		if(isReadyToAttack){
			isReadyToAttack = false;
			float rnd = Random.Range(0,1f);
			if(rnd > chanceToAttack){
				FullStop();
				aiHeroController.attackDelay = 1f;
				aiHeroController.Attack(AttackType.Attack1);
			}
			//Invoke("RefreshAttack",attackDelay);
			Invoke(Task.RefreshAttack.ToString(),attackDelay);
		}
	}

	private void RefreshAttack(){
		isReadyToAttack = true;
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

	private void SummonFireball(){
		if(!aiHeroController.IsDead && !playerHeroController.IsDead){
			if(aiHeroController.isFacingRight){
				projectileTempPosition =aiTransform.position;
				projectileTempPosition.y += 2f;
				projectileTempPosition.x += 1f;
				//Debug.Log("summon fireball right");
			}else{
				projectileTempPosition =aiTransform.position;
				projectileTempPosition.y += 2f;
				projectileTempPosition.x -= 1f;
				//Debug.Log("summon fireball left");
			}
			
			Vector3 scale = new Vector3(1f,1f,1f);
			projectileManager.CreateFireBallProjectile(ProjectileType.Fireball,projectileTempPosition,aiTransform.rotation,aiHeroController,250f,aiHeroController.id);
		}
	}
}
