using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BigMushroomAIController : AIController{
	private int rangeToAttack = 15;
	private float chanceToAttack =0.60f;

	private float delayToAttack = 5f;
	private bool isReadyToAttack = true;
	public GameObject summonPrefab;
	private int cacheRandom;

	private float sporeAttackDelay = 1.5f;
	private bool isSporeAttackReady = true;

	private Transform enemyHolder;
	private List<GameObject> enemies = new List<GameObject>();

	private float distanceToShowHp = 30f;

	public override void Start (){
		base.Start ();
		enemyHolder = this.gameObject.transform.parent;
	}

	public override void AddEventListener ()
	{
		base.AddEventListener ();
		aiHeroController.OnHeroHit += OnBigMushroomHit;
		aiHeroController.OnHitComplete+=OnHitComplete;
		aiHeroController.OnAttackComplete+=OnAttackComplete;
		aiHeroController.OnHeroDied+=OnBigMushroomDied;
	}

	public override void RemoveEventListener ()
	{
		base.RemoveEventListener ();
		if(aiHeroController!=null){
			aiHeroController.OnHeroHit -= OnBigMushroomHit;
			aiHeroController.OnHitComplete-=OnHitComplete;
			aiHeroController.OnAttackComplete-=OnAttackComplete;
			aiHeroController.OnHeroDied-=OnBigMushroomDied;
		}
	}
	
	public override void Update (){
		base.Update ();
		
		if(playerHero==null) return;

		
		if(this.gameObject.transform.position.x < disappearXRange){
			aiHeroController.Kill();
		}

		if(distance <= distanceToShowHp){
			ShowBossHp();
		}

		/*float distanceFromTarget =(playerHero.gameObject.transform.position.x - this.gameObject.transform.position.x);
		if(distanceFromTarget<0){
			distanceFromTarget*=-1;
		}*/

		if(distance < rangeToAttack && isReadyToAttack && !aiHeroController.isAttacking && !gameDataManager.player.IsDead){
			float rndAttack = Random.Range(0,1f);
			isReadyToAttack = false;
			//Invoke("RefreshDelayToAttack",delayToAttack);
			Invoke(Task.RefreshDelayToAttack.ToString(),delayToAttack);
			if(rndAttack >= chanceToAttack){
				FullStop();
				aiHeroController.Attack(AttackType.Attack2);
				//SummonObject(3);
			}
		}
	}

	private void ShowBossHp(){
		if(!gameDataManager.IsShowBossHP){
			gameDataManager.IsShowBossHP =true;
		}
	}

	private void OnBigMushroomHit(){
		FullStop();
	}

	private void OnHitComplete(){
		//Debug.Log("is hit big mushroom!!");
		SpreadSpore();
	}

	public void SpreadSpore(){
		if(!aiHeroController.isAttacking && !gameDataManager.player.IsDead && isSporeAttackReady){
			isSporeAttackReady = false;
			FullStop();
			AttackDefense();
			ShowSporeParticle();
			//Invoke("RefreshSporeAttack",sporeAttackDelay);
			Invoke(Task.RefreshSporeAttack.ToString(),sporeAttackDelay);
		}else{
			EnableColliderAndMesh();
			StartMovingFromFullStop();
		}
	}

	private void RefreshSporeAttack(){
		isSporeAttackReady = true;
	}

	private void OnAttackComplete(AttackType type){
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			if(type == AttackType.Attack2){
				SummonObject(3);
				EnableColliderAndMesh();
				//CheckWhereToGo();
				StartMovingFromFullStop();
			}else if(type == AttackType.Attack1){
				//CheckWhereToGo();
				EnableColliderAndMesh();
				StartMovingFromFullStop();
			}
		}
	}

	private void ShowSporeParticle(){
		//SetColliderSize(4f);
		Vector3 tempPosition = this.gameObject.transform.position;
		tempPosition.y += 3f;
		particleManager.CreateParticle( ParticleEffect.BigMushroomSpore,tempPosition,new Vector3(10f,10f,10f));
		soundManager.PlaySfx(SFX.Spore);
	}

	private void SummonObject(int summonCount){
		if(!aiHeroController.IsDead){
			int count = summonCount;
			float delay =0;
			for(int index =0; index<count;index++){
				Vector3 tempPosition = this.gameObject.transform.position;
				tempPosition.x = this.gameObject.transform.position.x + GetRandom(-20,20,2);
				tempPosition.y = this.gameObject.transform.position.y + GetRandom(10,20,2);
				tempPosition.z = 0;
				Quaternion tempRotation = Quaternion.Euler(new Vector3(0,0,0));
				StartCoroutine(DelaySummon(delay, summonPrefab,tempPosition,tempRotation ));
				delay+=0.4f;
				//GameObject summonObject = Instantiate( summonPrefab,tempPosition,tempRotation ) as GameObject;
				//soundManager.PlaySfx(SFX.FallingEnemy,0.5f);
			}
		}
	}

	private IEnumerator DelaySummon(float delay,GameObject prefab, Vector3 position, Quaternion rotation){
		yield return new WaitForSeconds(delay);
		GameObject summonObject = GetEnemy();
		if(summonObject==null){
			summonObject = Instantiate( prefab,position,rotation ) as GameObject;
			summonObject.gameObject.transform.parent = enemyHolder;
			enemies.Add(summonObject);
		}else{
			summonObject.gameObject.transform.position = position;
			summonObject.gameObject.transform.rotation = rotation;
		}
		soundManager.PlaySfx(SFX.FallingEnemy,0.5f);
	}


	private int GetRandom(int min, int max, int offset){
		int rnd = Random.Range(min,max);
		while(rnd==cacheRandom ||rnd==(cacheRandom - offset) || rnd==(cacheRandom + offset) ){
			rnd = Random.Range(min,max);
		}
		cacheRandom =rnd;
		return rnd;
	}

	private GameObject GetEnemy(  ){
		GameObject foundEnemy =null;
		int count = enemies.Count;
		for(int index=0;index<count;index++){
			if(enemies[index]!=null){
				enemies[index].gameObject.SetActive(true);
				LevelObjectTagger levelTagger = enemies[index].gameObject.transform.GetComponentInChildren<LevelObjectTagger>();
				if(levelTagger!=null){
					if(levelTagger.levelTag == LevelTag.Enemy){
						EnemyController enemyController = levelTagger.gameObject.transform.parent.GetComponent<EnemyController>();
						if(enemyController!=null){
							if(enemyController.IsDead){
								enemyController.ResetData();
								foundEnemy = enemies[index];
								//Debug.Log("reused enemy");
								break;
							}
						}
					}
				}
			}
		}


		return foundEnemy;
	}

	private void ClearAllEnemy( ){
		int count = enemies.Count-1;
		for(int index=count;index>=0;index--){
			if(enemies[index]!=null){
				Destroy(enemies[index]);
			}
		}
	}

	private void RefreshDelayToAttack(){
		isReadyToAttack = true;
	}

	public override void Attack ()
	{
		base.Attack ();
		//aiHeroController.Attack(AttackType.Attack2);
	}

	private void AttackDefense(){
		aiHeroController.Attack(AttackType.Attack1);
	}

	public override void HitByMario ()
	{
		base.HitByMario ();
		if( !playerHeroController.IsDead && !aiHeroController.IsDead && !aiHeroController.isHit){
			//Debug.Log("big mushroom hit by mario!");
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
		if(!aiHeroController.IsDead && !aiHeroController.isHit){
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
			//Debug.Log("big mushroom attack player");
		}
	}

	public override void HitByEnemy (LevelObjectTagger levelObjectTagger)
	{
		base.HitByEnemy (levelObjectTagger);
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			SpreadSpore();
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

	public override void InstantDeath ()
	{
		base.InstantDeath ();
		//aiHeroController.Kill();
		//gameDataManager.CurrentBossHP = 0;
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			bool hit = aiHeroController.Hit();
			if(hit){
				//aiHeroController.EnableDisableBody(false);
				playerHeroController.Bounce(0.35f);
				ShowHitParticle();
			}
		}
	}

	private void OnBigMushroomDied(){
		aiHeroController.EnableDisableBody(false);
		soundManager.PlaySfx(SFX.BossDied);
		StopAllCoroutines();
		//CancelInvoke("RefreshSporeAttack");
		CancelInvoke(Task.RefreshSporeAttack.ToString());
		gameDataManager.CurrentBossHP = 0;
	}

	private void ShowHitParticle(){
		soundManager.PlaySfx(SFX.StepOnEnemy);		
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 2f;
		Vector3 scale = new Vector3(2f,2f,2f);
		particleManager.CreateParticle(ParticleEffect.Hit1,newPosition,scale);
	}

	public override void OnGameRestart ()
	{
		base.OnGameRestart ();
		StopAllCoroutines();
		CancelInvoke(Task.RefreshSporeAttack.ToString());
		StartMovingFromForceStop();
		ClearAllEnemy();
	}
	
}
