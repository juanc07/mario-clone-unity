using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FixedPosition))]
public class HeroController : MonoBehaviour,IControllable {
	//dont modify this values
	public float speed = 0;
	public float speedY=0;
	public float jumpDistance=0;
	//dont modify this values

	public float jumpSpeed = 0.7f;
	public float jumpHeight = 4f;


	public float fallMaxSpeed = 18f;
	public float jumpMaxSpeed = 14f;
	public float runMaxSpeed = 10f;
	public float walkMaxSpeed = 5f;
	public float airMaxSpeed = 20f;

	public float acceleration = 5f;
	public float friction = 30f;
	public float airAcceleration = 3f;
	public float decelerationTime = 1f;
	public float gravity = 20f;

	public float hitDelay=1f;

	public float brickReboundForce =1f;
	public float wallSlideSpeed =3f;
	public float wallJumpPower =800f;
	public float maxWallJumpSpeed =20f;
	public float wallSlideSmoothing = 20.0F;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	public bool isDestroyBrick=false;
	public bool isLeftBtnPress{set;get;}
	public bool isRightBtnPress{set;get;}
	private bool isDownBtnPress;

	//actions
	public bool isIdle{set;get;}
	public bool isWalking{set;get;}
	public bool isRunning{set;get;}
	public bool isHitWall{set;get;}
	public bool isMovingLeft{set;get;}
	public bool isMovingRight{set;get;}
	public bool isFacingRight{set;get;}
	public bool isFacingLeft{set;get;}
	public bool isLookingUp{set;get;}
	public bool isLookingDown{set;get;}
	public bool isInAir{set;get;}
	//public bool isFalling{set;get;}
	public bool isFalling;
	public bool isHoldingAction{set;get;}
	public bool isHoldingSomething{set;get;}
	public bool isThrow{set;get;}
	public bool isJumping{set;get;}
	public bool isBouncing{set;get;}
	public bool isDoubleJumping{set;get;}
	public bool isHit{set;get;}
	public bool isDisableMovement{set;get;}

	public bool isAttacking{set;get;}
	public AttackType currentAttackType{set;get;}
	public float attackDelay =1f;

	private bool isDead;
	public bool IsDead{
		set{ isDead = value;
			if(isDead){
				if(null!=HeroDied){
					CancelInvoke(Task.RefreshMidAttack.ToString());
					CancelInvoke(Task.RefreshAttack.ToString());
					HeroDied();
				}
			}else{
				if(null!=HeroRevive){
					HeroRevive();
				}
			}
		}
		get{ return isDead;}
	}

	//skills
	public bool hasDoubleJump=false;
	public bool canMoveInAir=true;

	//events
	private Action HitComplete;
	public event Action OnHitComplete{
		add{HitComplete+=value;}
		remove{HitComplete-=value;}
	}

	private Action MidHit;
	public event Action OnMidHit{
		add{MidHit+=value;}
		remove{MidHit-=value;}
	}


	private bool hitLeftSide;
	private bool hitRightSide;
	private bool hitUpSide;
	private bool hitDownSide;

	public bool ApplyGravity=true;
	public bool alwaysRun;
	public bool isConstantSpeed=false;

	public string id;

	private float oldPositionY;
	private float newPositionY;

	public LevelObjectTagger levelObjectTagger{set;get;}

	//new events
	private Action HeroMoveDown;
	public event Action OnHeroMoveDown{
		add{ HeroMoveDown+=value;}
		remove{ HeroMoveDown-=value;}
	}

	private Action HeroMoveRight;
	public event Action OnHeroMoveRight{
		add{ HeroMoveRight+=value;}
		remove{ HeroMoveRight-=value;}
	}

	private Action HeroMoveLeft;
	public event Action OnHeroMoveLeft{
		add{ HeroMoveLeft+=value;}
		remove{ HeroMoveLeft-=value;}
	}

	private Action HeroFireWeapon;
	public event Action OnHeroFireWeapon{
		add{ HeroFireWeapon+=value;}
		remove{ HeroFireWeapon-=value;}
	}

