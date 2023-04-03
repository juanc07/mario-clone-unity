using UnityEngine;
using System.Collections;
using System;

public abstract class AIController : MonoBehaviour,IEventListener {

	public EnemyController aiHeroController{set;get;}
	private bool isTurning=false;
	//public HitController hitController;
	public GameObject hitCollider;
	public GameObject enemyPrefabModel;
	public GameObject enemyBlockerCollider;
	private CharacterController characterController;


	public float rangeToMove = 30f;
	public float distance{get;set;}
	public float distanceY{get;set;}

	public float disappearXRange= -74f;
	public float disappearYRange= 0;

	public float turnDelay = 1f;
	//public bool isMoveDelay =false;

	//private bool isStop = false;
	//public bool isWaiting = false;
	//private bool isPlayerDeads =false;

	private bool isFullStop =false;
	private bool isForceStop =false;
	public bool needsToWait = false;

	public bool isFollow = false;
	private bool isHovering = false;
	private float hoveringDelay = 0.5f;

	//new
	public bool isTrap = false;

	public float playerPositionX{set;get;}
	public float playerPositionY{set;get;}

	public float enemyPositionX{set;get;}
	public float enemyPositionY{set;get;}

	public float airOffsetY{set;get;}
	public float airOffsetX{set;get;}

	//public float airOffsetY =5f;
	//public float airOffsetX =10f;

	//tells who is this ai that is about to collide with another level object tagger
	public LevelObjectTagger levelObjectChecker{get;set;}


	public GameDataManager gameDataManager{set;get;}
	private LevelManager levelManager;
	public GameObject playerHero{set;get;}
	public HeroController playerHeroController{set;get;}
	public SoundManager soundManager{set;get;}
	public ParticleManager particleManager{set;get;}

	//new
	private SkinnedMeshRenderer skinnedMeshRenderer;
	private MeshRenderer meshRenderer;
	public Transform aiTransform{set;get;}

	private Transform playerHeroTransform;
	private Rigidbody body;
	private FixedPosition fixedPosition;
	// Use this for initialization
	public virtual void Start (){
		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();
		particleManager = ParticleManager.GetInstance();
		characterController = gameObject.GetComponent<CharacterController>();

		levelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
		//hitController = hitCollider.GetComponent<HitController>();
		aiHeroController = this.gameObject.GetComponent<EnemyController>();
		levelObjectChecker = this.gameObject.transform.GetComponentInChildren<LevelObjectTagger>();
		aiTransform = this.gameObject.transform;
		body = this.gameObject.GetComponent<Rigidbody>();
		fixedPosition = this.gameObject.GetComponent<FixedPosition>();
		//Debug.Log("who is this ai " +levelObjectChecker.levelTag);
		CacheMeshReference();
		InitPlayerReference();
		AddEventListener();


		if(isTrap){
			FullStop();
		}else{
			MoveLeft();
		}
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	public virtual void AddEventListener(){
		if(gameDataManager!=null){
			gameDataManager.player.OnPlayerDead+=OnPlayerDead;
			gameDataManager.OnLevelStart+=OnLevelStart;
			gameDataManager.OnGameRestart+=OnGameRestart;
		}

		if(aiHeroController!=null){
			aiHeroController.OnHeroHitLevelObject+=OnHeroHitLevelObject;
			aiHeroController.OnHeroDied += OnHeroDied;
		}
	}

	public virtual void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.player.OnPlayerDead-=OnPlayerDead;
			gameDataManager.OnLevelStart-=OnLevelStart;
			gameDataManager.OnGameRestart-=OnGameRestart;
		}

