using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LakituAIController : AIController{	

	private float attackDelay = 2f;
	private bool isReadyToAttack = true;
	private float chanceToAttack = 0.65f;

	private float distanceToAttack = 10f;
	private ProjectileManager projectileManager;
	public GameObject lakituThrowPrefab;
	private List<GameObject> summonMonster = new List<GameObject>();

	public override void Start (){
		base.Start ();
		airOffsetX = 12f;
		airOffsetY = 2f;
		//Debug.Log("Start lakitu ai controller");
	}

	public override void AddEventListener ()
	{
		base.AddEventListener ();
		projectileManager = GameObject.FindObjectOfType(typeof(ProjectileManager)) as ProjectileManager;
		projectileManager.OnActivateDeactivateProjectile+=OnActivateDeactivateProjectile;

		aiHeroController.OnAttackComplete+=OnAttackComplete;
		aiHeroController.OnMidAttackComplete += OnMidAttackComplete;
	}

	public override void RemoveEventListener ()
	{
		base.RemoveEventListener ();
		aiHeroController.OnAttackComplete-=OnAttackComplete;		
		aiHeroController.OnMidAttackComplete -= OnMidAttackComplete;
		projectileManager.OnActivateDeactivateProjectile-=OnActivateDeactivateProjectile;
	}

	private void OnMidAttackComplete(AttackType type){
		if(type == AttackType.Attack1){
			SummonIceBall();
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
		//Debug.Log(" check distance " + distance);
		if(playerHeroController!=null){
			if(playerHeroController.isIdle && distance <= distanceToAttack  && !gameDataManager.IsLevelComplete){
				ThrowIceBall();
			}
		}
	}

	private void ThrowIceBall(){
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

	private void SummonIceBall(){
		if(!aiHeroController.IsDead && !playerHeroController.IsDead){
			Vector3 tempPosition;
			if(aiHeroController.isFacingRight){
				tempPosition =this.gameObject.transform.position;
				tempPosition.x += 3f;
			}else{
				tempPosition =this.gameObject.transform.position;
				tempPosition.x -= 3f;
			}
			
			Vector3 scale = new Vector3(1f,1f,1f);
			projectileManager.CreateIceBallProjectile(ProjectileType.Iceball,tempPosition,this.gameObject.transform.rotation,aiHeroController,350f,aiHeroController.id);
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
		}else if(levelObjectTagger.levelTag == LevelTag.Enemy){
			EnemyController enemyController = levelObjectTagger.gameObject.GetComponent<EnemyController>();
			if(enemyController!=null){
				if(enemyController.enemyType == EnemyType.FallingEnemy){
					//Invoke("ActivateDeath",0.3f);
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

	private void ClearSummonMonster(){
		int summonCount = summonMonster.Count;
		bool found = false;

		for(int index=0;index<summonCount;index++){
			if(summonMonster[index]!=null){
				Destroy(summonMonster[index]);
			}
		}
		summonMonster.Clear();
	}

	public override void OnLevelStart ()
	{
		base.OnLevelStart ();
		ClearSummonMonster();
	}

	public override void OnGameRestart ()
	{
		base.OnGameRestart ();
		ClearSummonMonster();
	}


	private void OnActivateDeactivateProjectile(bool isActivate,int id,Transform projectileTransform){
		if(!isActivate){
			GameObject lakituThrow = Instantiate(lakituThrowPrefab,projectileTransform.position,aiTransform.rotation) as GameObject;
			lakituThrow.transform.parent = aiTransform.parent;
			summonMonster.Add(lakituThrow);
		}
	}
}
