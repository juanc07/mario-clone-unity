using UnityEngine;
using System.Collections;

public abstract class CharacterAnimationController : MonoBehaviour {

	public GameObject model;
	public HeroController heroController;
	private bool hasPlayedJump =false;
	public Animation modelAnimation{set;get;}
	private bool isHitPlayed =false;
	private bool isDeathPlayed =false;
	private bool isAttackPlayed =false;

	private Quaternion tempRotation;

	public enum Animations{idle,walk,run,jump,hit,falling,falling2,death,attack01,attack02,attack03,attack04,attack05}

	private GameDataManager gameDataManager;
	private Transform modelTransform;

	public bool disableFacingLeftAndRight;

	// Use this for initialization
	public virtual void Start () {
		gameDataManager = GameDataManager.GetInstance();
		modelAnimation = model.gameObject.GetComponent<Animation>();
		modelTransform = model.gameObject.transform;
		AddEventListener();
	}

	private void AddEventListener(){
		//heroController.OnHeroHit+=OnHeroHit;
		heroController.OnHitComplete += OnHitComplete;
		heroController.OnHeroRevive+= OnHeroRevive;
		//heroController.OnFaceLeft+=OnFaceLeft;
		//heroController.OnFaceRight+=OnFaceRight;
		//heroController.OnHeroDied+=OnHeroDied;
		//heroController.OnAttacking+=OnAttacking;

		gameDataManager.OnGameRestart+= OnGameRestart;
	}
	
	private void RemoveEventListener(){
		//heroController.OnHeroHit-=OnHeroHit;
		heroController.OnHitComplete -= OnHitComplete;
		heroController.OnHeroRevive-= OnHeroRevive;
		//heroController.OnFaceLeft-=OnFaceLeft;
		//heroController.OnFaceRight-=OnFaceRight;
		//heroController.OnHeroDied-=OnHeroDied;
		//heroController.OnAttacking-=OnAttacking;

		if(gameDataManager!=null){
			gameDataManager.OnGameRestart-= OnGameRestart;
		}
	}
	
	public virtual void OnDestroy(){
		RemoveEventListener();
		//Debug.Log("on destory hammer bro controller");
	}

	private void OnHeroRevive(){
		isDeathPlayed =false;
		//PlayIdle();
	}
	
	public virtual void OnGameRestart(){
		PlayWalk();
		isDeathPlayed =false;
	}

	private void OnHitComplete(){
		isHitPlayed = false;
	}

	//new
	/*private void OnHeroHit(){
		if(!isHitPlayed ){
			PlayHit();
		}
	}*/

	//new
	/*private void OnFaceLeft(){
		FaceLeft();
	}*/

	//new
	/*private void OnFaceRight(){
		FaceRight();
	}*/

	//new
	/*private void OnHeroDied(){
		if(!isDeathPlayed ){
			PlayDeath();
		}
	}*/

	//new
	/*private void OnAttacking(){
		if(heroController.IsDead){
			return;
		}

		PlayAttacking();
	}*/

	// Update is called once per frame
	public virtual void Update (){

		/*if(heroController.isHit || heroController.IsDead){
			return;
		}
		
		if(heroController.isInAir){
			if((!hasPlayedJump && !heroController.isFalling) || (heroController.isHitWall && !heroController.isFalling)){
				PlayJump();
			}else{
				PlayFalling2();
			}
		}else{
			if(!heroController.isAttacking){
				if(heroController.isIdle){
					PlayIdle();
				}else if(heroController.isWalking){
					if(heroController.alwaysRun){
						PlayRun();
					}else{
						PlayWalk();
					}
					
				}else if(heroController.isRunning){
					PlayRun();
				}
			}
		}*/		

		if(!disableFacingLeftAndRight){
			if(heroController.isFacingLeft){
				FaceLeft();
			}else if(heroController.isFacingRight){
				FaceRight();
			}
		}

		if(heroController.isHit && !isHitPlayed ){
			PlayHit();
		}else if(heroController.IsDead && !isDeathPlayed ){
			PlayDeath();
		}else{
			if(heroController.isHit || heroController.IsDead){
				return;
			}

			if(heroController.isInAir){
				if((!hasPlayedJump && !heroController.isFalling) || (heroController.isHitWall && !heroController.isFalling)){
					PlayJump();
				}else{
					PlayFalling2();
				}
			}else{
				if(heroController.isAttacking){
					PlayAttacking();
				}else{
					if(heroController.isIdle){
						PlayIdle();
					}else if(heroController.isWalking){
						if(heroController.alwaysRun){
							PlayRun();
						}else{
							PlayWalk();
						}
						
					}else if(heroController.isRunning){
						PlayRun();
					}
				}
			}
		}
	}

	private void PlayAttacking(){
		if(heroController.currentAttackType == AttackType.Attack1){
			Attack1();
		}else if(heroController.currentAttackType == AttackType.Attack2){
			Attack2();
		}else if(heroController.currentAttackType == AttackType.Attack3){
			Attack3();
		}else if(heroController.currentAttackType == AttackType.Attack4){
			Attack4();
		}else if(heroController.currentAttackType == AttackType.Attack5){
			Attack5();
		}
	}

	public virtual void Attack1(){
		isAttackPlayed =true;
	}

	public virtual void Attack2(){
		isAttackPlayed =true;
	}

	public virtual void Attack3(){
		isAttackPlayed =true;
	}

	public virtual void Attack4(){
		isAttackPlayed =true;
	}

	public virtual void Attack5(){
		isAttackPlayed =true;
	}

	public virtual void PlayHit(){
		isHitPlayed =true;
	}

	public virtual void PlayDeath(){
		isDeathPlayed = true;
	}

	public virtual void PlayIdle(){
		hasPlayedJump =false; 
	}

	public virtual void PlayWalk(){
		hasPlayedJump =false; 
	}

	public virtual void PlayRun(){
		hasPlayedJump =false; 
	}

	public virtual void PlayJump(){
		hasPlayedJump =true;
	}

	public virtual void PlayFalling(){
		
	}

	public virtual void PlayFalling2(){
		
	}

	public void FaceLeft(){
		tempRotation =  Quaternion.Euler(0, 270f, 0);
		//model.gameObject.transform.rotation = tempRotation;
		modelTransform.rotation = tempRotation;
	}

	public void FaceRight(){
		tempRotation =  Quaternion.Euler(0, 90f, 0);
		//model.gameObject.transform.rotation = tempRotation;
		modelTransform.rotation = tempRotation;
	}
}