	private Action HeroJump;
	public event Action OnHeroJump{
		add{ HeroJump+=value;}
		remove{ HeroJump-=value;}
	}

	private Action HeroHit;
	public event Action OnHeroHit{
		add{ HeroHit+=value;}
		remove{ HeroHit-=value;}
	}

	private Action <LevelObjectTagger>HeroHitLevelObject;
	public event Action <LevelObjectTagger>OnHeroHitLevelObject{
		add{ HeroHitLevelObject+=value;}
		remove{ HeroHitLevelObject-=value;}
	}

	private Action HeroDied;
	public event Action OnHeroDied{
		add{ HeroDied+=value;}
		remove{ HeroDied-=value;}
	}

	private Action HeroRevive;
	public event Action OnHeroRevive{
		add{ HeroRevive+=value;}
		remove{ HeroRevive-=value;}
	}

	private Action <AttackType>AttackComplete;
	public event Action <AttackType>OnAttackComplete{
		add{AttackComplete+=value;}
		remove{AttackComplete-=value;}
	}

	private Action <AttackType>MidAttackComplete;
	public event Action <AttackType>OnMidAttackComplete{
		add{MidAttackComplete+=value;}
		remove{MidAttackComplete-=value;}
	}

	private Action <AttackType>MidNearAttackComplete;
	public event Action <AttackType>OnMidNearAttackComplete{
		add{MidNearAttackComplete+=value;}
		remove{MidNearAttackComplete-=value;}
	}

	private Action <AttackType>NearAttackComplete;
	public event Action <AttackType>OnNearAttackComplete{
		add{NearAttackComplete+=value;}
		remove{NearAttackComplete-=value;}
	}

	private Action FaceLeft;
	public event Action OnFaceLeft{
		add{FaceLeft+=value;}
		remove{FaceLeft-=value;}
	}

	private Action FaceRight;
	public event Action OnFaceRight{
		add{FaceRight+=value;}
		remove{FaceRight-=value;}
	}

	private Action Attacking;
	public event Action OnAttacking{
		add{Attacking+=value;}
		remove{Attacking-=value;}
	}

	//optimzation
	private Transform heroTransform;

	public virtual void Start(){
		id = UniqueIdGenerator.GenerateId();
		controller = GetComponent<CharacterController>();
		heroTransform = this.gameObject.transform;
		ResetState();
	}

	public void ResetState(){
		ResetStateOnLand();
		//es
		isFacingRight =true;
		isHit  =false;
		isDead = false;
		isIdle =false;
		// es
	}

	private void ResetStateOnLand(){
		isInAir =false;
		isJumping =false;
		isDoubleJumping =false;
		isWalking =true;
		isRunning = false;
		//isFalling = true;
		isFalling = false;
		isBouncing = false;
		jumpDistance=0;
		moveDirection = Vector3.zero;
	}

	public bool isHitRightSide{
		set{hitRightSide=value;}
		get{ return hitRightSide;}
	}

	public bool isHitLeftSide{
		set{hitLeftSide=value;}
		get{ return hitLeftSide;}
	}

	public bool isHitUpSide{
		set{hitUpSide=value;}
		get{ return hitUpSide;}
	}

	public bool isHitDownSide{
		set{hitDownSide=value;}
		get{ return hitDownSide;}
	}


