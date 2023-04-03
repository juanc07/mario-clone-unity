using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarioController : HeroController{

	private GameDataManager gameDataManager;
	private SoundManager soundManager;
	private ParticleManager particleManager;

	private Vector3 initialPosition;
	private GameTimer gameTimer;

	public float fireWeaponDelay = 0.5f;
	private bool isReadyToFire = true;

	private ProjectileManager projectileManager;

	private const float Normal_Friction = 30f;
	private const float ICE_Friction = 5f;
	private const float MOVING_PLATFORM_FRICTION = 70f;

	private Transform marioHolder;
	private bool isRaparent = false;

	//new 
	private LevelObjectTagger currentLevelObject = null;
	public HealthProperties healthProperties;

	private Transform marioTransform;

	public override void Start(){
		base.Start();
		projectileManager = GameObject.FindObjectOfType(typeof(ProjectileManager)) as ProjectileManager;
		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();
		particleManager = ParticleManager.GetInstance();

		gameTimer =  GameObject.FindObjectOfType(typeof(GameTimer)) as GameTimer;
		gameDataManager.player.HP = healthProperties.hp;

		marioHolder = this.gameObject.transform.parent;
		marioTransform = this.gameObject.transform;

		SetInitialPosition();
		AddListener();
	}

	private void OnDestroy(){
		RemoveListener();
	}
	
	private void AddListener(){
		gameDataManager.player.OnPlayerRevive+=OnPlayerRevive;
		gameTimer.OnTimeOut+=OnTimeOut;
		this.OnHeroJump+=OnMarioJump;
		this.OnHeroHit+=OnMarioHit;
		this.OnHeroDied += OnPlayerDied;
		this.OnHeroHitLevelObject+=OnMarioHitLevelObject;
		gameDataManager.player.OnPlayerInvulnerableChange+=OnPlayerInvulnerableChange;
		this.OnHeroFireWeapon+=OnMarioFireWeapon;
		this.OnHeroMoveDown += OnMarioMoveDown;
	}
	
	private void RemoveListener(){
		gameDataManager.player.OnPlayerRevive-=OnPlayerRevive;
		gameTimer.OnTimeOut-=OnTimeOut;
		this.OnHeroJump-=OnMarioJump;
		this.OnHeroHit-=OnMarioHit;
		this.OnHeroDied -= OnPlayerDied;
		this.OnHeroHitLevelObject-=OnMarioHitLevelObject;
		gameDataManager.player.OnPlayerInvulnerableChange-=OnPlayerInvulnerableChange;
		this.OnHeroFireWeapon-=OnMarioFireWeapon;
		this.OnHeroMoveDown -= OnMarioMoveDown;
	}

	private void SetInitialPosition(){
		LevelObjectTagger[] levelObjectTaggers = GameObject.FindObjectsOfType(typeof(LevelObjectTagger)) as LevelObjectTagger[];
		foreach(LevelObjectTagger levelTagger in levelObjectTaggers){
			if(levelTagger.levelTag == LevelTag.Entrance){
				marioTransform.position = levelTagger.gameObject.transform.position;
				initialPosition = marioTransform.position;
			}
		}
	}

	private void OnPlayerDied(){
		gameDataManager.player.IsInvulnerable = false;
		gameDataManager.player.HP = 0;
		gameDataManager.player.IsDead =true;
		gameDataManager.Life--;
	}

	private void OnMarioMoveDown(){
		if(currentLevelObject!=null){
			if(currentLevelObject.levelTag == LevelTag.StaticPlatform){
				StaticPlatformController staticPlatformController = levelObjectTagger.gameObject.GetComponent<StaticPlatformController>();
				if(staticPlatformController!=null){
					staticPlatformController.boxCollider.isTrigger = true;
				}
			}
		}
	}

	private void OnMarioJump(){
		if(isRaparent){
			isRaparent = false;
			marioTransform.parent = marioHolder;
		}
		friction = Normal_Friction;
		soundManager.PlaySfx(SFX.JumpSfx);
	}

	private void OnMarioHit(){
		if(gameDataManager.player.IsInvulnerable && !gameDataManager.IsLevelComplete) return;
		if(gameDataManager.player.HP > 0){
			gameDataManager.player.HP--;
			if(healthProperties.hp<=0){
				healthProperties.hp = 0; 
			}else{
				healthProperties.hp = gameDataManager.player.HP;
			}
			
			if(gameDataManager.player.HP<=0 && !gameDataManager.player.IsDead && !IsDead){
				if(!IsDead){
					KillMario();
				}
			}
			//Debug.Log("overried hit");
		}


	  	Vector3 newPosition =	marioTransform.position;
		newPosition.y += 2f;
		Vector3 scale = new Vector3(2f,2f,2f);
		particleManager.CreateParticle(ParticleEffect.Hit1,newPosition,scale);
		soundManager.PlaySfx(SFX.Hit1Sfx);
	}

	public void KillMario(){
		if(gameDataManager.player.IsInvulnerable ) return;
		Kill();
	}

	private void OnTimeOut(){
		if(!gameDataManager.player.IsDead && !IsDead){
			KillMario();
			//IsDead =true;
			//gameDataManager.player.IsDead = true;
		}
	}

	private void OnPlayerRevive(){
		ResetState();
		ApplyGravity =true;

		//soon you will have a checkpoint 
		//find nearest checkpoint and get that check point position and use that instead
		marioTransform.position = initialPosition;
	}

	private void OnMarioFireWeapon(){
		if(isReadyToFire){
			isReadyToFire = false;
			//Invoke("FireWeaponTimer", fireWeaponDelay);
			Invoke(Task.FireWeaponTimer.ToString(), fireWeaponDelay);
			SummonFireWeapon();
		}
	}

	private void FireWeaponTimer(){
		isReadyToFire = true;
	}

	private void SummonFireWeapon(){
		Vector3 tempPosition;
		if(isFacingRight){
			tempPosition =marioTransform.position;
			tempPosition.x += 0.5f;
		}else{
			tempPosition =marioTransform.position;
			tempPosition.x -= 0.5f;
		}
		projectileManager.CreateFireBallProjectile(ProjectileType.Fireball,tempPosition,marioTransform.rotation,this,900f,id);
		soundManager.PlaySfx(SFX.Fireball);
	}

	public override void Update ()
	{
		base.Update ();
		//if(this.gameObject.transform.position.y <= 0){
		if(marioTransform.position.y <= 0){
			if(!IsDead){
				Kill();
			}
		}
	}

	private void ShowStarParticle(){
		Vector3 newPosition =	marioTransform.position;
		Vector3 scale = new Vector3(10f,10f,10f);
		particleManager.CreateParticle(ParticleEffect.CollectCoin,newPosition,scale);
		soundManager.PlaySfx(SFX.StarPower,0.75f);
		//Invoke("ShowStarParticle",0.5f);
		Invoke(Task.ShowStarParticle.ToString(),0.5f);
	}

	public void TakeDamage(){
		bool success = Hit();
		if(success){
			ShowHitParticle();
		}
	}

	private void OnPlayerInvulnerableChange(bool val){
		if(val){
			ShowStarParticle();
			//Debug.Log("mario invulnerable " + val);
		}else{
			//CancelInvoke("ShowStarParticle");
			CancelInvoke(Task.ShowStarParticle.ToString());
			//Debug.Log("mario invulnerable " + val);
		}
	}

	private void ShowHitParticle(){
		soundManager.PlaySfx(SFX.StepOnEnemy);		
		Vector3 newPosition =	marioTransform.position;
		newPosition.y += 2f;
		Vector3 scale = new Vector3(2f,2f,2f);
		particleManager.CreateParticle(ParticleEffect.Hit1,newPosition,scale);
	}

	private void ShowExplosionParticle(){
		Vector3 newPosition =	marioTransform.position;
		Vector3 scale = new Vector3(7f,7f,7f);
		particleManager.CreateParticle(ParticleEffect.Explosion1,newPosition,scale);
		soundManager.PlaySfx(SFX.SmallExplosion,1f);
	}

	private void OnMarioHitLevelObject(LevelObjectTagger levelObjecttagger){
		//Debug.Log("Mario hit level object: " + levelObjecttagger.levelTag);
		if(levelObjectTagger==null)return;
		currentLevelObject = levelObjectTagger;

		if(levelObjectTagger.levelTag == LevelTag.MovingPlatform){
			MovingPlatformController movingPlatformController = levelObjectTagger.gameObject.GetComponent<MovingPlatformController>();
			if(movingPlatformController!=null){
				if(!isRaparent){
					isRaparent = true;
					marioTransform.parent =  levelObjectTagger.gameObject.transform;
					friction = MOVING_PLATFORM_FRICTION;
					StopMoving();
				}
			}
		}else{
			if(isRaparent){
				isRaparent = false;
				marioTransform.parent = marioHolder;
			}
		}

		if(levelObjectTagger.levelTag == LevelTag.Brick || levelObjectTagger.levelTag == LevelTag.ItemBrick){
			//Debug.Log("mario hit brick");
			if(isHitDownSide){
				friction = Normal_Friction;
			}
			if(isHitUpSide && isInAir){
				Brick brick = levelObjectTagger.gameObject.GetComponent<Brick>();
				if(brick!=null){
					if(brick.isDestroyable){
						if( isDestroyBrick){
							brick.DestroyBlock();
						}
					}else{
						brick.AnimateBrick();
					}
					BrickRebound();
				}
			}
		}else if(levelObjectTagger.levelTag == LevelTag.UnBreakableBrick){
			friction = Normal_Friction;
			if(isHitUpSide && isInAir){
				BrickRebound();
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Ground){
			GroundSizeController groundSizeController = levelObjectTagger.gameObject.GetComponent<GroundSizeController>();
			if(groundSizeController!=null){
				if(groundSizeController.groundType == GroundType.Grass){
					friction = Normal_Friction;
				}else if(groundSizeController.groundType == GroundType.Ice){
					friction = ICE_Friction;
				}
			}
		}

		if(levelObjectTagger.levelTag == LevelTag.FallingPlatform){
			FallingPlatformController fallingPlatformController = levelObjectTagger.gameObject.GetComponent<FallingPlatformController>();
			if(fallingPlatformController!=null){
				friction = Normal_Friction;
				fallingPlatformController.ActivateFall();
			}
		}

		if(levelObjectTagger.levelTag == LevelTag.StaticPlatform){
			StaticPlatformController staticPlatformController = levelObjectTagger.gameObject.GetComponent<StaticPlatformController>();
			if(staticPlatformController!=null){
				if(isHitUpSide){
					staticPlatformController.boxCollider.isTrigger = true;
				}else{
					staticPlatformController.boxCollider.isTrigger = false;
				}
			}
		}



		if(levelObjectTagger.levelTag == LevelTag.Enemy){
			EnemyController enemyController = levelObjectTagger.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
			AIController aiController = levelObjectTagger.gameObject.transform.parent.gameObject.GetComponent<AIController>();
			if(gameDataManager.player.IsInvulnerable){
				if(enemyController!=null){
					if(!enemyController.IsDead){
						ShowHitParticle();
						aiController.InstantDeath();
					}
				}
			}else{
				if(enemyController!=null){
					if(isHitLeftSide || isHitRightSide || isHitUpSide && !enemyController.IsDead && !IsDead){
						if(enemyController.enemyType == EnemyType.FallingEnemy){
							KillMario();
						}else{
							bool success = Hit();
							if(success){
								ShowHitParticle();
							}
						}
					}
				}
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Boss){
			EnemyController enemyController = levelObjectTagger.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
			AIController aiController = levelObjectTagger.gameObject.transform.parent.gameObject.GetComponent<AIController>();
			if(gameDataManager.player.IsInvulnerable){
				if(!enemyController.IsDead){
					aiController.TakeDamage();
				}
			}else{
				if(isHitLeftSide || isHitRightSide || isHitUpSide && !enemyController.IsDead && !IsDead){
					bool success = Hit();
					if(success){
						ShowHitParticle();
					}
				}
			}
		}else if(levelObjectTagger.levelTag == LevelTag.CannonBullet){
			AIController aiController = levelObjectTagger.gameObject.transform.parent.gameObject.GetComponent<AIController>();
			EnemyController enemyController = levelObjectTagger.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
			if(gameDataManager.player.IsInvulnerable){
				if(!enemyController.IsDead){
					ShowHitParticle();
					aiController.InstantDeath();
				}
			}else{
				if(isHitLeftSide || isHitRightSide || isHitUpSide){
					if(!IsDead  && !enemyController.IsDead){
						ShowExplosionParticle();
						KillMario();
					}
				}
			}
		}

		if(levelObjectTagger.levelTag == LevelTag.Mushroom || levelObjectTagger.levelTag == LevelTag.MushroomLife){
			MushroomController mushroomController = levelObjectTagger.gameObject.transform.parent.GetComponent<MushroomController>();
			if(mushroomController!=null){
				mushroomController.CollectMushroom();
				//Debug.Log("Mario hit level object: " + levelObjectTagger.levelTag);
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Star){
			StarController starController = levelObjectTagger.gameObject.transform.parent.GetComponent<StarController>();
			if(starController!=null){
				starController.CollectStar();
			}
		}else if(levelObjectTagger.levelTag == LevelTag.Flower){
			FlowerController flowerController = levelObjectTagger.gameObject.transform.parent.GetComponent<FlowerController>();
			if(flowerController!=null){
				flowerController.CollectFlower();
			}
		}
	}

	public void HitByWeapon (LevelObjectTagger levelObject){
		if( levelObject.levelTag == LevelTag.Projectile ){
			TakeDamage();
		}
	}
}
