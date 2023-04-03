using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LavaBossAIController : AIController{
	private float distanceToShowHp = 30f;

	public bool isReadyToAttack1 = true;
	private float attack1_Delay = 0.5f;
	private float attack1_coolDown = 3f;
	private float chanceToAttack1 = 0.5f;

	public bool isReadyToAttack2 = true;
	private float attack2_Delay = 0.5f;
	private float attack2_coolDown = 3f;
	private float chanceToAttack2 = 0.1f;

	//private float percent = 0;

	private int dropCount = 3;
	private int fireballCount = 10;

	public GameObject[] dropItems;
	private List<GameObject> dropItemCollection =new List<GameObject>();
	private Vector3 tempDropItemPosition;
	private GameObject item;
	private ProjectileManager projectileManager;

	public bool activateDamage{set;get;}
	public Transform dropItemHolder;

	private SoundManager soundManager;

	private List<GameObject> crateCollection = new List<GameObject>();
	public Transform crateHolder;

	public override void Start (){
		base.Start ();
		soundManager = SoundManager.GetInstance();
		projectileManager =  GameObject.FindObjectOfType(typeof(ProjectileManager)) as ProjectileManager;
	}

	private void ResetStatus(){
		isReadyToAttack1 = true;
		isReadyToAttack2 = true;
		activateDamage = false;
	}

	public override void OnLevelStart ()
	{
		base.OnLevelStart ();
		//ForceDeActivateCrateCollection();
		ForceStop();
		gameDataManager.IsShowBossHP =false;
		ClearDropItems();
		ResetStatus();

	}

	public override void OnGameRestart ()
	{
		base.OnGameRestart ();
		Invoke("ForceDeActivateCrateCollection", 0.3f);
		//ForceDeActivateCrateCollection();
		ForceStop();
		gameDataManager.IsShowBossHP =false;
		ClearDropItems();
		ResetStatus();
	}

	public override void AddEventListener ()
	{
		base.AddEventListener ();
		aiHeroController.OnHeroHit += OnLavaBossHit;
		aiHeroController.OnMidHit += OnMidHit;
		aiHeroController.OnHitComplete+=OnHitComplete;
		aiHeroController.OnAttackComplete+=OnAttackComplete;
		aiHeroController.OnMidAttackComplete+=OnMidAttackComplete;
		aiHeroController.OnMidNearAttackComplete+=OnMidNearAttackComplete;
		aiHeroController.OnNearAttackComplete+=OnNearAttackComplete;
		aiHeroController.OnHeroDied+=OnLavaBossDied;
	}
	
	public override void RemoveEventListener ()
	{
		base.RemoveEventListener ();
		if(aiHeroController!=null){
			aiHeroController.OnHeroHit -= OnLavaBossHit;
			aiHeroController.OnMidHit += OnMidHit;
			aiHeroController.OnHitComplete-=OnHitComplete;
			aiHeroController.OnAttackComplete-=OnAttackComplete;
			aiHeroController.OnMidAttackComplete-=OnMidAttackComplete;
			aiHeroController.OnMidNearAttackComplete-=OnMidNearAttackComplete;
			aiHeroController.OnNearAttackComplete-=OnNearAttackComplete;		
			aiHeroController.OnHeroDied+=OnLavaBossDied;
		}
	}

	public override void OnPlayerDead ()
	{
		base.OnPlayerDead ();
		CancelInvoke("Attack1");
		CancelInvoke("Attack2");
		CancelInvoke("RefreshAttack1");
		CancelInvoke("RefreshAttack2");
		gameDataManager.IsShowBossHP =false;
		ClearDropItems();
		ResetStatus();
	}

	private void OnLavaBossHit(){

	}

	private void OnMidHit(){
		activateDamage = true;
	}

	private void OnHitComplete(){
		ResetStatus();
	}

	private void OnAttackComplete(AttackType type){
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			if(type == AttackType.Attack1){
				activateDamage = false;
				StartMovingFromForceStop();
				Invoke("RefreshAttack1",attack1_coolDown);
			}else if(type == AttackType.Attack2){
				activateDamage = false;
				StartMovingFromForceStop();
				Invoke("RefreshAttack2",attack2_coolDown);
			}
		}
	}

	private void OnMidAttackComplete(AttackType type){
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			if(type == AttackType.Attack2){
				activateDamage = true;
				if(!playerHeroController.IsDead && !gameDataManager.IsLevelComplete){
					ShowSmashParticles();
					SummonItems();
				}
			}
		}
	}

	private void OnMidNearAttackComplete(AttackType type){
		if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			if(type == AttackType.Attack1){
				activateDamage = true;
			}
		}
	}

	private void OnNearAttackComplete(AttackType type){
		/*if( !playerHeroController.IsDead && !aiHeroController.IsDead){
			if(type == AttackType.Attack1){
				activateDamage = true;
			}else if(type == AttackType.Attack2){
				activateDamage = true;
				if(!playerHeroController.IsDead && !gameDataManager.IsLevelComplete){
					SummonItems();
				}
			}
		}*/
	}

	private void ShowSmashParticles(){
		Vector3 newPosition =	aiTransform.position;
		newPosition.z =0;
		Vector3 scale = new Vector3(1f,1f,1f);
		particleManager.CreateParticle(ParticleEffect.LavaBossSmash,newPosition,scale);		
		soundManager.PlaySfx(SFX.CrateExplosion,1f);
	}

	private void OnLavaBossDied(){
		//aiHeroController.EnableDisableBody(false);
		//StopAllCoroutines();
		//gameDataManager.CurrentBossHP = 0;

		CancelInvoke("Attack1");
		CancelInvoke("Attack2");
		CancelInvoke("RefreshAttack1");
		CancelInvoke("RefreshAttack2");
		gameDataManager.IsShowBossHP =false;
		ClearDropItems();
		ResetStatus();
	}

	public void ForceMoveLeft(){
		aiHeroController.isWalking =true;
		MoveLeft();
	}
	
	public void ForceMoveRight(){
		aiHeroController.isWalking =true;
		MoveRight();
	}

	private void SummonItems(){
		for(int index =0; index<dropCount;index++){
			int rnd = Random.Range(0,dropItems.Length);
			tempDropItemPosition = aiTransform.position;
			tempDropItemPosition.x += Random.Range(-35,35);
			tempDropItemPosition.y += Random.Range(40,100);
			tempDropItemPosition.z = 0;

			if(rnd == 1 || rnd == 2){
				item = GetCrate();
				if(item ==null){
					item =Instantiate(dropItems[rnd],tempDropItemPosition,aiTransform.rotation) as GameObject;
					item.gameObject.transform.parent = crateHolder;
					crateCollection.Add(item);
				}else{
					item.gameObject.transform.position = tempDropItemPosition;
					item.gameObject.transform.rotation = aiTransform.rotation;
				}
			}else{
				item =Instantiate(dropItems[rnd],tempDropItemPosition,aiTransform.rotation) as GameObject;
				item.gameObject.transform.parent = dropItemHolder;
				dropItemCollection.Add(item);
			}
		}

		for(int index =0; index<fireballCount;index++){
			tempDropItemPosition = aiTransform.position;
			tempDropItemPosition.x += Random.Range(-35,35);
			tempDropItemPosition.y += Random.Range(40,100);
			tempDropItemPosition.z = 0;
			projectileManager.CreateFireBallProjectile(ProjectileType.Fireball,tempDropItemPosition,aiTransform.rotation,aiHeroController,0,aiHeroController.id);
		}
	}

	private GameObject GetCrate(){
		int crateCount = crateCollection.Count;
		GameObject found = null;

		for(int index=0;index<crateCount;index++){
			if(crateCollection[index]!=null){
				if(!crateCollection[index].gameObject.activeSelf){
					found = crateCollection[index];
					found.SetActive(true);
					CrateColliderController crateColliderController = found.GetComponent<CrateColliderController>();
					if(crateColliderController!=null){
						crateColliderController.TurnOnOffGravity(true);
					}

					//Debug.Log("lavaBossAIController found inactive crate reused it");
					break;
				}
			}
		}

		return found;
	}

	private void ForceDeActivateCrateCollection(){
		int crateCount = crateCollection.Count;		
		for(int index=0;index<crateCount;index++){
			if(crateCollection[index]!=null){
				if(crateCollection[index].gameObject.activeSelf){
					CrateColliderController crateColliderController = crateCollection[index].gameObject.GetComponent<CrateColliderController>();
					if(crateColliderController!=null){
						crateColliderController.TurnOnOffGravity(false);
					}
					crateCollection[index].gameObject.SetActive(false);
					//Debug.Log("LavaBossAIController found active crate deactivate it!");
				}
			}
		}
	}

	private void ClearDropItems(){
		int droppedItems = dropItemCollection.Count - 1;
		for(int index =droppedItems; index>=0;index--){
			if(dropItemCollection[index]!=null){
				Destroy(dropItemCollection[index]);
				dropItemCollection.RemoveAt(index);
			}
		}
		dropItemCollection.Clear();
	}

	public override void Update ()
	{
		base.Update ();

		if(playerHero==null) return;

		if(distance <= distanceToShowHp){
			ShowBossHp();
		}

		if(distance <= 0.5f){
			if(!aiHeroController.isAttacking || !aiHeroController.isIdle && !aiHeroController.isHit){
				ForceStop();
			}

			if(aiHeroController.hp <= 7){
				ActivateAttack2();
			}else{
				ActivateAttack1();
			}
		}else{
			if(gameDataManager.IsShowBossHP){
				if(!aiHeroController.isAttacking && !aiHeroController.isHit){
					if(playerPositionX > enemyPositionX ){
						ForceMoveRight();
					}else if(playerPositionX < enemyPositionX){
						ForceMoveLeft();
					}
				}
			}
		}
	}

	private void ActivateAttack1(){
		if(isReadyToAttack1 && !aiHeroController.isAttacking && !aiHeroController.isHit){
			isReadyToAttack1 = false;
			float percent = Random.Range(0,1f);
			if(percent >= chanceToAttack1){
				Invoke("Attack1",attack1_Delay);
			}else{
				Invoke("RefreshAttack1",attack1_coolDown);
				ActivateAttack2();
			}
		}
	}
	
	private void Attack1(){
		if(!gameDataManager.player.IsDead && !aiHeroController.IsDead){
			aiHeroController.Attack(AttackType.Attack1);
		}
	}

	private void RefreshAttack1(){
		isReadyToAttack1 = true;
	}

	private void ActivateAttack2(){
		if(isReadyToAttack2 && !aiHeroController.isAttacking && !aiHeroController.isHit){
			isReadyToAttack2 = false;
			float percent = Random.Range(0,1f);
			if(percent > chanceToAttack2){
				Invoke("Attack2",attack2_Delay);
			}else{
				Invoke("RefreshAttack2",attack2_coolDown);
			}
		}
	}
	
	private void Attack2(){
		if(!gameDataManager.player.IsDead && !aiHeroController.IsDead){
			aiHeroController.Attack(AttackType.Attack2);
		}
	}
	
	private void RefreshAttack2(){
		isReadyToAttack2 = true;
	}

	private void ShowBossHp(){
		if(!gameDataManager.IsShowBossHP){
			soundManager.bgmAudioSource.Stop();
			soundManager.PlayBGM(BGM.LavaBossBGM);
			gameDataManager.IsShowBossHP =true;
		}
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
			Debug.Log("Lava boss ai controller attack player");
		}
	}
	
	public override void InstantDeath ()
	{
		base.InstantDeath ();
		//ActivateDeath();
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
