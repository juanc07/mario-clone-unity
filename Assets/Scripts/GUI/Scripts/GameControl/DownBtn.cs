using UnityEngine;
using System.Collections;

public class DownBtn : MonoBehaviour {
	
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
			heroController.isLookingDown =false;
			return;
		}

		if(isPressed && !heroController.IsDead){
			heroController.isLookingUp = false;
			heroController.isLookingDown = true;
		}else{
			heroController.isLookingDown =false;
		}
	}
	
	private void OnPress(bool isDown){
		if(heroController==null)return;
		isPressed = isDown;
		if(isPressed){
			heroController.IsDownBtnPress = true;
		}else{
			heroController.IsDownBtnPress = false;
		}
	}
}
