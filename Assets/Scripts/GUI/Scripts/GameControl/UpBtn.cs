using UnityEngine;
using System.Collections;

public class UpBtn : MonoBehaviour {
	
	private HeroController heroController;
	private bool isPressed=false;
	public LevelManager levelManager;
	private GameDataManager gameDataManager;

	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		AddEventListner();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListner(){
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
		heroController = levelManager.heroInstance.GetComponent<HeroController>();
	}

	private void OnGameRestart(){
		heroController = levelManager.heroInstance.GetComponent<HeroController>();
	}
	
	private void Update(){
		if(heroController==null)return;

		if(gameDataManager.IsLevelComplete){
			heroController.isLookingUp =false;
			return;
		}

		if(isPressed && !heroController.IsDead){
			heroController.isLookingUp = true;
			heroController.isLookingDown = false;
		}else{
			heroController.isLookingUp =false;
		}
	}
	
	private void OnPress(bool isDown){
		isPressed = isDown;
	}
}