		if(aiHeroController!=null){
			aiHeroController.OnHeroHitLevelObject-=OnHeroHitLevelObject;
			aiHeroController.OnHeroDied -= OnHeroDied;
		}
	}

	private void InitPlayerReference(){
		if(levelManager.heroInstance!=null){
			playerHero = levelManager.heroInstance;
			playerHeroController = playerHero.gameObject.GetComponent<HeroController>();
			playerHeroTransform = playerHero.gameObject.transform;
		}
	}

	private void CacheMeshReference(){
		skinnedMeshRenderer = GetSkinnedMesh(this.gameObject);
		meshRenderer = GetdMesh(this.gameObject);
	}

	private SkinnedMeshRenderer GetSkinnedMesh(GameObject parent){
		int count = parent.gameObject.transform.childCount;
		SkinnedMeshRenderer tempSkinnedMeshRenderer = null;

		for(int index=0;index<count;index++){
			Transform child = parent.gameObject.transform.GetChild(index);
			tempSkinnedMeshRenderer = child.gameObject.GetComponent<SkinnedMeshRenderer>();
			if(tempSkinnedMeshRenderer!=null){
				//Debug.Log("found enemy mesh");
				break;
			}else{
				//Debug.Log("failed search inner");
				tempSkinnedMeshRenderer = GetSkinnedMesh(child.gameObject);
				if(tempSkinnedMeshRenderer!=null){
					break;
				}
			}
		}

		return tempSkinnedMeshRenderer;
	}

	private MeshRenderer GetdMesh(GameObject parent){
		int count = parent.gameObject.transform.childCount;
		MeshRenderer tempMeshRenderer = null;
		
		for(int index=0;index<count;index++){
			Transform child = parent.gameObject.transform.GetChild(index);
			tempMeshRenderer = child.gameObject.GetComponent<MeshRenderer>();
			if(tempMeshRenderer!=null){
				//Debug.Log("found enemy mesh");
				break;
			}else{
				//Debug.Log("failed search inner");
				tempMeshRenderer = GetdMesh(child.gameObject);
				if(tempMeshRenderer!=null){
					break;
				}
			}
		}
		
		return tempMeshRenderer;
	}

	public virtual void OnLevelStart(){
		InitPlayerReference();
		EnableColliderAndMesh();
		//TurnOnOffMesh(true);
		//ForceStartMoving();
		//Debug.Log("AiController level start");
	}

	public virtual void OnGameRestart(){
		InitPlayerReference();
		EnableColliderAndMesh();
		ForceStartMoving();
		//TurnOnOffMesh(true);
		//FullStop();
		//isFullStop = true;
		//Debug.Log("AiController level restart");
		//ForceStartMoving();
	}

	private void OnHeroHitLevelObject(LevelObjectTagger levelObjectTagger){
		//if(levelObjectTagger.levelTag!= LevelTag.Ground){
			//Debug.Log("ai hit levelObjectTagger.levelTag " + levelObjectTagger.levelTag);
		//}

		if(levelObjectTagger.levelTag == LevelTag.Hero || levelObjectTagger.levelTag == LevelTag.Mario){
			//Debug.Log("hit hero or mario? " + levelObjectTagger.levelTag );
			if(gameDataManager.player.IsInvulnerable){
				if(!aiHeroController.IsDead){
					InstantDeath();
				}
			}else{
				if(aiHeroController.isHitUpSide){
					if(playerHeroController.isFalling){
						if(!aiHeroController.IsDead && !playerHeroController.IsDead){
							//TakeDamage();
							HitByMario();
						}
					}
				}else{
					if(!playerHeroController.IsDead && !aiHeroController.IsDead){
						AttackPlayer();
					}
				}
			}
		}

		if(levelObjectTagger.levelTag == LevelTag.Crate){
			CrateColliderController crateColliderController = levelObjectTagger.gameObject.GetComponent<CrateColliderController>();
			if(crateColliderController!=null){
				if(crateColliderController.isThrown){
					crateColliderController.ExplodeCrate();
					if(!aiHeroController.IsDead){
						HitByWeapon(levelObjectTagger);
					}
				}else{
					if(!aiHeroController.IsDead){
						CheckWhereToGo();
					}
				}
			}else{
				if(!aiHeroController.IsDead){
					CheckWhereToGo();
				}
			}
		}else if(levelObjectTagger.levelTag == LevelTag.DeathGround){
			if(!aiHeroController.IsDead){
				InstantDeath();
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Brick){
			Brick brick = levelObjectTagger.gameObject.GetComponent<Brick>();
			if(brick.isMoving){
				if(!aiHeroController.IsDead){
					HitByMovingBrick();
				}
			}else{
				if(aiHeroController.isHitLeftSide || aiHeroController.isHitRightSide){
					if(!aiHeroController.IsDead){
						CheckWhereToGo();
					}
				}
			}
		}else if(levelObjectTagger.levelTag == LevelTag.UnBreakableBrick){
			HitUnBreakbleBrick();
			if(aiHeroController.isHitLeftSide || aiHeroController.isHitRightSide){
				if(!aiHeroController.IsDead){
					CheckWhereToGo();
				}
			}
		}


		if(levelObjectTagger.levelTag == LevelTag.CannonBullet){
			if(!aiHeroController.IsDead && !CheckIfHitSelf(levelObjectTagger)){
				HitByEnemy(levelObjectTagger);
				//InstantDeath();
				//Invoke("InstantDeath",0.3f);
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Enemy){
			if(!aiHeroController.IsDead && !CheckIfHitSelf(levelObjectTagger) ){
				HitByEnemy(levelObjectTagger);
				CheckWhereToGo();
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Boss){
			if(!aiHeroController.IsDead && !CheckIfHitSelf(levelObjectTagger) ){
				HitByEnemy(levelObjectTagger);
				CheckWhereToGo();
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Mushroom 
		         || levelObjectTagger.levelTag == LevelTag.MushroomLife 
		         ||	levelObjectTagger.levelTag == LevelTag.Star 
		         ||	levelObjectTagger.levelTag == LevelTag.Flower 
		       ){
			if(!aiHeroController.IsDead){
				HitItem();
				CheckWhereToGo();
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Entrance 
		         ||	levelObjectTagger.levelTag == LevelTag.Exit
		         ||	levelObjectTagger.levelTag == LevelTag.Well
		         ||	levelObjectTagger.levelTag == LevelTag.Crate
		         ||	levelObjectTagger.levelTag == LevelTag.MovingPlatform
		         ||	levelObjectTagger.levelTag == LevelTag.StaticPlatform
		         ||	levelObjectTagger.levelTag == LevelTag.FallingPlatform
		        ){
			if(!aiHeroController.IsDead){
				HitObstacle();
				CheckWhereToGo();
			}
		}else if(levelObjectTagger.levelTag == LevelTag.EnemyBlocker){
			if(!aiHeroController.IsDead){
				CheckWhereToGo();
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Ground){
			if(!aiHeroController.IsDead){
				HitGround();
				if(aiHeroController.isHitLeftSide || aiHeroController.isHitRightSide){
					//Debug.Log("enemy hit ground");
					CheckWhereToGo();
				}
			}
		}
	}

	private bool CheckIfHitSelf(LevelObjectTagger levelObjectTagger){
		bool hitSelf = false;
		EnemyController collidedController = levelObjectTagger.gameObject.GetComponent<EnemyController>();
		if(collidedController==null){
			collidedController = levelObjectTagger.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
		}
		
		if(collidedController!=null){
			if(collidedController.id.Equals(aiHeroController.id,StringComparison.Ordinal)){
				hitSelf =  true;
			}else{
				hitSelf = false;
			}
		}

		return hitSelf;
	}

	public virtual void HitGround(){

	}

	public virtual void HitUnBreakbleBrick(){
		
	}

	public virtual void HitObstacle(){
		
	}

	public virtual void HitItem(){
		
	}

	public virtual void TakeDamage(){

	}

	public virtual void HitByMario(){
		
	}

	public virtual void AttackPlayer(){

	}

	public virtual void InstantDeath(){

	}

	public virtual void HitByMovingBrick(){

	}

	public virtual void HitByEnemy(LevelObjectTagger levelObjectTagger){

	}

	public virtual void HitByWeapon(LevelObjectTagger levelObject){

	}

	public virtual void OnPlayerDead(){
		//gameDataManager.player.IsDead =true;
		//FullStop();
	}


	// Update is called once per frame
	public virtual void Update (){
		//enemyPositionX = this.gameObject.transform.position.x;
		//enemyPositionY = this.gameObject.transform.position.y;
		enemyPositionX = aiTransform.position.x;
		enemyPositionY = aiTransform.position.y;

		if(playerHero!=null){
			//playerPositionX = playerHero.gameObject.transform.position.x;
			//playerPositionY = playerHero.gameObject.transform.position.y;

			playerPositionX = playerHeroTransform.position.x;
			playerPositionY = playerHeroTransform.position.y;

			distance = playerPositionX - enemyPositionX;
			if(distance<=0){
				distance*=-1;
			}

			distanceY = playerPositionY - enemyPositionY;
			if(distanceY<=0){
				distanceY*=-1;
			}
		}

		CheckIfNeedsToWait();
		CheckForPosition();
		FollowPlayer();
	}

	private void CheckForPosition(){
		if(enemyPositionX < disappearXRange){
			aiHeroController.Kill();
		}
		
		if(enemyPositionY <= disappearYRange){
			aiHeroController.Kill();
		}
	}

	public void CheckIfNeedsToWait(){
		if(playerHero==null || gameDataManager.player.IsDead || isForceStop)return;

		if(needsToWait){			
			if(distance <= rangeToMove){
				//TurnOnOffMesh(true);
				if(!isTrap){
					StartMovingFromFullStop();
				}
			}else{
				FullStop();
				//TurnOnOffMesh(false);
			}
		}			
	}

	public virtual void Attack(){
		//Debug.Log("Ai Controller atack!!");
	}
	
	public void CheckWhereToGo(){
		//if(isPlayerDead || isWaiting || aiHeroController.isAttacking )return;
		//Debug.Log("check where to go isForceStop " + isForceStop);
		if(gameDataManager.player.IsDead || aiHeroController.isAttacking || isForceStop )return;
		if(aiHeroController.isFacingRight && !isTurning){
			//Debug.Log("check where to go to left");
			isTurning =true;
			aiHeroController.isWalking =true;
			MoveLeft();
		}else if(aiHeroController.isFacingLeft && !isTurning){
		//else if(aiHeroController.isFacingLeft && !isTurning && !playerHeroController.isIdle){
			//Debug.Log("check where to go to right");
			isTurning =true;
			aiHeroController.isWalking =true;
			MoveRight();
		}
	}

	private void RefreshDelay(){
		isTurning =false;
	}

	public void ForceMoveLeft(){
		aiHeroController.isWalking =true;
		MoveLeft();
	}

	public void ForceMoveRight(){
		aiHeroController.isWalking =true;
		MoveRight();
	}

	/*public virtual void StartMoving(){
		if(isWaiting){
			isWaiting =false;
			isStop =false;
			if(!isFollow){
				MoveLeft();
			}
		}
	}*/

	public virtual void ForceStartMoving(){
		isFullStop =false;
		isForceStop =false;
		if(aiHeroController.isFacingRight){
			aiHeroController.isWalking =true;
			MoveRight();
		}else if(aiHeroController.isFacingLeft){
			aiHeroController.isWalking =true;
			MoveLeft();
		}
	}

	public virtual void StartMovingFromFullStop(){
		if(isFullStop && !gameDataManager.player.IsDead){
			isFullStop =false;
			//isStop =false;
			//isWaiting = false;

			if(aiHeroController.isFacingRight){
				aiHeroController.isWalking =true;
				MoveRight();
			}else if(aiHeroController.isFacingLeft){
				aiHeroController.isWalking =true;
				MoveLeft();
			}
		}else{
			//Debug.Log("can't start moving from fullstop! isFullStop " + isFullStop + " isDead " + gameDataManager.player.IsDead);
		}
	}

	public virtual void StartMovingFromForceStop(){
		if(!gameDataManager.player.IsDead){
			isForceStop =false;
			if(aiHeroController.isFacingRight){
				aiHeroController.isWalking =true;
				MoveRight();
			}else if(aiHeroController.isFacingLeft){
				aiHeroController.isWalking =true;
				MoveLeft();
			}
		}
	}

	private void FollowPlayer(){
		if(isForceStop) return;
		if(distance <=rangeToMove){
			if(gameDataManager!=null){
				if(!gameDataManager.IsLevelComplete){
					//if(!isStop && isFollow && playerHero!= null){
					if(isFollow && playerHero!= null){
						//if(playerPositionX> (this.gameObject.transform.position.x + airOffsetX) ){
						if(playerPositionX> (aiTransform.position.x + airOffsetX) ){
							if(!isTurning && !aiHeroController.isFacingRight){
								isTurning =true;
								aiHeroController.isWalking =true;
								MoveRight();
								//Debug.Log("follow player move right");
							}
						}
						
						//if(playerPositionX < (this.gameObject.transform.position.x - airOffsetX)){
						if(playerPositionX < (aiTransform.position.x - airOffsetX)){
							if(!isTurning && !aiHeroController.isFacingLeft){
								isTurning =true;
								aiHeroController.isWalking =true;
								MoveLeft();
								//Debug.Log("follow player move left");
							}
						}

						//if(distanceY <= 15f){
							fixedPosition.originY = aiTransform.position.y;
							fixedPosition.isFixedY = false;

							if((playerPositionY + airOffsetY) > enemyPositionY ){
								AirMoveUp(airOffsetY);
							}
							
							if((playerPositionY + airOffsetY) < enemyPositionY){
								AirMoveDown(airOffsetY);
							}
						//}else{
							//fixedPosition.originY = aiTransform.position.y;
							//fixedPosition.isFixedY = true;
						//}
					}
				}
			}
		}
	}

	public void AirMoveUp(float val){
		if(!isHovering){
			isHovering = true;
			aiHeroController.AirMoveUp(val);
			//Invoke("RefreshHovering",hoveringDelay);
			Invoke(Task.RefreshHovering.ToString(),hoveringDelay);
		}
	}

	public void AirMoveDown(float val){
		if(!isHovering){
			isHovering = true;
			aiHeroController.AirMoveDown(val);
			//Invoke("RefreshHovering",hoveringDelay);
			Invoke(Task.RefreshHovering.ToString(),hoveringDelay);
		}
	}

	private void RefreshHovering(){
		isHovering = false;
	}

	public void MoveLeft(){
		//Debug.Log("Move left!!");
		if(aiHeroController==null) return;

		//isStop =false;
		aiHeroController.speed = 0;
		aiHeroController.isRightBtnPress =false;
		aiHeroController.isLeftBtnPress =true;	
		aiHeroController.isHitLeftSide = false;
		aiHeroController.isHitRightSide = false;
		//Invoke("RefreshDelay", turnDelay);
		Invoke(Task.RefreshDelay.ToString(), turnDelay);
	}

	public void MoveRight(){
		if(aiHeroController==null) return;

		//isStop =false;
		aiHeroController.speed = 0;
		aiHeroController.isRightBtnPress =true;
		aiHeroController.isLeftBtnPress =false;
		aiHeroController.isHitLeftSide =false;
		aiHeroController.isHitRightSide =false;
		//Invoke("RefreshDelay", turnDelay);
		Invoke(Task.RefreshDelay.ToString(), turnDelay);
	}

	/*private void StopMoving(){
		if(!isStop){
			isStop =true;
			aiHeroController.StopMoving();
			Invoke(Task.CheckWhereToGo.ToString(), moveDelay);
		}
	}*/

	public virtual void FullStop(){
		if(!isFullStop){
			isFullStop =true;
			aiHeroController.StopMoving();
			//Debug.Log("fullstop!");
		}
	}

	public void ForceStop(){
		//if(!isForceStop){
			isForceStop =true;
			aiHeroController.StopMoving();
		//}
	}

	public void EnableColliderAndMesh(){
		//if(!aiHeroController.IsDead){
			characterController.enabled = true;
			aiHeroController.isDisableMovement = false;
			enemyPrefabModel.gameObject.SetActive(true);
			hitCollider.gameObject.SetActive(true);
			enemyBlockerCollider.gameObject.SetActive(true);
		//}
	}

	//for optimzation and for enemy that when died creats a obstacle that the play may stacked
	private void DisableColliderAndMesh(){
		characterController.enabled = false;
		aiHeroController.isDisableMovement = true;
		hitCollider.gameObject.SetActive(false);
		enemyBlockerCollider.gameObject.SetActive(false);
	}


	private void TurnOnOffMesh(bool val){
		if(skinnedMeshRenderer!=null){
			skinnedMeshRenderer.enabled = val;
		}

		if(meshRenderer!=null){
			meshRenderer.enabled = val;
		}
	}

	private void OnHeroDied(){
		DisableColliderAndMesh();
	}
}