	public virtual void OnControllerColliderHit(ControllerColliderHit hit){
		//Debug.Log("Hero Controller hit something hit what " + hit.gameObject.name);
		//Debug.Log(" check hit.moveDirection.x " + hit.moveDirection.x);
		if(hit.moveDirection.x < -0.99f){
			//hit left side
			hitLeftSide = true;
			hitRightSide =false;
			//Debug.Log("hit left");
		}else if(hit.moveDirection.x > 0.99f){
			//hit right side
			hitLeftSide = false;
			hitRightSide =true;
			//Debug.Log("hit right");
		}

		//Debug.Log("Hero Controller collided with " + hit.gameObject.tag);
		//Debug.Log(" hit.point " + hit.point);
		Debug.DrawRay ( hit.point, hit.normal,Color.red );
		if ( hit.moveDirection.y > 0.01){
			hitDownSide =false;
			hitUpSide =true;
		}else if ( hit.moveDirection.y < -0.99f){
			ResetStateOnLand();
			hitDownSide =true;
			hitUpSide =false;
			//Debug.Log("hit down");
		}else  if (hit.normal.y < 0.707){
			//character collided with another thing (not the ground)
			//Debug.Log("character collided with another thing (not the ground): " + hit.gameObject.tag);
		}

		levelObjectTagger = hit.gameObject.GetComponent<LevelObjectTagger>();			
		if(levelObjectTagger!=null){
			if(null!=HeroHitLevelObject){
				HeroHitLevelObject(levelObjectTagger);
			}				
		}else{
			levelObjectTagger = hit.gameObject.GetComponentInChildren<LevelObjectTagger>();
			if(levelObjectTagger!=null){
				if(null!=HeroHitLevelObject){
					HeroHitLevelObject(levelObjectTagger);
				}					
			}
		}
	}

	public void BrickRebound(){
		if(!isFalling){
			moveDirection.y -= speedY * brickReboundForce;
		}
	}

	public virtual void Update(){
		if(isDisableMovement) return;
		UpdateMovement();
		UpdateGravity();
		LimitSpeed();
		LimitJumpSpeed();
	}

	public virtual void LimitSpeed(){
		if(alwaysRun){
			isRunning =true;
		}

		if(isFacingRight){
			if(isRunning){
				if(speed>runMaxSpeed){
					speed=runMaxSpeed;
					//Debug.Log("reach max walking  right");
				}
			}else if(isWalking){
				if(speed>walkMaxSpeed){
					speed=walkMaxSpeed;
					//Debug.Log("reach max walking right");
				}
			}else if(isJumping || isFalling){
				if(speed>airMaxSpeed){
					speed=airMaxSpeed;
					//Debug.Log("reach max walking  right");
				}
			}
			if(null!=FaceRight){
				FaceRight();
			}
			//Debug.Log( "lmit walk speed id " + id );
		}else if(isFacingLeft){
			if(isRunning){
				if(speed<-runMaxSpeed){
					speed=-runMaxSpeed;
					//Debug.Log("reach running max left");
				}
			}else if(isWalking){
				if(speed<-walkMaxSpeed){
					speed=-walkMaxSpeed;
					//Debug.Log("reach walkingmax left");
				}
			}else if(isJumping || isFalling){
				if(speed<-airMaxSpeed){
					speed=-airMaxSpeed;
					//Debug.Log("reach running max left");
				}
			}

			if(null!=FaceLeft){
				FaceLeft();
			}
			//Debug.Log( "lmit run speed id " + id );
		}
	}

	private void LimitJumpSpeed(){
		if(moveDirection.y >= jumpMaxSpeed){
			//Debug.Log(" exceed going up moveDirection.y " + moveDirection.y);
			moveDirection.y = jumpMaxSpeed;
		}
		
		if(isFalling){
			if(moveDirection.y <= -fallMaxSpeed){
				//Debug.Log(" exceed going down moveDirection.y " + moveDirection.y);
				moveDirection.y = -fallMaxSpeed;
			}
		}
	}


	private void UpdateMovement(){
		if (isLeftBtnPress && !isDead){
			isIdle =false;
			MoveLeft();
		}else if (isRightBtnPress && !isDead){
			isIdle =false;
			MoveRight();
		}else if (!isLeftBtnPress && !isRightBtnPress && !isDead){
			isIdle =true;
			isWalking =false;
			isRunning =false;
			Deceleration();
		}else if (isDead){
			isIdle =false;
			isWalking =false;
			isRunning =false;
			Deceleration();
		}

		if((isJumping || isDoubleJumping || isBouncing) && !isDead){
		//if((isJumping || isDoubleJumping) && !isDead){
			MoveUp();
		}

		moveDirection.x = Mathf.Lerp(moveDirection.x, speed, 1f);
		//moveDirection.x = speed;

		//dont call this per frame
		if(controller!=null){
			controller.Move( moveDirection * Time.deltaTime);
		}
		//Debug.Log("Hero x" + this.gameObject.transform.position.x);
	}

