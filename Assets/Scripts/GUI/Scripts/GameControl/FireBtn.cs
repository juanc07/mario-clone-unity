using UnityEngine;
using System.Collections;

public class FireBtn : MonoBehaviour {

	private HeroController heroController;
	private bool isPressed =false;
	private GameDataManager gameDataManager;
	public LevelManager levelManager;

	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		gameDataManager.OnLevelStart+=OnLevelStart;
		gameDataManager.OnGameRestart+=OnGameRestart;
	}
	
	private void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnLevelStart-=OnLevelStart;
			gameDataManager.OnGameRestart-=OnGameRestart;
		}
	}
	
	private void OnLevelStart(){
		heroController = levelManager.heroInstance.gameObject.GetComponent<HeroController>();
	}
	
	private void OnGameRestart(){
		heroController = levelManager.heroInstance.gameObject.GetComponent<HeroController>();
	}
	
	private void OnClick(){
		//Debug.Log("fire!!");
	}


	private void Run(){
		if(heroController==null)return;

		heroController.isRunning = true;
		heroController.isWalking = false;
	}

	private void Walk(){
		if(heroController==null)return;

		heroController.isRunning = false;
		heroController.isWalking = true;
	}


	private void Update(){
		if(heroController==null)return;

		if(gameDataManager.IsLevelComplete){
			heroController.isHoldingAction=false;
			heroController.isWalking = false;
			heroController.isRunning = false;
			return;
		}

		if(isPressed){
			/*if(!heroController.isInAir && !heroController.IsDead){
				Run();
			}else */

			if( gameDataManager.player.IsGotFireball){
				heroController.FireWeapon();
				Debug.Log("hold fire!!");
			}
		}else{
			heroController.isHoldingAction=false;
			if(heroController.alwaysRun){
				Run();
			}else{
				Walk();
			}
		}
	}

	private void OnPress(bool isDown){
		if(heroController==null)return;

		isPressed = isDown;
		if(isPressed && heroController.isIdle){
			if( gameDataManager.player.IsGotFireball){
				heroController.FireWeapon();
			}else{
				if(!heroController.isHoldingSomething){
					heroController.isHoldingAction=true;
				}else if(  heroController.isHoldingSomething){
					heroController.isThrow =true;
				}
			}
		}
	}
}
