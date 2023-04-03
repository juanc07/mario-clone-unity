using UnityEngine;
using System.Collections;
using System;

public class EnemyController : HeroController,IEventListener{

	public float hp;
	public float originalHp{set;get;}
	public EnemyType enemyType = EnemyType.Goomba;

	public float deathParticleDelay = 1f;
	public BoxCollider hitCollider;

	public GameDataManager gameDataManager{set;get;}
	public ParticleManager particleManager{set;get;}
	public SoundManager soundManager{set;get;}

	private HeroController eController;
	private CharacterController charController;
	private bool hasListener = false;

	public override void Start(){
		base.Start();
		gameDataManager = GameDataManager.GetInstance();
		particleManager =  ParticleManager.GetInstance();
		soundManager = SoundManager.GetInstance();

		isDestroyBrick =false;
		GetHP();
		//originalHp = hp;
		CacheControllers();
		AddEventListener();
		//Debug.Log("enemy controller start!");
	}

	public virtual void AddEventListener(){
		if(gameDataManager!=null){
			if(!hasListener){
				hasListener = true;
				gameDataManager.OnGameRestart+= OnGameRestart;
				gameDataManager.OnLevelStart+= OnLevelStart;
				gameDataManager.player.OnPlayerDead += OnPlayerDead;
				
				this.OnHeroHit += OnEnemyHit;
				this.OnHeroDied += OnEnemyDied;
				//Debug.Log("add event listener enemy controller");
			}
		}
	}
	
	public virtual void RemoveEventListener(){
		if(gameDataManager!=null){
			if(hasListener){
				gameDataManager.OnGameRestart-= OnGameRestart;
				gameDataManager.OnLevelStart-= OnLevelStart;
				gameDataManager.player.OnPlayerDead -= OnPlayerDead;

				this.OnHeroHit -= OnEnemyHit;
				this.OnHeroDied -= OnEnemyDied;
				//Debug.Log("remove event listener enemy controller");
			}
		}
	}
	
	private void OnDestroy(){
		RemoveEventListener();
		//Debug.Log("on destory hammer bro controller");
	}
	
	public virtual void OnGameRestart(){
		CacheControllers();
		ResetData();
	}

	public virtual void OnLevelStart(){
		CacheControllers();
		ResetData();
	}

	private void CacheControllers(){
		if(this!=null){
			if(eController==null){
				eController = this.gameObject.GetComponent<HeroController>();
			}
			
			if(charController==null){
				charController = this.gameObject.GetComponent<CharacterController>();
			}
		}
	}

	public void ResetData(){
		EnableDisableBody(true);
		IsDead = false;
		//hp = originalHp;
		GetHP();
		//isIdle = true;
		//MoveLeft();
		//StartMoving();
	}

	private void OnPlayerDead(){
		StopMoving();
	}


	public virtual void OnEnemyHit(){
		//Debug.Log("b4 on Enemy hit hp " + hp);
		if(hp> 0){
			hp--;
			if(hp<=0){
				if(!IsDead){
					IsDead =true;
				}
			}
		}
		//Debug.Log("after on Enemy hit hp " + hp);
	}


	public override void Update ()
	{
		base.Update ();
		if(this.gameObject.transform.position.y <= 0){
			Kill();
		}
	}

	public void KillMe(){
		Kill();
	}

	public virtual void OnEnemyDied(){
		//Debug.Log(" OnEnemyDied ");
		if(IsDead){
			//Invoke("ShowParticleDeath", deathParticleDelay);
			Invoke(Task.ShowDeathParticle.ToString(), deathParticleDelay);
		}
	}

	public void EnableDisableBody(bool val){
		if(this!=null){
			if(this.gameObject !=null ){
				if(eController!=null){
					eController.enabled =val;
				}else{
					CacheControllers();
					Debug.Log("hero controller not found");
				}

				if(charController!=null ){
					charController.enabled =val;
				}else{
					CacheControllers();
					Debug.Log("character controller not found");
				}

				hitCollider.enabled = val;
			}
		}
	}

	//overide this and show particle when enemy is hit by doing this you can show different particles for different enemies when hit
	//for optimization too

	public virtual void ShowDeathParticle(){
		EnableDisableBody(false);
	}

	private void GetHP(){
		int currentHp;
		switch(enemyType){
			case EnemyType.Goomba:
				currentHp = HPValue.GOOMBA;
			break;
			case EnemyType.HammerBro:
				currentHp = HPValue.HAMMER_BRO;
			break;
			case EnemyType.CannonBullet:
				currentHp = HPValue.CANNON_BULLET;
			break;
			case EnemyType.PlantEnemy:
				currentHp = HPValue.PLANT_ENEMY;
			break;
			case EnemyType.BigMushroom:
				currentHp = HPValue.GRASS_BOSS;
			break;
			case EnemyType.IceShock:
				currentHp = HPValue.ICE_SHOCK;
			break;
			case EnemyType.Snowman:
				currentHp = HPValue.SNOW_MAN;
			break;
			case EnemyType.Lakitu:
				currentHp = HPValue.LAKITU;
			break;
			case EnemyType.LakituThrow:
				currentHp = HPValue.LAKITU_THROW;
			break;
			case EnemyType.FallingEnemy:
				currentHp = HPValue.FALLING_ENEMY;
			break;
			case EnemyType.IceBoss:
				currentHp = HPValue.ICE_BOSS;
			break;
			case EnemyType.FireFly:
				currentHp = HPValue.FIRE_FLY;
			break;
			case EnemyType.FireMonster:
				currentHp = HPValue.FIRE_MONSTER;
			break;
			case EnemyType.Boo:
				currentHp = HPValue.BOO;
			break;

			case EnemyType.LavaBoss:
				currentHp = HPValue.LAVA_BOSS;
			break;

			default:
				currentHp = 1;
			break;
		}

		hp = currentHp;
		originalHp = currentHp;
	}
}