	private void UpdateGravity(){
		if(!ApplyGravity)return;

		//newPositionY = this.gameObject.transform.position.y;
		newPositionY = heroTransform.position.y;
		if(oldPositionY != newPositionY){
			if(newPositionY > oldPositionY){
				//Debug.Log("jumping");
				isFalling =false;
			}else{
				//Debug.Log("falling");
				isFalling =true;
			}
			oldPositionY = newPositionY;
			//Debug.Log("update position y");
		}else{
			//Debug.Log("dont update position y");
		}

		float curSmooth = wallSlideSmoothing * Time.deltaTime;
		float fallSpeed;

		if(isHitWall && moveDirection.y < 0){
			fallSpeed = wallSlideSpeed * Time.deltaTime;
			moveDirection.y = Mathf.Lerp(moveDirection.y, moveDirection.y - fallSpeed, curSmooth);
		}else{
			moveDirection.y = Mathf.Lerp(moveDirection.y,moveDirection.y -(gravity * Time.deltaTime),1f);
		}

		speedY = moveDirection.y;
	}

	private void Deceleration(){
		if(speed > friction * Time.deltaTime){
			speed = Mathf.Lerp(speed, speed - friction * Time.deltaTime, decelerationTime);
		}else if(speed < -friction * Time.deltaTime){
			speed = Mathf.Lerp(speed, speed + friction * Time.deltaTime,decelerationTime);
		}else{
			speed = 0;
		}
		//Debug.Log("deceleration!...");
	}

	public void Kill(){
		if(!IsDead){
			IsDead = true;
		}
	}


	public bool Hit(){
		if(!isHit && !isDead && !isAttacking){
			speed=0;
			isHit =true;
			if(null!=HeroHit){
				HeroHit();
			}

			Invoke(Task.RefreshHit.ToString(),hitDelay);
			Invoke(Task.RefreshMidHit.ToString(),hitDelay * 0.5f);
			return true;
		}else{
			return false;
		}
	}

	private void RefreshHit(){
		isHit =false;
		//Debug.Log("hit refresh");
		if(null!= HitComplete){
			HitComplete();
		}
	}

	private void RefreshMidHit(){
		if(null!=MidHit){
			MidHit();
		}
	}


	//0-1
	public void Bounce( float percent = 0.5f ){
		if(isDead) return;

		isBouncing = true;
		moveDirection = Vector3.zero;
		jumpDistance = jumpHeight * percent;
		isIdle =false;
		isInAir =true;
	}

	public bool IsDownBtnPress{
		set{ isDownBtnPress =value;
			if(isDownBtnPress){
				if(null!=HeroMoveDown){
					HeroMoveDown();
				}
			}
		}

		get{return isDownBtnPress;}
	}

	public void FireWeapon(){
		if(null!=HeroFireWeapon){
			HeroFireWeapon();
		}
	}

	public void Jump(){
		if(isDead) return;

		if(!isInAir && !isFalling){
			moveDirection = Vector3.zero;
			jumpDistance=0;
			isJumping =true;
			isIdle =false;
			isInAir =true;
			if(null!=HeroJump){
				HeroJump();
			}
		}else if(!isJumping && isInAir && hasDoubleJump && !isDoubleJumping && !isHitWall){
			moveDirection = Vector3.zero;
			jumpDistance=0;
			isDoubleJumping =true;
			isIdle =false;
			isInAir =true;

			if(null!=HeroJump){
				HeroJump();
			}
		}

		if(isHitWall && isInAir && moveDirection.y < 0 ){
			jumpDistance=0;
			isJumping =true;


			if(isMovingLeft){
				WallJumpRight();
				isFacingLeft =false;
				isFacingRight =true;

				isMovingLeft = false;
				isMovingRight =true;
			}else if(isMovingRight){
				WallJumpLeft();
				isFacingLeft =true;
				isFacingRight=false;

				isMovingLeft = true;
				isMovingRight =false;
			}
		}
	}

