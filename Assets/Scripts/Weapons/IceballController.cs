using UnityEngine;
using System.Collections;
using System;

public class IceballController : Projectile {
	//properties
	//public float constantDownForce{set;get;}
	private float constantDownForce = -8f;
	
	//private float forwardForce = 900f;
	public float forwardForce{set;get;}

	private float upForce;
	private float hitForwardForce;
	private Rigidbody rigidBody;
	
	public HeroController heroController{set;get;}
	private LevelObjectTagger levelObjectTagger;
	
	private int hitCounter;
	private int maxHit = 3;
	private int direction;

	public string ownerId{set;get;}
	private float killDelay=6f;

	//methods	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		levelObjectTagger = this.gameObject.GetComponent<LevelObjectTagger>();
		GetRigidBody();
		ResetData();
	}

	private void OnDestroy(){
		//CancelInvoke("KillIceBall");
		CancelInvoke(Task.KillIceBall.ToString());
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
		upForce = 10f;
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
		}else{
			direction = 1;
			rigidBody.AddForce(new Vector3(-forwardForce,0,0),ForceMode.Acceleration);
		}
		if(IsInvoking("KillIceBall")){
			//CancelInvoke("KillIceBall");
			CancelInvoke(Task.KillIceBall.ToString());
		}else{
			//Invoke("KillIceBall",killDelay);
			Invoke(Task.KillIceBall.ToString(),killDelay);
		} 
	}

	private void KillIceBall(){
		if(IsActive){
			IsActive = false;
		}
	}

	private void SummonLakituThrow(){

	}


	// events
	private void OnCollisionEnter(Collision collision){
		LevelObjectTagger levelObjecttagger = collision.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjecttagger==null){
			levelObjecttagger = collision.gameObject.transform.parent.gameObject.GetComponent<LevelObjectTagger>();
		}
		
		if(levelObjecttagger!=null){
			if(levelObjecttagger.levelTag == LevelTag.Hero || levelObjecttagger.levelTag == LevelTag.Mario){
				//Debug.Log("Iceball hit " + levelObjecttagger.levelTag + " IsActive " + IsActive);
				if(IsActive){
					//Debug.Log("Iceball hit mario2");
					MarioController marioController = levelObjecttagger.gameObject.GetComponent<MarioController>();
					if(marioController!=null){
						if(!gameDataManager.IsInvulnerable){
							marioController.HitByWeapon(levelObjectTagger);
						}
						KillIceBall();
					}
				}
			}else if(levelObjecttagger.levelTag == LevelTag.Boss && IsActive ){
				EnemyController enemyController = levelObjecttagger.gameObject.GetComponent<EnemyController>();
				if(enemyController.enemyType == EnemyType.BigMushroom){
					AIController aiController = levelObjecttagger.gameObject.GetComponent<AIController>();
					if(aiController!=null){
						aiController.HitByWeapon(levelObjectTagger);
						KillIceBall();
					}
				}

			}else if(levelObjecttagger.levelTag == LevelTag.Enemy && IsActive ){
				AIController aiController = levelObjecttagger.gameObject.GetComponent<AIController>();
				EnemyController enemyController = levelObjecttagger.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(enemyController.enemyType == EnemyType.Lakitu){
						string hitEnemyId = enemyController.id;
						if(!ownerId.Equals(hitEnemyId,StringComparison.Ordinal)){
							if(aiController!=null){
								aiController.HitByWeapon(levelObjectTagger);
								KillIceBall();
							}	
						}
					}else{
						KillIceBall();
					}
				}
			}else if(levelObjecttagger.levelTag == LevelTag.CannonBullet && IsActive ){
				AIController aiController = levelObjecttagger.gameObject.GetComponent<AIController>();
				if(aiController!=null){
					aiController.HitByWeapon(levelObjectTagger);
					KillIceBall();
				}
			}else if(levelObjecttagger.levelTag == LevelTag.Projectile && IsActive ){
				KillIceBall();
			}else if(levelObjecttagger.levelTag == LevelTag.MovingPlatform && IsActive ){
				KillIceBall();
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
					KillIceBall();
					/*if(IsActive){
						IsActive = false;
					}*/
				}
			}					
		}
	}
	
	
}
