using UnityEngine;
using System.Collections;

public class RightBtn : MonoBehaviour {
	
	private HeroController heroController;
	private bool isPressed=false;
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

	private void Update(){
		if(heroController==null)return;

		if(gameDataManager.IsLevelComplete){
			heroController.isRightBtnPress =false;
			return;
		}

		if(isPressed && !heroController.IsDead ){
			heroController.isRightBtnPress = true;
			heroController.isLeftBtnPress = false;
		}else{
			heroController.isRightBtnPress =false;
		}
	}
	
	private void OnPress(bool isDown){
		isPressed = isDown;

		/*
		heroController.isRightBtnPress = isDown;
		if(isPressed && !heroController.isDead ){
			heroController.isWalking =true;
			heroController.isRunning =false;
		}*/
	}
}