	private void MoveUp(){
		if(jumpDistance< jumpHeight){
			jumpDistance+=jumpSpeed * Time.deltaTime;
			moveDirection.y = Mathf.Lerp( moveDirection.y,moveDirection.y + jumpSpeed * Time.deltaTime ,1f);
		}
	}

	public void Bounce2(float val){
		if(isDead) return;		

		isIdle =false;
		isInAir =true;
		moveDirection.y += val;
	}

	public void AirMoveUp(float val){
		moveDirection.y += val;
	}

	public void AirMoveDown(float val){
		moveDirection.y -= val;
	}

	public void MoveLeft(){
		if (isInAir || canMoveInAir){
			isMovingLeft =true;
			isMovingRight =false;

			isFacingLeft =true;
			isFacingRight =false;

			if(speed > 0){
				speed = 0;
			}

			if(isInAir){
				speed= (speed - airAcceleration * Time.deltaTime);
			}else{
				speed= (speed - acceleration  * Time.deltaTime);
			}

			if(isConstantSpeed){
				speed = -walkMaxSpeed;
			}
			if(null!=HeroMoveLeft){
				HeroMoveLeft();
			}
		}
	}

	private void WallJumpLeft(){
		speed = 0;
		speed= (speed - wallJumpPower  * Time.deltaTime);
	}

	private void WallJumpRight(){
		speed = 0;
		speed= (speed + wallJumpPower  * Time.deltaTime);
	}

	public void MoveRight(){
		if (!isInAir || canMoveInAir){
			isMovingLeft =false;
			isMovingRight =true;

			isFacingLeft =false;
			isFacingRight =true;

			if(speed < 0){
				speed = 0;
			}

			if(isInAir){
				speed= (speed + airAcceleration * Time.deltaTime);
			}else{
				speed= (speed + acceleration * Time.deltaTime);
			}

			if(isConstantSpeed){
				speed = walkMaxSpeed;
			}

			if(null!=HeroMoveRight){
				HeroMoveRight();
			}
		}
	}


	public void StopMoving(){
		isLeftBtnPress =false;
		isRightBtnPress =false;

		speed =0;
		isIdle =true;
		isWalking =false;
		isRunning =false;
		moveDirection = Vector3.zero;
	}

	public void StartMoving(){
		isIdle =false;
		isWalking =true;
		isRunning =false;
	}

	public void Attack(AttackType attackType){
		if(!isAttacking){
			currentAttackType =attackType;
			isAttacking = true;
			Invoke(Task.RefreshMidAttack.ToString(),attackDelay*0.5f);
			Invoke(Task.RefreshMidNearAttack.ToString(),attackDelay*0.65f);
			Invoke(Task.RefreshNearAttack.ToString(),attackDelay*0.75f);
			Invoke(Task.RefreshAttack.ToString(),attackDelay);
			if(null!=Attacking){
				Attacking();
			}
		}
	}

	private void RefreshAttack(){
		isAttacking =false;
		if(null!=AttackComplete){
			AttackComplete(currentAttackType);
		}
	}

	private void RefreshMidNearAttack(){
		if(null!=MidNearAttackComplete){
			MidNearAttackComplete(currentAttackType);
		}
	}

	private void RefreshMidAttack(){
		if(null!=MidAttackComplete){
			MidAttackComplete(currentAttackType);
		}
	}

	private void RefreshNearAttack(){
		if(null!=NearAttackComplete){
			NearAttackComplete(currentAttackType);
		}
	}

	private void Message(string msg){
		Debug.Log(msg);
	}
}