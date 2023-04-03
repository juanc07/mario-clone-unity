using UnityEngine;
using System.Collections;
using System;


public class FireballController : Projectile {
	//properties
	private float constantDownForce = -8f;

	//private float forwardForce = 900f;
	public float forwardForce{set;get;}

	private float upForce;
	private float hitForwardForce;
	private Rigidbody rigidBody;

	public HeroController heroController{set;get;}
	private LevelObjectTagger fireballLevelObjectTagger;

	private int hitCounter;
	private int maxHit = 1;
	private int direction;

	public string ownerId{set;get;}
	private string hitId;
	private string hitEnemyId;
	//methods

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		fireballLevelObjectTagger = this.gameObject.GetComponent<LevelObjectTagger>();
		GetRigidBody();
		ResetData();
	}

	// Update is called once per frame
	public override void Update ()
	{
		base.Update ();
		rigidBody.AddForce(new Vector3(0,constantDownForce,0),ForceMode.Acceleration);
	}

	public void ResetData(){
		rigidBody.velocity = Vector3.zero;
		hitForwardForce = 300f;
		upForce = 8f;
		hitCounter = 0;
	}

	private void GetRigidBody(){
		if(rigidBody==null){
			rigidBody = this.gameObject.GetComponent<Rigidbody>();
		}
	}

	public void Shoot(){
		GetRigidBody();
		if(heroController.isFacingRight){
			direction = 0;
			rigidBody.AddForce(new Vector3(forwardForce,0,0),ForceMode.Acceleration);
			//Debug.Log("fireball controller shoot right");
		}else{
			direction = 1;
			rigidBody.AddForce(new Vector3(-forwardForce,0,0),ForceMode.Acceleration);
			//Debug.Log("fireball controller shoot left");
		}
	}

	// events
	private void OnCollisionEnter(Collision collision){
		LevelObjectTagger levelObjecttagger = collision.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjecttagger==null){
			levelObjecttagger = collision.gameObject.transform.parent.gameObject.GetComponent<LevelObjectTagger>();
		}

		if(levelObjecttagger!=null){
			if(levelObjecttagger.levelTag == LevelTag.Hero 
			   || levelObjecttagger.levelTag == LevelTag.Mario 
			   ){
				//Debug.Log("fireball hit mario!");
				HeroController heroController =  levelObjecttagger.gameObject.GetComponent<HeroController>();
				MarioController marioController =  levelObjecttagger.gameObject.GetComponent<MarioController>();
				if(heroController!=null){
					hitId = heroController.id;
					if(!ownerId.Equals(hitId,StringComparison.Ordinal)){
						if(marioController!=null){
							marioController.HitByWeapon(fireballLevelObjectTagger);
							IsActive = false;
						}	
					}
				}
				//Debug.Log("fireball hit " + levelObjecttagger.levelTag );
			}else if(levelObjecttagger.levelTag == LevelTag.Boss && IsActive ){
				AIController aiController = levelObjecttagger.gameObject.GetComponent<AIController>();
				if(aiController!=null){
					aiController.HitByWeapon(fireballLevelObjectTagger);
					IsActive = false;
				}
			}else if(levelObjecttagger.levelTag == LevelTag.Enemy && IsActive ){
				AIController aiController = levelObjecttagger.gameObject.GetComponent<AIController>();
				EnemyController enemyController = levelObjecttagger.gameObject.GetComponent<EnemyController>();
				if(aiController!=null){
					if(enemyController!=null){
						if(enemyController.enemyType != EnemyType.FallingEnemy){
							if(enemyController.enemyType == EnemyType.FireMonster){
								hitEnemyId = enemyController.id;
								//Debug.Log("fireball hit firemonster check id hitid "+ hitEnemyId + " ownerId " + ownerId);
								if(!ownerId.Equals(hitEnemyId,StringComparison.Ordinal)){
									if(aiController!=null){
										aiController.HitByWeapon(fireballLevelObjectTagger);
										IsActive = false;
									}	
								}
							}else{
								aiController.HitByWeapon(fireballLevelObjectTagger);
								IsActive = false;
							}
						}else{
							IsActive = false;
						}
					}
				}
			}else if(levelObjecttagger.levelTag == LevelTag.CannonBullet && IsActive ){
				AIController aiController = levelObjecttagger.gameObject.GetComponent<AIController>();
				if(aiController!=null){
					aiController.HitByWeapon(fireballLevelObjectTagger);
					IsActive = false;
				}
			}else if(levelObjecttagger.levelTag == LevelTag.Projectile && IsActive ){
				IsActive = false;
			}else{
				if(hitCounter <= maxHit){
					hitCounter++;
					rigidBody.AddForce(new Vector3(0,upForce,0),ForceMode.VelocityChange);
					if(direction==0){
						rigidBody.AddForce(new Vector3(hitForwardForce,0,0),ForceMode.Acceleration);
					}else{
						rigidBody.AddForce(new Vector3(-hitForwardForce,0,0),ForceMode.Acceleration);
					}

					if(upForce>0){
						upForce-=2f;
					}else{
						upForce=0;
					}

					if(hitForwardForce>0){
						hitForwardForce-=150f;
					}else{
						hitForwardForce=0;
					}
				}else{
					if(IsActive){
						IsActive = false;
					}
				}
			}					
		}
	}


}
